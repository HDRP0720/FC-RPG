using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController>
{
  private Animator animator;
  private CharacterController cc;
  
  private readonly int moveHash = Animator.StringToHash("Move");
  private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
  
  public override void OnInitialized()
  {
    animator = context.GetComponent<Animator>();
    cc = context.GetComponent<CharacterController>();
  }
  
  public override void OnEnter()
  {
    if (animator != null)
    {
      animator.SetBool(moveHash, false);
      animator.SetFloat(moveSpeedHash, 0);
    }

    if (cc != null)
      cc.Move(Vector3.zero);
  }

  public override void Update(float deltaTime)
  {
    Transform enemy = context.SearchEnemy();  // TODO: SearchEnemy
    if (enemy)
    {
      if (context.IsAvailableAttack())        // TODO: IsAvailableAttack
      {
        stateMachine.ChangeState<AttackState>();
      }
      else
      {
        stateMachine.ChangeState<MoveState>();
      }
    }
  }
}
