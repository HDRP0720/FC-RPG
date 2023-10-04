using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State<EnemyController>
{
  private Animator animator;
  private CharacterController cc;
  private NavMeshAgent agent;
  
  private readonly int moveHash = Animator.StringToHash("IsMove");
  private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
  
  public override void OnInitialized()
  {
    animator = context.GetComponent<Animator>();
    cc = context.GetComponent<CharacterController>();
    agent = context.GetComponent<NavMeshAgent>();
  }
  
  public override void OnEnter()
  {
    if (context.targetPatrolPoint == null)
      context.FindNextPatrolPoint();
    
    if (context.targetPatrolPoint)
    {
      if (agent != null) 
        agent.SetDestination(context.targetPatrolPoint.position);
      
      if (animator != null)
        animator.SetBool(moveHash, true);
    }
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
    else
    {
      if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
      {
        Transform nextPoint = context.FindNextPatrolPoint();
        if (nextPoint)
          agent.SetDestination(nextPoint.position);
        
        stateMachine.ChangeState<IdleState>();
      }
      else
      {
        cc.Move(agent.velocity * deltaTime);
        animator.SetFloat(moveSpeedHash, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
      }
    }
  }

  public override void OnExit()
  {
    if(animator != null)
      animator.SetBool(moveHash, false);
    
    agent.ResetPath();
  }
}