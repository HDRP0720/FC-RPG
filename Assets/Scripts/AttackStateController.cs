using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateController : MonoBehaviour
{
  public delegate void OnEnterAttackState();
  public delegate void OnExitAttackState();

  public OnEnterAttackState enterAttackStateHandler;
  public OnExitAttackState exitAttackStateHandler;
  
  public bool IsInAttackState { get; private set; }

  private void Start()
  {
    enterAttackStateHandler = new OnEnterAttackState(EnterAttackState);
    exitAttackStateHandler = new OnExitAttackState(ExitAttackState);
  }

  public void OnStartOfAttackState()
  {
    IsInAttackState = true;
    enterAttackStateHandler();
  }
  public void OnEndOfAttackState()
  {
    IsInAttackState = false;
    exitAttackStateHandler();
  }

  public void OnCheckAttackCollider(int attackIndex)
  {
    GetComponent<IAttackable>()?.OnExecuteAttack(attackIndex);
  }
  
  private void EnterAttackState()
  {
    
  }
  private void ExitAttackState()
  {
    
  }
}
