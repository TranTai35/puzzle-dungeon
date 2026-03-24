using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Peak : MonoBehaviour
{
    private const string IsPeak = "IsPeak";
    [SerializeField] private Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger(IsPeak);
    }

}
