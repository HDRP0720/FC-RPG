using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour, IAttackable, IDamageable
{
  #region Variables
  public float attackRange;
  public Transform hitTransform;
  public Transform projectileTransform;
  
  [Header("HP settings")]
  public int maxHealth = 100;
  public int currentHealth;
  
  [Header("Patrol settings")]
  public bool isPatrol = false;
  public Transform[] patrolPoints;
  [HideInInspector] public Transform targetPatrolPoint;
  
  private StateMachine<EnemyController> stateMachine;
  private Animator animator;
  private EnemyFOV fov;
  private int patrolpointIndex = 0;
  private IdleState idleState = new IdleState();
  [SerializeField] private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();
  
  private readonly int hitTriggerHash = Animator.StringToHash("HitTrigger");
  #endregion

  #region Properties
  public StateMachine<EnemyController> GetStateMachine => stateMachine;
  public Transform GetTarget => fov?.GetNearestTarget;
  public LayerMask GetTargetMask => fov.targetMask;
  #endregion

  private void Start()
  {
    stateMachine = new StateMachine<EnemyController>(this, new PatrolState());
    stateMachine.AddState(idleState);
    stateMachine.AddState(new MoveState());
    stateMachine.AddState(new AttackState());
    stateMachine.AddState(new DeadState());

    animator = GetComponent<Animator>();
    fov = GetComponent<EnemyFOV>();

    currentHealth = maxHealth;
    
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

  #region IAttackable interface
  public AttackBehaviour CurrentAttackBehaviour { get; private set; }
  public void OnExecuteAttack(int attackIndex)
  {
    if(CurrentAttackBehaviour != null && GetTarget != null)
      CurrentAttackBehaviour.ExcuteAttack(GetTarget.gameObject, projectileTransform);
  }
  #endregion

  #region IDamageable interface
  public bool IsAlive => currentHealth > 0;
  public void TakeDamage(int damage, GameObject hitEffectPrefab)
  {
    if (!IsAlive) return;

    currentHealth -= damage;

    if (hitEffectPrefab)
      Instantiate(hitEffectPrefab, hitTransform);
    
    if(IsAlive)
      animator?.SetTrigger(hitTriggerHash);
    else
      stateMachine.ChangeState<DeadState>();
  }
  #endregion
  
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
  }
}