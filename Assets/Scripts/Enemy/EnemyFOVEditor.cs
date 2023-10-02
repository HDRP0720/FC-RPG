using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class EnemyFOVEditor : Editor
{
  private void OnSceneGUI()
  {
    EnemyFOV fov = (EnemyFOV)target;
    
    Handles.color = Color.yellow;
    Vector3 viewAngleA = fov.CalcDirFromAngle(-fov.viewAngle / 2, false);
    Vector3 viewAngleB = fov.CalcDirFromAngle(fov.viewAngle / 2, false);
    
    // Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
    Handles.DrawWireArc(fov.transform.position, Vector3.up, viewAngleA, fov.viewAngle, fov.viewRadius);
    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

    Handles.color = Color.red;
    foreach (Transform visibleTarget in fov.GetVisibleTargets)
    {
      Handles.DrawLine(fov.transform.position, visibleTarget.position);
    }
  }
}
