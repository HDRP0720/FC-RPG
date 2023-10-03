using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController>
{
  public bool isPatrol = true;
  
  private float minIdleTime = 0.0f;
  private float maxIdleTime = 3.0f;
  private float idleTime = 0.0f;
  
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
    
    if (isPatrol)
      idleTime = Random.Range(minIdleTime, maxIdleTime);
  }

  public override void Update(float deltaTime)
  {
    Transform enemy = context.SearchEnemy();
    if (enemy)
    {
      if (context.IsAvailableAttack())
        stateMachine.ChangeState<AttackState>();
      else
        stateMachine.ChangeState<MoveState>();
    }
    else if (isPatrol && stateMachine.GetElapsedTimeInState > idleTime)
    {
      stateMachine.ChangeState<PatrolState>();
    }
  }
}
