using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const string DieKey = "Die";
    [SerializeField] private Animator animator;
    public void OnHit()
    {
        
        animator.SetTrigger(DieKey);
      
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
