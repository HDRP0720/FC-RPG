using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRandomState : StateMachineBehaviour
{
  #region Variables
  public int numberOfStates = 3;
  public float minPlayTime = 0f;
  public float maxPlayTime = 5f;
  public float randomPlayTime;

  private readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");
  #endregion

  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    randomPlayTime = Random.Range(minPlayTime, maxPlayTime);
  }

  // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
  public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
    {
      animator.SetInteger(hashRandomIdle, -1);
    }

    if (stateInfo.normalizedTime > randomPlayTime && !animator.IsInTransition(0))
    {
      animator.SetInteger(hashRandomIdle, Random.Range(0, numberOfStates));
    }
  }

  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  //{
  //    
  //}

  // OnStateMove is called right after Animator.OnAnimatorMove()
  //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  //{
  //    // Implement code that processes and affects root motion
  //}

  // OnStateIK is called right after Animator.OnAnimatorIK()
  //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  //{
  //    // Implement code that sets up animation IK (inverse kinematics)
  //}
}
