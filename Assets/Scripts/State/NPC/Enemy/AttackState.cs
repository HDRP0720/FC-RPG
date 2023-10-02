using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
  private Animator animator;
  
  private readonly int attackHash = Animator.StringToHash("Attack");

  public override void OnInitialized()
  {
    animator = context.GetComponent<Animator>();
  }
  
  public override void OnEnter()
  {
    if (context.IsAvailableAttack())
    {
      if (animator != null)
        animator.SetTrigger(attackHash);
      else
        stateMachine.ChangeState<IdleState>();
    }
  }

  public override void Update(float deltaTime)
  {
    
  }
}
