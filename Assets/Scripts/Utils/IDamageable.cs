using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
  public bool IsAlive { get; }

  public void TakeDamage(int damage, GameObject hitEffectPrefab);
}
