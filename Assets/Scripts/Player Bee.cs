using UnityEngine;
using UnityEngine.WSA;

public class BeeController : MonoBehaviour
{
    // movement control panels

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    [SerializeField] private Transform cam;
    [SerializeField] private CharacterController controller;
    [SerializeField] private int startHealth = 3;
    [SerializeField] private PlayerUI ui;
    [SerializeField] private int pollenCount = 0; // Счетчик пыльцы

    private Rigidbody rb;
    private Animator animator;
    private int _health;
    private float lastDamageTime;
    private float damageCooldown = 3f;

    void Start()
    {
        /*animator.SetBool("IsMoving", rb.velocity.magnitude > 0.1f);*/
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _health = startHealth;
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

    void getDamage(int damage)
    {
        _health -= damage;
        ui.SetHealth(_health);

        if (_health <= 0 )
        {
            Debug.Log("Game over");
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("NidleBush"))
        {
            // Проверяем, прошло ли достаточно времени с момента последнего удара
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                // Игнорируем столкновение с объектами, имеющими тег "NidleBush" на некоторое время
                if (hit.gameObject.CompareTag("NidleBush"))
                {
                    Physics.IgnoreCollision(controller, hit.collider, true);
                    Invoke("ResetCollision", 2f); // Инвокация сброса коллизии через 2 секунды
                }

                // Наносим урон и обновляем время последнего удара
                getDamage(1);
                lastDamageTime = Time.time;
            }
        }
    }

    void ResetCollision()
    {
        // Восстанавливаем коллизии с объектами, имеющими тег "NidleBush" после прошествия времени
        Collider[] nidleBushColliders = GameObject.FindGameObjectsWithTag("NidleBush")[0].GetComponents<Collider>();
        foreach (Collider collider in nidleBushColliders)
        {
            Physics.IgnoreCollision(controller, collider, false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flower"))
        {
            Flower flower = other.GetComponent<Flower>();

            if (flower != null && !flower.IsPicked)
            {
                // Подбираем пыльцу
                CollectPollen();

                // Убираем цветок под землю
                flower.HideFlower();
            }
        }
    }

    void CollectPollen()
    {
        pollenCount++;
        ui.pollenCount++;

        Debug.Log("Pollen collected: " + pollenCount);

        // Здесь вы можете добавить дополнительные действия при сборе пыльцы
    }
}
