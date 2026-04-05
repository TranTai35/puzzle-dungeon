using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<Enemy> list = new();
    private const string DieKey = "Die";

    private void Awake()
    {
        if (list == null || list.Count == 0) return;
        for(int i = 0; i < list.Count; i++)
        {
            list[i].Hit += OnHit;
        }


    }

    private void OnDestroy()
    {
        if (list == null || list.Count == 0) return;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Hit -= OnHit;
        }
    }

    private void OnHit()
    {
        
        if (animator != null)
        {
            animator?.SetTrigger(DieKey);
        }
        
    }

    
}
