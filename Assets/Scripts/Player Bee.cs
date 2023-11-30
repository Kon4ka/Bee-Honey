using UnityEngine;

public class BeeController : MonoBehaviour
{
    // movement control panels

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    [SerializeField] private Transform cam;
    [SerializeField] private CharacterController controller;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        /*animator.SetBool("IsMoving", rb.velocity.magnitude > 0.1f);*/
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        MoveBee(moveInput);
        UpdateAnimations(moveInput);
    }

    void MoveBee(Vector3 moveInput)
    {
        if (moveInput.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }


    void UpdateAnimations(Vector3 moveInput)
    {
        animator.SetBool("IsMoving", moveInput.magnitude > 0.1f);
        animator.SetBool("IsRotating", moveInput.magnitude > 0.1f);
        /*Debug.Log(animator.GetBool("IsMoving"));*/
    }
}
