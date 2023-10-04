using UnityEngine;

public class MeleeAttackBehaviour : AttackBehaviour
{
  public ManualCollision attackCollision;
  
  public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
  {
    Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);
    foreach (Collider col in colliders)
    {
      col.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);
    }
  }
}
