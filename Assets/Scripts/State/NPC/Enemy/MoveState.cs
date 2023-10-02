using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<EnemyController>
{
  private Animator animator;
  private CharacterController cc;
  private NavMeshAgent agent;
  
  private readonly int moveHash = Animator.StringToHash("Move");
  private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
  
  public override void OnInitialized()
  {
    animator = context.GetComponent<Animator>();
    cc = context.GetComponent<CharacterController>();
    agent = context.GetComponent<NavMeshAgent>();
  }

  public override void OnEnter()
  {
    if (agent != null)
      agent.SetDestination(context.target.position);
    
    if(animator != null)
      animator.SetBool(moveHash, true);
  }
  
  public override void Update(float deltaTime)
  {
    Transform enemy = context.SearchEnemy();
    if (enemy)
    {
      agent.SetDestination(context.target.position);
      if (agent.remainingDistance > agent.stoppingDistance)
      {
        cc.Move(agent.velocity * deltaTime);
        animator.SetFloat(moveSpeedHash, agent.velocity.magnitude/agent.speed, 1f, deltaTime);
        return;
      }
    }
    
    if (!enemy && agent.remainingDistance <= agent.stoppingDistance)
      stateMachine.ChangeState<IdleState>();
  }

  public override void OnExit()
  {
    if (animator != null)
    {      
      animator.SetBool(moveHash, false);
      animator.SetFloat(moveSpeedHash, 0f);
    }
    
    agent.ResetPath();
  }
}
