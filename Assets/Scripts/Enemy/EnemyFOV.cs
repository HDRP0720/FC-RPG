using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
  [Range(0, 360)]
  public float viewAngle = 90f;
  public float viewRadius = 5f;
  public LayerMask targetMask;
  public LayerMask obstacleMask;
  public float delaytime = 0.2f;

  private List<Transform> visibleTargets = new List<Transform>();
  private Transform nearestTarget;
  private float distanceToTaraget = 0.0f;
  
  // getters
  public List<Transform> GetVisibleTargets => visibleTargets;
  public Transform GetNearestTarget => nearestTarget;

  private void Start()
  {
    StartCoroutine(FindTargetsWithDelay(delaytime));
  }
  
  private IEnumerator FindTargetsWithDelay(float seconds)
  {
    while (true)
    {
      yield return new WaitForSeconds(seconds);
      FindVisibleTargets();
    }
  }
  private void FindVisibleTargets()
  {
    distanceToTaraget = 0.0f;
    nearestTarget = null;
    visibleTargets.Clear();

    Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
    for (int i = 0; i < targetsInViewRadius.Length; i++)
    {
      Transform target = targetsInViewRadius[i].transform;
      Vector3 dirToTarget = (target.position - transform.position).normalized;
      if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
      {
        float dstToTarget = Vector3.Distance(transform.position, target.position);
        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
        {
          visibleTargets.Add(target);
          if (nearestTarget == null || distanceToTaraget > dstToTarget)
          {
            nearestTarget = target;
            distanceToTaraget = dstToTarget;
          }
        }
      }
    }
  }

  public Vector3 CalcDirFromAngle(float angleInDeg, bool isGlobalAngle)
  {
    if (!isGlobalAngle)
      angleInDeg += transform.eulerAngles.y;

    float x = Mathf.Sin(angleInDeg * Mathf.Deg2Rad);
    float z = Mathf.Cos(angleInDeg * Mathf.Deg2Rad);

    return new Vector3(x, 0, z);
  }
}
