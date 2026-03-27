using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const string DieKey = "Die";
    [SerializeField] private Animator animator;

    public event Action Hit;

    public void OnHit()
    {
        if (animator != null)
        {
            animator?.SetTrigger(DieKey);
        }
        Hit?.Invoke();
      
    }
    private void OnCompleteAnimationDie()
    {
        LevelManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }

    public void PlayHitDame()
    {
        AudioController.Instance.PlayHitDame();
    }

}
