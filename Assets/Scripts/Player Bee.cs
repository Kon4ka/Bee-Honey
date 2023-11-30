using UnityEngine;

public class BeeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;

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
        MoveBee();
        UpdateAnimations();
    }

    void MoveBee()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveInput.magnitude > 0.1f)
        {

            Quaternion rotation = Quaternion.LookRotation(moveInput);
            rotation.x = 0;

            rotation.z = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            rb.velocity = moveInput * moveSpeed;
        }
    }

            void UpdateAnimations()
    {
        animator.SetBool("IsMoving", rb.velocity.magnitude > 0.1f);
        animator.SetBool("IsRotating", rb.angularVelocity.magnitude > 0.1f);
        /*Debug.Log(animator.GetBool("IsMoving"));*/
    }
}
