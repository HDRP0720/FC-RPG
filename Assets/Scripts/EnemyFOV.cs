using System;
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

  private List<Transform> visibleTargets = new List<Transform>();
  private Transform nearestTarget;
  private float distanceToTaraget = 0.0f;

  private void Update()
  {
    FindVisibleTargets();
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
}
