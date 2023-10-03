using UnityEngine;

public class DeadState : State<EnemyController>
{
  private Animator animator;

  private readonly int isAliveHash = Animator.StringToHash("IsAlive");

  public override void OnInitialized()
  {
    animator = context.GetComponent<Animator>();
  }

  public override void OnEnter()
  {
    if(animator != null)
      animator.SetBool(isAliveHash, false);
  }

  public override void Update(float deltaTime)
  {
    if (stateMachine.GetElapsedTimeInState > 3.0f)
    {
      GameObject.Destroy(context.gameObject);
    }
  }
}
