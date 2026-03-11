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
   

    private const string XKey = "X";
    private const string YKey = "Y";
    private const string IsMovingKey = "IsMoving";
    //private int status;
    private int countStatus;
    

    private Vector2 _input = Vector2.zero;
    bool isMoving;
    private RaycastHit2D hit;


    private void Start()
    {
        inventoryManager.SelectSlot(0);
        //status = 1;
        countStatus = 1;
    }
    private void Update()
    {
        Move();

    }

    private void ChangeStatus(int index)
    {

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<ItemSkill>();
        if (item == null) return;
        Debug.Log("Nhat");
        inventoryManager.AddItem(item.Data);
        itemSkillList.Add(item);
        countStatus++;

        //TODO: sửa lỗi bị dịch chhuyển đến vị trí vật phẩm
        isMoving = false;
        _input = Vector2.zero;
        animator.SetFloat(XKey, _input.x);
        animator.SetFloat(YKey, _input.y);
        animator.SetBool(IsMovingKey, isMoving);
        transform.position = collision.transform.position;
        
        Destroy(collision.gameObject);
    }


}
