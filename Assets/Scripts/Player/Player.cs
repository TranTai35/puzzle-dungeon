using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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

    public void SetCanPlay(bool Play) => canPlay = Play;
    private void Update()
    {

        if (!canPlay) return;
        if (InventoryManager.Instance.itemSelecting.Name == "Boots")
        {
            if (moveToItem || moveToProjectile)
            {
                
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
            //float horizontal = Input.GetAxisRaw("Horizontal");
            //float vertical = Input.GetAxisRaw("Vertical");
            //if (horizontal != 0) vertical = 0;
            //_input = new Vector2(horizontal, vertical);
            checkCanAction();
            isMoving = _input.x != 0 || _input.y != 0;
            if (_input != Vector2.zero)
            {
                GameController.Instance.UseAction();
            }
            

        }
        
        //Nếu đang di chuyển thì sẽ di chuyển đi khi gặp va chạm rồi mới dừng
        if (isMoving)
        {
            // lấy vị trí ở phần đầu theo hướng player di chuyển
            Vector2 origin = (Vector2)transform.position + _input * 0.6f ;
            //tạo 1 đường thẳng để kiểm tra có sẽ va chạm với gì không, (vị trí, hướng,độ dài)
            hit = Physics2D.Raycast(origin, _input,0);

           
            //di chuyển nhưng vẫn kiểm tra va chạm
            rb.MovePosition(rb.position + 5 * Time.fixedDeltaTime * _input);
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);

           
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (hit.collider == null) return;
        if ((hit.collider.gameObject.CompareTag("Enemy") && collision.gameObject.CompareTag("Enemy")) ||
            (hit.collider.gameObject.CompareTag("Wall") && collision.gameObject.CompareTag("Wall")) ||
            (hit.collider.gameObject.CompareTag("Obstacle") && collision.gameObject.CompareTag("Obstacle")))
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
            GameController.Instance.IsDefeat = true;
            GameController.Instance.PlayDefeat();
            animator.SetTrigger(IsDieKey);

        }
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void MoveToItem()
    {
      
        rb.MovePosition(Vector2.MoveTowards( rb.position,  targetItemPos,5 * Time.fixedDeltaTime));
        
        if (Vector2.Distance(rb.position, targetItemPos) < 0.15f)
        {
            
            
            if (currentItem != null)
            {
                InventoryManager.Instance.AddItem(currentItem.Data);
                Destroy(currentItem.gameObject);
                currentItem = null;
            }
            if (currentProjectile != null)
            {
                canShot = true;
                Destroy(currentProjectile.gameObject);
                currentProjectile = null;
                moveToProjectile = false;
            }

            moveToItem = false;
            isMoving = false;
            _input = Vector2.zero;
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        var item = collision.GetComponent<ItemSkill>();
        if (item != null)
        {
            

            currentItem = item;
            targetItemPos = collision.transform.position;
            moveToItem = true;
            return;

        }

        if (collision.gameObject.CompareTag("Chest"))
        {
            isMoving = false;
            _input = Vector2.zero;
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);
            GameController.Instance.PlayVictory();
            canPlay = false;
            
            
        }

        if (hit.collider == null) return;
        if (collision.gameObject.CompareTag("Chair") && hit.collider.gameObject.CompareTag("Chair"))
        {
            targetItemPos = collision.transform.position;
            moveToItem = true;
            return;


        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
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
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        //if (horizontal != 0) vertical = 0;
        //_input = new Vector2(horizontal, vertical);
        checkCanAction();

        if (_input.x == 0 && _input.y == 0) return;

        isAttacking = true;
        GameController.Instance.UseAction();

        animator.SetFloat(XKey, _input.x);
        animator.SetFloat(YKey, _input.y);
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
        
        if (collider != null && collider.GetComponent<Enemy>() != null)
        {
            collider.GetComponent<Enemy>().OnHit();
        }
    }

    private void Shot()
    {
        if (canShot == false) return;
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        //if (horizontal != 0) vertical = 0;
        //_input = new Vector2(horizontal, vertical);
        checkCanAction();
        if (_input.x == 0 && _input.y == 0) return;

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
        GameController.Instance.UseAction();
        canShot = false;
    }

    private void ShotArrow(Transform attackPoint)
    {
        var clone = Instantiate(arrow, attackPoint.position, attackPoint.rotation);
        clone.Init(_input);
    }

    private void checkCanAction()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0) vertical = 0;
        _input = new Vector2(horizontal, vertical);
        if (_input.y >= 1f)
        {
            checkPoint(upAttackPoint);
        }
        else if (_input.y <= -1f)
        {
            checkPoint(downAttackPoint);

        }
        else if (_input.x >= 1f)
        {
            checkPoint(rightAttackPoint);

        }
        else if (_input.x <= -1f)
        {
            checkPoint(leftAttackPoint);

        }
    }

    private void checkPoint(Transform Point)
    {
        var collider = Physics2D.OverlapCircle(Point.position, attackRadius);
        if (collider == null) return;


       

        if (collider.gameObject.CompareTag("Peak")
            ||collider.gameObject.CompareTag("Wall") 
            ||collider.gameObject.CompareTag("Obstacle") )
        {
            
            _input = Vector2.zero;
        }
    }
    

    private void OnCompleteAttack()
    {
        
        isAttacking = false;
        _input = Vector2.zero;
    }

    private void PlayWalk()
    {
        AudioController.Instance.PlayHumanWalk();
    }

    private void checkDefeat()
    {
        GameController.Instance.PlayDefeat();
        if (GameController.Instance.IsDefeat == true)
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
