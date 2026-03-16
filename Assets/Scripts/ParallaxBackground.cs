using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float xMax;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 _basePos;

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (transform.position.x <= xMax)
        {
            transform.position = _basePos;
        }

        transform.Translate(speed * Time.deltaTime * new Vector3(-1f, 0f, 0f));
    }
}
