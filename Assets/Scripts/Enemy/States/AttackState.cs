using UnityEngine;

public class AttackState : State<EnemyController>
{
  private Animator animator;
  private AttackStateController attackStateController;
  private IAttackable attackable;
  
  private readonly int attackTriggerHash = Animator.StringToHash("AttackTrigger");
  private readonly int attackIndexHash = Animator.StringToHash("AttackIndex");

  public override void OnInitialized()
  {
    animator = context.GetComponent<Animator>();
    attackStateController = context.GetComponent<AttackStateController>();
    attackable = context.GetComponent<IAttackable>();
  }
  
  public override void OnEnter()
  {
    if (attackable == null || attackable.CurrentAttackBehaviour == null)
    {
      stateMachine.ChangeState<IdleState>();
      return;
    }

    attackStateController.enterAttackHandler += OnEnterAttackState;
    attackStateController.exitAttackHandler += OnExitAttackState;
    
    animator.SetInteger(attackIndexHash, attackable.CurrentAttackBehaviour.animaionIndex);
    animator.SetTrigger(attackTriggerHash);
  }

  public override void Update(float deltaTime)
  {
    
  }

  public override void OnExit()
  {
    attackStateController.enterAttackHandler -= OnEnterAttackState;
    attackStateController.exitAttackHandler -= OnExitAttackState;
  }

  private void OnEnterAttackState()
  {
    
  }
  private void OnExitAttackState()
  {
    stateMachine.ChangeState<IdleState>();
  }
}
