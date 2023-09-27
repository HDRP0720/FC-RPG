using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class NavAgentCharacter : MonoBehaviour
{
  #region Variables
  public Transform clickMark;
  public LayerMask groundLayerMask;
  
  private CharacterController cc;
  private NavMeshAgent agent;
  private Camera mainCamera;
  private LineRenderer lr;
  private Coroutine draw;
  #endregion

  private void Start()
  {
    cc = GetComponent<CharacterController>();
    agent = GetComponent<NavMeshAgent>();
    agent.updatePosition = false;
    agent.updateRotation = true;
    mainCamera = Camera.main;
    
    lr = GetComponent<LineRenderer>();
    lr.startWidth = 0.1f;
    lr.endWidth = 0.1f;
    lr.material.color = Color.green;
    lr.enabled = false;
  }
  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayerMask))
      {
        Debug.Log($"We hit {hit.collider.name} {hit.point}");
     
        agent.SetDestination(hit.point);
        
        clickMark.position = hit.point + new Vector3(0, 0.01f, 0);
        clickMark.gameObject.SetActive(true);
        
        if(draw != null) StopCoroutine(draw);
        draw = StartCoroutine(DrawPath());
      }
    }
    
    if (agent.remainingDistance > agent.stoppingDistance)
    {
      cc.Move(agent.velocity * Time.deltaTime);
    }
    else
    {
      cc.Move(Vector3.zero);
      clickMark.gameObject.SetActive(false);
      lr.enabled = false;
      if(draw != null) StopCoroutine(draw);
    }
  }
  private void LateUpdate()
  {
    transform.position = agent.nextPosition;
  }

  private IEnumerator DrawPath()
  {
    lr.enabled = true;
    yield return null;
    while (true)
    {
      int count = agent.path.corners.Length;
      lr.positionCount = count;
      for (int i = 0; i < count; i++)
      {
        lr.SetPosition(i, agent.path.corners[i]);
      }
      yield return null;
    }
  }
}