using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [Header("Target settings")]
  public Transform target;
  public LayerMask targetMask;
  
  [Header("Range settings")]
  public float viewRadius;
  public float attackRange;
  
  private StateMachine<EnemyController> stateMachine;

  // getter
  public StateMachine<EnemyController> GetStateMachine => stateMachine;

  private void Start()
  {
    stateMachine = new StateMachine<EnemyController>(this, new IdleState());
    stateMachine.AddState(new MoveState());
    stateMachine.AddState(new AttackState());
  }
  private void Update()
  {
    stateMachine.Update(Time.deltaTime);

    Debug.Log(stateMachine.GetCurrentState);
  }

  public Transform SearchEnemy()
  {
    target = null;
    Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
    if (targetsInViewRadius.Length > 0)
      target = targetsInViewRadius[0].transform;

    return target;
  }

  public bool IsAvailableAttack()
  {
    if (!target) return false;

    float distance = Vector3.Distance(transform.position, target.position);
    return distance <= attackRange;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, viewRadius);
    
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
  }
}
