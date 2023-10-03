using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
  #if UNITY_EDITOR
  [Multiline] [Tooltip("This is only for additional explanation in editor")] 
  public string devDescription;
  #endif

  public int animaionIndex;
  public int priority;
  public int damage = 10;
  public float range = 3f;
  public GameObject effectPrefab;
  public LayerMask targetMask;

  [SerializeField] protected float coolTime;
  protected float calcCoolTime = 0.0f;
  
  // getter
  public bool IsAvailable => calcCoolTime >= coolTime;

  private void Start()
  {
    calcCoolTime = coolTime;
  }
  private void Update()
  {
    if (calcCoolTime < coolTime)
    {
      calcCoolTime += Time.deltaTime;
    }
  }
  
  // startPoint is for projectile position
  public abstract void ExcuteAttack(GameObject target = null, Transform startPoint = null);
}
