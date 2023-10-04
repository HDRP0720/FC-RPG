using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllerCharacter : MonoBehaviour
{
  #region Variables
  public float speed = 5f;
  public float jumpHeight = 2f;
  public float dashDistance = 5f;
  public float gravity = -9.81f;
  public Vector3 drag;

  private CharacterController cc;
  private Vector3 calcVelocity;
  private bool bIsGrounded;
  #endregion

  private void Start()
  {
    cc = GetComponent<CharacterController>();
  }
  private void Update()
  {
    bIsGrounded = cc.isGrounded;
    if (bIsGrounded && calcVelocity.y < 0)
      calcVelocity.y = 0;
    
    // Implement character movement by user input
    Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    cc.Move(move * (Time.deltaTime * speed));
    if (move != Vector3.zero)
      transform.forward = move;
    
    // Implement character jump by user input
    if (Input.GetButtonDown("Jump") && bIsGrounded)
      calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
    
    // Implement character dash by user input
    if (Input.GetButtonDown("Dash"))
    {
      calcVelocity += Vector3.Scale(transform.forward,
        dashDistance * new Vector3((Mathf.Log(Time.deltaTime * drag.x + 1) / Time.deltaTime),
          0, (Mathf.Log(Time.deltaTime * drag.z + 1) / Time.deltaTime))
      );
    }
    
    // Calc gravity
    calcVelocity.y += gravity * Time.deltaTime;
    
    // Calc dash drag
    calcVelocity.x /= 1 + drag.x * Time.deltaTime;
    calcVelocity.y /= 1 + drag.y * Time.deltaTime;
    calcVelocity.z /= 1 + drag.z * Time.deltaTime;

    cc.Move(calcVelocity * Time.deltaTime);
  }
}
