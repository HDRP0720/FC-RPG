using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StateMachine<T>
{
  private T context;

  private State<T> prevState;
  private State<T> currentState;
  private float elapsedTimeInState = 0.0f;

  private Dictionary<System.Type, State<T>> states = new Dictionary<Type, State<T>>();
  
  // Constructor
  public StateMachine(T context, State<T> initialState)
  {
    this.context = context;

    AddState(initialState);
    currentState = initialState;
    currentState.OnEnter();
  }
  
  // Getters
  public State<T> GetCurrentState => currentState;
  public State<T> GetPrevState => prevState;
  public float GetElapsedTimeInState => elapsedTimeInState;

  public void AddState(State<T> state)
  {
    state.SetStateMachineAndContext(this, context);
    states[state.GetType()] = state;
  }

  public void Update(float deltaTime)
  {
    elapsedTimeInState += deltaTime;
    
    currentState.Update(deltaTime);
  }
  
  public R ChangeState<R>() where R : State<T>
  {
    var newType = typeof(R);
    if (currentState.GetType() == newType) return currentState as R;
    
    currentState?.OnExit();
    
    prevState = currentState;
    currentState = states[newType];
    currentState.OnEnter();
    elapsedTimeInState = 0.0f;

    return currentState as R;
  }
}
