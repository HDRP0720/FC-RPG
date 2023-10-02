using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public float attackRange;
  
  private StateMachine<EnemyController> stateMachine;
  private EnemyFOV fov;

  // getter
  public StateMachine<EnemyController> GetStateMachine => stateMachine;
  public Transform GetTarget => fov.GetNearestTarget;

  private void Start()
  {
    stateMachine = new StateMachine<EnemyController>(this, new IdleState());
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

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
  }
}
