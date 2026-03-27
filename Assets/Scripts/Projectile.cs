using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform transforms;
    [SerializeField] private Collider2D colliders;
    private RaycastHit2D hitArrow;
    private Vector2 _input = Vector2.zero;

    private Enemy currentEnemy;
    private Transform target;
    private bool moveToEnemy = false;

    public void Init(Vector2 input)
    {
        _input = input;
        Debug.Log("Xoay");
        Debug.Log(_input);
        if (_input.x >= 1f)
        {
            Debug.Log("Xoay cung");
            transforms.Rotate(0f, 0f, 90f);
        }else if (_input.x <= -1f)
        {
            transforms.Rotate(0f, 0f, -90f);
        }else if (_input.y >= 1f)
        {
            transforms.Rotate(0f, 0f, 180f);
        }

    }

    private void Update()
    {
        if (!moveToEnemy)
        {
            Move();
        }
        else
        {
            MoveToEnemy();
        }
    }
    private void Move()
    {
        Vector2 origin = (Vector2)transforms.position + _input * 1f;
        //tạo 1 đường thẳng để kiểm tra có sẽ va chạm với gì không, (vị trí, hướng,độ dài)
        hitArrow = Physics2D.Raycast(origin, _input, 0);

        //Debug.Log(hit.collider);
        //di chuyển nhưng vẫn kiểm tra va chạm
        rb.MovePosition(rb.position + 5 * Time.fixedDeltaTime * _input);
      
    }

    private void MoveToEnemy()
    {
      
        rb.MovePosition(Vector2.MoveTowards(rb.position, target.position, 5 * Time.fixedDeltaTime));

        if (Vector2.Distance(rb.position, target.position) < 0.1f)
        {
        
        
            if (currentEnemy != null)
            {
                currentEnemy.OnHit();
                currentEnemy = null;
            }
            moveToEnemy = false;  
            _input = Vector2.zero;
  
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogError($"Trigger: {collision?.gameObject.name}");

        if (hitArrow.collider == null) return;
        if ((hitArrow.collider.gameObject.CompareTag("Peak") && collision.gameObject.CompareTag("Peak")) ||
            (hitArrow.collider.gameObject.CompareTag("Wall") && collision.gameObject.CompareTag("Wall")))
        {
            _input = Vector2.zero;
            rb.isKinematic = true;

            if (target == null)
            {
                target = collision.transform;
            }
        }

        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    currentEnemy = collision.transform.GetComponent<Enemy>();
        //    if (currentEnemy == null)
        //    {
        //        Debug.LogError("1");

        //        return;
        //    }

        //    if (target == null)
        //    {
        //        target = collision.transform;
        //    }
        //    moveToEnemy = true;
        //}
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.LogError($"Trigger: {collision?.gameObject.name}");

    //    if (hitArrow.collider == null) return;
    //    if (hitArrow.collider.gameObject.CompareTag("Peak") && collision.gameObject.CompareTag("Peak"))
    //    {
    //        _input = Vector2.zero;
    //        rb.isKinematic = true;
    //        target = collision.transform.position;
    //        colliders.isTrigger = true;
    //    }

    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        currentEnemy = collision.transform.GetComponent<Enemy>();
    //        if (currentEnemy == null)
    //        {
    //            Debug.LogError("1");

    //            return;
    //        }
    //        target = collision.transform.position;
    //        moveToEnemy = true;
    //    }

    //    //colliders.isTrigger = true;

    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentEnemy = collision.transform.GetComponent<Enemy>();
            if (currentEnemy == null)
            {
                Debug.LogError("1");

                return;
            }

            if (target == null)
            {
                target = collision.transform;
            }
            moveToEnemy = true;
        }
    }

}

