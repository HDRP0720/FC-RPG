using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IAttackable, IDamageable
{
  public float attackRange;
  public Transform projectileTransform;
  
  [Header("Patrol settings")]
  public bool isPatrol = false;
  public Transform[] patrolPoints;
  [HideInInspector] public Transform targetPatrolPoint;
  
  private StateMachine<EnemyController> stateMachine;
  private EnemyFOV fov;
  private int patrolpointIndex = 0;
  private IdleState idleState = new IdleState();
  private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

  // getter
  public StateMachine<EnemyController> GetStateMachine => stateMachine;
  public Transform GetTarget => fov?.GetNearestTarget;
  public LayerMask GetTargetMask => fov.targetMask;
  public AttackBehaviour CurrentAttackBehaviour { get; private set; }
  public bool IsAlive { get; private set; }

  private void Start()
  {
    stateMachine = new StateMachine<EnemyController>(this, new PatrolState());
    stateMachine.AddState(idleState);
    stateMachine.AddState(new MoveState());
    stateMachine.AddState(new AttackState());
    stateMachine.AddState(new DeadState());

    fov = GetComponent<EnemyFOV>();
    
    InitAttackBehaviour();
  }
  private void Update()
  {
    idleState.isPatrol = isPatrol;
    
    stateMachine.Update(Time.deltaTime);
    
    CheckAttackBehaviour();

    // Debug.Log(stateMachine.GetCurrentState);
  }

  public Transform SearchEnemy()
  {
    return GetTarget;
  }

  public bool IsAvailableAttack()
  {
    if (!GetTarget) return false;

    float distance = Vector3.Distance(transform.position, GetTarget.position);
    return distance <= attackRange;
  }

  public Transform FindNextPatrolPoint()
  {
    targetPatrolPoint = null;
    if (patrolPoints.Length > 0)
      targetPatrolPoint = patrolPoints[patrolpointIndex];

    patrolpointIndex = (patrolpointIndex + 1) % patrolPoints.Length;
      
    return targetPatrolPoint;
  }
  
  public void OnExecuteAttack(int attackIndex)
  {
    if(CurrentAttackBehaviour != null && GetTarget != null)
      CurrentAttackBehaviour.ExcuteAttack(GetTarget.gameObject, projectileTransform);
  }

  public void TakeDamage(int damage, GameObject hitEffectPrefab)
  {
    
  }

  private void InitAttackBehaviour()
  {
    foreach (AttackBehaviour behaviour in attackBehaviours)
    {
      if (CurrentAttackBehaviour == null)
        CurrentAttackBehaviour = behaviour;

      behaviour.targetMask = GetTargetMask;
    }
  }
  private void CheckAttackBehaviour()
  {
    if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
    {
      CurrentAttackBehaviour = null;
      foreach (AttackBehaviour behaviour in attackBehaviours)
      {
        if (behaviour.IsAvailable)
        {
          if (CurrentAttackBehaviour == null || CurrentAttackBehaviour.priority < behaviour.priority)
          {
            CurrentAttackBehaviour = behaviour;
          }
        }
      }
    }
  }
  
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
  }
}
