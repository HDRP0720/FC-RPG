using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour
{
  #region Variables
  public float speed = 5f;
  public float jumpHeight = 2f;
  public float dashDistance = 5f;
  public LayerMask groundLayerMask;
  public float groundCheckDistance = 0.3f;

  private Rigidbody rb;
  private Vector3 inputDirection = Vector3.zero;
  private bool bIsGrounded;
  #endregion

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
  }
  private void Update()
  {
    CheckGround();
      
    // Implement character movement by user input
    inputDirection = Vector3.zero;
    inputDirection.x = Input.GetAxis("Horizontal");
    inputDirection.z = Input.GetAxis("Vertical");
    if (inputDirection != Vector3.zero)
      transform.forward = inputDirection;
    
    // Implement character jump by user input
    if (Input.GetButtonDown("Jump") && bIsGrounded)
    {
      Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
      rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
    }
    
    // Implement character dash by user input
    if (Input.GetButtonDown("Dash"))
    {
      Vector3 dashVelocity = Vector3.Scale(transform.forward, 
        dashDistance * new Vector3((Mathf.Log(Time.deltaTime * rb.drag + 1) / Time.deltaTime),
        0,
        (Mathf.Log(Time.deltaTime * rb.drag + 1) / Time.deltaTime)));
      rb.AddForce(dashVelocity, ForceMode.VelocityChange);
    }
  }
  private void FixedUpdate()
  {
    rb.MovePosition(rb.position + inputDirection * (speed * Time.fixedDeltaTime));
  }

  private void CheckGround()
  {
    Vector3 rayOrigin = transform.position + (Vector3.up * 0.1f);
    if (Physics.Raycast(rayOrigin,Vector3.down, out _, groundCheckDistance, groundLayerMask))
    {
      bIsGrounded = true;
    }
    else
    {
      bIsGrounded = false;
    }
    #if UNITY_EDITOR
    Debug.DrawLine(rayOrigin, rayOrigin + Vector3.down * groundCheckDistance, Color.red);
    #endif
  }
}
