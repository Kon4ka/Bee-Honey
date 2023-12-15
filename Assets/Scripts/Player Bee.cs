using System;
using UnityEngine;
using UnityEngine.Events;

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
    private float playerLifetime = 0f;
    private bool _canMove = true;

    public class HealthZeroEvent : UnityEvent<float> { }
    public HealthZeroEvent OnPlayerHealthZero = new HealthZeroEvent();
    public UnityEvent OnPollenCountReset;
    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    void Start()
    {
        /*animator.SetBool("IsMoving", rb.velocity.magnitude > 0.1f);*/
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _health = startHealth;
        InvokeRepeating("UpdatePlayerLifetime", 1f, 1f); // Обновление времени жизни каждую секунду
        if (OnPollenCountReset == null)
            OnPollenCountReset = new UnityEvent();
    }

    void FixedUpdate()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        MoveBee(moveInput);
        UpdateAnimations(moveInput);
    }

    void MoveBee(Vector3 moveInput)
    {
        if (CanMove)
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

        if (_health <= 0)
        {
            Debug.Log("Game over");
            OnPlayerHealthZero.Invoke(GetPlayerLifetime()); // Передаем счет игрока при вызове события
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

    public void CollectPollen()
    {
        pollenCount++;
        ui.pollenCount++;

        Debug.Log("Pollen collected: " + pollenCount);

        // Здесь вы можете добавить дополнительные действия при сборе пыльцы
    }

    void UpdatePlayerLifetime()
    {
        playerLifetime += 1f; // Увеличиваем время жизни игрока на 1 секунду
    }
    public float GetPlayerLifetime()
    {
        Debug.Log("Time: " + playerLifetime.ToString() + " Pollens: " + pollenCount.ToString() + " = " + (playerLifetime - pollenCount * 5).ToString());
        return playerLifetime - pollenCount*5; //лишняя пыльца сокращает время на раунд
    }

    public int GetPollenCount()
    {
        return pollenCount;
    }

    public void RestorePlayer()
    {
        // Телепортация на начало уровня
        TeleportToStart();

        // Обнуление счетчика pollenCount
        ResetPollenCount();

        // Обнуление времени, потраченного на уровень
        ResetPlayerLifetime();

        // Восстановление максимального здоровья
        RestoreMaxHealth();

        // Обнуление lastDamageTime
        ResetLastDamageTime();

        // Включение возможности перемещения
        CanMove = true;

        OnPollenCountReset.Invoke();
    }

    public void TeleportToStart()
    {
        // Здесь вы должны реализовать телепортацию на начало уровня.
        // Например, перемещение игрока к стартовой точке.
        transform.position = new Vector3(1.56f, 1f, 0f); // Это просто пример, замените на свою логику.
    }

    public void ResetPollenCount()
    {
        // Обнуление счетчика пыльцы
        pollenCount = 0;
        ui.pollenCount = 0; 
    }

    public void ResetPlayerLifetime()
    {
        // Обнуление времени, потраченного на уровень
        playerLifetime = 0f;
    }

    public void RestoreMaxHealth()
    {
        // Восстановление максимального здоровья
        _health = startHealth;
        ui.SetHealth(_health); // Если у вас есть интерфейс здоровья, обновите его также.
    }

    public void ResetLastDamageTime()
    {
        // Обнуление времени последнего полученного урона
        lastDamageTime = 0f;
    }
}
