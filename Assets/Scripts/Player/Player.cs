using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private List<ItemSkill> itemSkillList = new();


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
    

    
    
    
    

    private Vector2 _input = Vector2.zero;
    private bool isMoving;
    private RaycastHit2D hit;

    private Vector2 targetItemPos;
    private bool moveToItem = false;
    private ItemSkill currentItem;

    private bool isAttacking;

    public bool IsMoving { get { return isMoving; } }
    public bool IsAttacking { get { return isAttacking; } }

    private void Start()
    {
        inventoryManager.SelectSlot(0);
        inventoryManager.status = 0;
      
        
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

        if (inventoryManager.status == 0)
        {
            if (moveToItem)
            {
                Debug.Log("Da nhat");
                MoveToItem();
            }
            else
            {
                Move();
            }
        }else if (inventoryManager.status == 1)
        {
            Attack();
        }


       



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

            Debug.Log(hit.collider);
            //di chuyển nhưng vẫn kiểm tra va chạm
            rb.MovePosition(rb.position + 5 * Time.fixedDeltaTime * _input);
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);

           
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
        if (hit.collider == null) return;
        if ((hit.collider.gameObject.CompareTag("Obstacle") && collision.gameObject.CompareTag("Obstacle")) ||
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

    private void MoveToItem()
    {
        rb.MovePosition(Vector2.MoveTowards( rb.position,  targetItemPos,5 * Time.fixedDeltaTime));
        
        if (Vector2.Distance(rb.position, targetItemPos) < 0.05f)
        {
            inventoryManager.AddItem(currentItem.Data);
            itemSkillList.Add(currentItem);

            Destroy(currentItem.gameObject);

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
        //var item = collision.GetComponent<ItemSkill>();
        //if (item == null) return;
        //Debug.Log("Nhat");
        //inventoryManager.AddItem(item.Data);
        //itemSkillList.Add(item);






        //////TODO: sửa lỗi bị dịch chhuyển đến vị trí vật phẩm chứ không phải di chuyển

        //isMoving = false;
        //_input = Vector2.zero;
        //animator.SetFloat(XKey, _input.x);
        //animator.SetFloat(YKey, _input.y);
        //animator.SetBool(IsMovingKey, isMoving);
        //transform.position = collision.transform.position;

        //Destroy(collision.gameObject);


        var item = collision.GetComponent<ItemSkill>();
        if (item == null) return;
        Debug.Log("Nhat item");

        currentItem = item;
        targetItemPos = collision.transform.position;

        moveToItem = true;
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

    private void OnCompleteAttack()
    {
        Debug.Log("Het tan cong");
        isAttacking = false;
        _input = Vector2.zero;
    }

}
