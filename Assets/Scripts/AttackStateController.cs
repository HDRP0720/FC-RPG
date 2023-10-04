using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateController : MonoBehaviour
{
  public delegate void OnEnterAttackState();
  public delegate void OnExitAttackState();

  public OnEnterAttackState enterAttackHandler;
  public OnExitAttackState exitAttackHandler;
  
  public bool IsInAttackState { get; private set; }

  private void Start()
  {
    enterAttackHandler = new OnEnterAttackState(EnterAttackState);
    exitAttackHandler = new OnExitAttackState(ExitAttackState);
  }

  public void OnStartOfAttackState()
  {
    IsInAttackState = true;
    enterAttackHandler();
  }
  public void OnEndOfAttackState()
  {
    IsInAttackState = false;
    exitAttackHandler();
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
