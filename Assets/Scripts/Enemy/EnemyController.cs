using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public float attackRange;
  
  [Header("Patrol settings")]
  public bool isPatrol = false;
  public Transform[] patrolPoints;
  [HideInInspector] public Transform targetPatrolPoint;
  
  private StateMachine<EnemyController> stateMachine;
  private EnemyFOV fov;
  private int patrolpointIndex = 0;

  // getter
  public StateMachine<EnemyController> GetStateMachine => stateMachine;
  public Transform GetTarget => fov?.GetNearestTarget;

  private void Start()
  {
    stateMachine = new StateMachine<EnemyController>(this, new PatrolState());
    IdleState idleState = new IdleState();
    idleState.isPatrol = isPatrol;
    
    stateMachine.AddState(idleState);
    stateMachine.AddState(new MoveState());
    stateMachine.AddState(new AttackState());

    fov = GetComponent<EnemyFOV>();
  }
  private void Update()
  {
    stateMachine.Update(Time.deltaTime);

    Debug.Log(stateMachine.GetCurrentState);
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

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
  }
}
