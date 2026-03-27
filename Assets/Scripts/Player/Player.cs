using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private InventoryManager inventoryManager;
    //[SerializeField] private List<ItemSkill> itemSkillList = new();
    [SerializeField] private ItemSkill itemBoots;
    [SerializeField] private Projectile arrow;
    

    [Header("Attack")]
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Transform downAttackPoint;
    [SerializeField] private Transform leftAttackPoint;
    [SerializeField] private Transform rightAttackPoint;
    [SerializeField] private Transform upAttackPoint;


    private const string XKey = "X";
    private const string YKey = "Y";
    private const string IsMovingKey = "IsMoving";
    private const string IsAttackKey = "Attack";
    private const string IsDieKey = "Die";
    
    
    
    
    
    

    private Vector2 _input = Vector2.zero;
    private bool isMoving;
    private RaycastHit2D hit;

    private Vector2 targetItemPos;
    private bool moveToItem = false;
    private bool moveToProjectile = false;
    private ItemSkill currentItem;
    private Projectile currentProjectile;     

    private bool isAttacking;
    private bool canPlay = true;
    public bool canShot = false;

    public bool IsMoving { get { return isMoving; } }
    public bool IsAttacking { get { return isAttacking; } }



    private void Start()
    {
        InventoryManager.Instance.AddItem(itemBoots.Data);
        //itemSkillList.Add(itemBoots);
        InventoryManager.Instance.SelectSlot(0);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(downAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(leftAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(rightAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(upAttackPoint.position, attackRadius);
    }
    private void Update()
    {

        if (!canPlay) return;
        if (InventoryManager.Instance.itemSelecting.Name == "Boots")
        {
            if (moveToItem || moveToProjectile)
            {
                Debug.Log("Da nhat");
                MoveToItem();
            }
            else
            {
                Move();
            }
        }else if (InventoryManager.Instance.itemSelecting.Name == "Sword")
        {
            Attack();
        }else if (InventoryManager.Instance.itemSelecting.Name == "Bow")
        {
            Shot();
        }


            checkDefeat();



    }

    
    private void Move()
    {

        //Kiểm tra nếu không di chuyển thì mới cho nhận giá trị để xét input để di chuyển
        if (!isMoving)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            isMoving = horizontal != 0 || vertical != 0;
            if (horizontal != 0) vertical = 0;
            _input = new Vector2(horizontal, vertical);
            if (_input != Vector2.zero)
            {
                LevelManager.Instance.UseAction();
            }
            

        }
        
        //Nếu đang di chuyển thì sẽ di chuyển đi khi gặp va chạm rồi mới dừng
        if (isMoving)
        {
            // lấy vị trí ở phần đầu theo hướng player di chuyển
            Vector2 origin = (Vector2)transform.position + _input * 0.6f ;
            //tạo 1 đường thẳng để kiểm tra có sẽ va chạm với gì không, (vị trí, hướng,độ dài)
            hit = Physics2D.Raycast(origin, _input,0);

            Debug.Log(gameObject);
            Debug.Log(gameObject,hit.collider);
            //di chuyển nhưng vẫn kiểm tra va chạm
            rb.MovePosition(rb.position + 5 * Time.fixedDeltaTime * _input);
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);

           
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject);
        if (hit.collider == null) return;
        if ((hit.collider.gameObject.CompareTag("Enemy") && collision.gameObject.CompareTag("Enemy")) ||
            (hit.collider.gameObject.CompareTag("Wall") && collision.gameObject.CompareTag("Wall")))
        {
            if (isMoving)
            {
                isMoving = false;
                _input = Vector2.zero;
                animator.SetFloat(XKey, _input.x);
                animator.SetFloat(YKey, _input.y);
                animator.SetBool(IsMovingKey, isMoving);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hit.collider == null) return;
        if (hit.collider.gameObject.CompareTag("Peak") && collision.gameObject.CompareTag("Peak"))
        {

           
            AudioController.Instance.PlaySoundLose();
            LevelManager.Instance.IsDefeat = true;
            LevelManager.Instance.PlayDefeat();
            animator.SetTrigger(IsDieKey);

        }
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void MoveToItem()
    {
        Debug.Log("Se nhac");
        Debug.Log(targetItemPos);
        AudioController.Instance.PlayHumanPick();
        rb.MovePosition(Vector2.MoveTowards( rb.position,  targetItemPos,5 * Time.fixedDeltaTime));
        
        if (Vector2.Distance(rb.position, targetItemPos) < 0.1f)
        {
            Debug.Log("Se nhat");
            if (currentItem != null)
            {
                InventoryManager.Instance.AddItem(currentItem.Data);
                Destroy(currentItem.gameObject);
                currentItem = null;
                moveToItem = false;
            }
            if (currentProjectile != null)
            {
                canShot = true;
                Destroy(currentProjectile.gameObject);
                currentProjectile = null;
                moveToProjectile = false;
            }

            
            isMoving = false;
            _input = Vector2.zero;
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       
        var item = collision.GetComponent<ItemSkill>();
        if (item != null)
        {
            Debug.Log("Nhat item");

            currentItem = item;
            targetItemPos = collision.transform.position;

            moveToItem = true;

        }

        if (collision.gameObject.CompareTag("Chest"))
        {
            isMoving = false;
            _input = Vector2.zero;
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);
            AudioController.Instance.PlaySoundWin();
            LevelManager.Instance.PlayVictory();
            canPlay = false;
            
            
        }
        Debug.Log("Nhat projectile");
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Nhat projectile");

            currentProjectile = collision.GetComponent<Projectile>();
            targetItemPos = collision.transform.position;
            if (isMoving)
            {
                moveToProjectile = true;
            }
            
        }
    


    }

    private void Attack()
    {
        if (isAttacking) return;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == 0 && vertical == 0) return;
        if (horizontal != 0) vertical = 0;
        _input = new Vector2(horizontal, vertical);
        isAttacking = true;
        LevelManager.Instance.UseAction();

        animator.SetFloat(XKey, horizontal);
        animator.SetFloat(YKey, vertical);
        animator.SetTrigger(IsAttackKey);
        AudioController.Instance.PlaySoundAttack();

        if (_input.y >= 1f)
        {
            CheckAttack(upAttackPoint);
        }
        else if (_input.y <= -1f)
        {
            CheckAttack(downAttackPoint);
        }
        else if (_input.x >= 1f)
        {
            CheckAttack(rightAttackPoint);
        }
        else if (_input.x <= -1f)
        {
            CheckAttack(leftAttackPoint);
        }

    }

    private void CheckAttack(Transform attackPoint)
    {
        var collider = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayerMask);
        Debug.Log(collider == null);
        if (collider != null && collider.GetComponent<Enemy>() != null)
        {
            Debug.Log("Danh trung enemy");
            collider.GetComponent<Enemy>().OnHit();
        }
    }

    private void Shot()
    {
        if (canShot == false) return;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == 0 && vertical == 0) return;
        if (horizontal != 0) vertical = 0;
        _input = new Vector2(horizontal, vertical);
        if (_input.y >= 1f)
        {
            ShotArrow(upAttackPoint);
        }
        else if (_input.y <= -1f)
        {
            ShotArrow(downAttackPoint);
        }
        else if (_input.x >= 1f)
        {
            ShotArrow(rightAttackPoint);
        }
        else if (_input.x <= -1f)
        {
            ShotArrow(leftAttackPoint);
        }
        LevelManager.Instance.UseAction();
        canShot = false;
    }

    private void ShotArrow(Transform attackPoint)
    {
        var clone = Instantiate(arrow, attackPoint.position, attackPoint.rotation);
        clone.Init(_input);
    }

    private void OnCompleteAttack()
    {
        Debug.Log("Het tan cong");
        isAttacking = false;
        _input = Vector2.zero;
    }

    private void PlayWalk()
    {
        AudioController.Instance.PlayHumanWalk();
    }

    private void checkDefeat()
    {
        LevelManager.Instance.PlayDefeat();
        if (LevelManager.Instance.IsDefeat == true)
        {
            AudioController.Instance.PlaySoundLose();
            isMoving = false;
            _input = Vector2.zero;
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);
            canPlay = false;
        }

    }

}
