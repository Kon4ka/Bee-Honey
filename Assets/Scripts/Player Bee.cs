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
    [SerializeField] private int pollenCount = 0; // ������� ������

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
        InvokeRepeating("UpdatePlayerLifetime", 1f, 1f); // ���������� ������� ����� ������ �������
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
            OnPlayerHealthZero.Invoke(GetPlayerLifetime()); // �������� ���� ������ ��� ������ �������
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("NidleBush"))
        {
            // ���������, ������ �� ���������� ������� � ������� ���������� �����
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                // ���������� ������������ � ���������, �������� ��� "NidleBush" �� ��������� �����
                if (hit.gameObject.CompareTag("NidleBush"))
                {
                    Physics.IgnoreCollision(controller, hit.collider, true);
                    Invoke("ResetCollision", 2f); // ��������� ������ �������� ����� 2 �������
                }

                // ������� ���� � ��������� ����� ���������� �����
                getDamage(1);
                lastDamageTime = Time.time;
            }
        }
    }

    void ResetCollision()
    {
        // ��������������� �������� � ���������, �������� ��� "NidleBush" ����� ���������� �������
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
                // ��������� ������
                CollectPollen();

                // ������� ������ ��� �����
                flower.HideFlower();
            }
        }
    }

    public void CollectPollen()
    {
        pollenCount++;
        ui.pollenCount++;

        Debug.Log("Pollen collected: " + pollenCount);

        // ����� �� ������ �������� �������������� �������� ��� ����� ������
    }

    void UpdatePlayerLifetime()
    {
        playerLifetime += 1f; // ����������� ����� ����� ������ �� 1 �������
    }
    public float GetPlayerLifetime()
    {
        Debug.Log("Time: " + playerLifetime.ToString() + " Pollens: " + pollenCount.ToString() + " = " + (playerLifetime - pollenCount * 5).ToString());
        return playerLifetime - pollenCount*5; //������ ������ ��������� ����� �� �����
    }

    public int GetPollenCount()
    {
        return pollenCount;
    }

    public void RestorePlayer()
    {
        // ������������ �� ������ ������
        TeleportToStart();

        // ��������� �������� pollenCount
        ResetPollenCount();

        // ��������� �������, ������������ �� �������
        ResetPlayerLifetime();

        // �������������� ������������� ��������
        RestoreMaxHealth();

        // ��������� lastDamageTime
        ResetLastDamageTime();

        // ��������� ����������� �����������
        CanMove = true;

        OnPollenCountReset.Invoke();
    }

    public void TeleportToStart()
    {
        // ����� �� ������ ����������� ������������ �� ������ ������.
        // ��������, ����������� ������ � ��������� �����.
        transform.position = new Vector3(1.56f, 1f, 0f); // ��� ������ ������, �������� �� ���� ������.
    }

    public void ResetPollenCount()
    {
        // ��������� �������� ������
        pollenCount = 0;
        ui.pollenCount = 0; 
    }

    public void ResetPlayerLifetime()
    {
        // ��������� �������, ������������ �� �������
        playerLifetime = 0f;
    }

    public void RestoreMaxHealth()
    {
        // �������������� ������������� ��������
        _health = startHealth;
        ui.SetHealth(_health); // ���� � ��� ���� ��������� ��������, �������� ��� �����.
    }

    public void ResetLastDamageTime()
    {
        // ��������� ������� ���������� ����������� �����
        lastDamageTime = 0f;
    }
}
