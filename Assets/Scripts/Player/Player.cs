using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;


    private const string XKey = "X";
    private const string YKey = "Y";
    private const string IsMovingKey = "IsMoving";
    

    private Vector2 _input = Vector2.zero;
    bool isMoving;


    private void Update()
    {
        Move();

    }

    private void Move()
    {


        if (!isMoving)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            isMoving = horizontal != 0 || vertical != 0;
            if (horizontal != 0) vertical = 0;
            _input = new Vector2(horizontal, vertical);
        }

        if (isMoving)
        {
            rb.MovePosition(rb.position + 5 * Time.fixedDeltaTime * _input);
            animator.SetFloat(XKey, _input.x);
            animator.SetFloat(YKey, _input.y);
            animator.SetBool(IsMovingKey, isMoving);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
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

   
}
