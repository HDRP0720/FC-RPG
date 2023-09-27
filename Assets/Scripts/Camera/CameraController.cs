using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  #region Variables
  public float height = 5f;
  public float distance = 10f;
  public float angle = 45f;
  public float lookAtHeight = 2f;
  public float smoothSpeed = 0.5f;

  public Transform target;

  private Vector3 refVelocity;
  #endregion

  private void LateUpdate()
  {
    HandleCamera();
  }

  public void HandleCamera()
  {
    if (!target) return;

    Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);

    Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;

    Vector3 finalTargetPosition = target.position;
    finalTargetPosition.y += lookAtHeight;

    Vector3 finalPosition = finalTargetPosition + rotatedVector;
    
    transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
    
    transform.LookAt(target.position);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
    if (target)
    {
      Vector3 lookAtPosition = target.position;
      lookAtPosition.y += lookAtHeight;
      Gizmos.DrawLine(transform.position, lookAtPosition);
      Gizmos.DrawSphere(lookAtPosition, 0.25f);
    }
    
    Gizmos.DrawSphere(transform.position, 0.25f);
  }
}
