using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  protected StateMachine<EnemyController> stateMachine;

  private void Start()
  {
    stateMachine = new StateMachine<EnemyController>(this, new IdleState());
    stateMachine.AddState(new MoveState());
    stateMachine.AddState(new AttackState());
  }

  private void Update()
  {
    stateMachine.Update(Time.deltaTime);
  }
}
