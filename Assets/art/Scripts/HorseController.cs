using UnityEngine;

public class HorseController : MonoBehaviour
{
    [Header("Movement")]
    public float currentSpeed = 0f;

    public float maxForwardSpeed = 20f;

    // Назад отключен
    public float maxBackwardSpeed = 0f;

    public float accelerationAmount = 3f;

    public float brakeAmount = 15f;

    [Header("Turning")]
    public float turnSpeed = 60f;
    public float turnDistance = -0.022f;

    [Header("Brake Distances")]
    public float brakeDistance = -0.03f;
    public float hardStopDistance = -0.2f;

    [Header("Hands")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Reins Center")]
    public Transform reinsCenter;

    [Header("Animation")]
    public Animator animator;

    [Header("Acceleration")]
    public float whipSensitivity = 0.15f;

    [Header("Whip Cooldown")]
    public float whipCooldownTime = 1f;

    [Header("Debug")]
    public float leftHandDistance;
    public float rightHandDistance;

    public float leftHandYSpeed;
    public float rightHandYSpeed;

    public float brakeStrength;
    public float averagePull;

    private Rigidbody rb;

    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;

    private float whipCooldown = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;

        rb.drag = 0f;
        rb.angularDrag = 999f;

        // Не переворачивается
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        lastLeftPos = leftHand.position;
        lastRightPos = rightHand.position;
    }

    void Update()
    {
        HandleTurning();

        HandleSpeed();

        UpdateAnimation();

        lastLeftPos = leftHand.position;
        lastRightPos = rightHand.position;
    }

    void FixedUpdate()
    {
        MoveHorse();
    }

    void HandleTurning()
    {
        Vector3 leftLocal =
            reinsCenter.InverseTransformPoint(
                leftHand.position
            );

        Vector3 rightLocal =
            reinsCenter.InverseTransformPoint(
                rightHand.position
            );

        leftHandDistance = leftLocal.z;
        rightHandDistance = rightLocal.z;

        // Насколько сильно тянут
        float rightPull =
            Mathf.Abs(
                Mathf.Min(
                    rightHandDistance - turnDistance,
                    0f
                )
            );

        float leftPull =
            Mathf.Abs(
                Mathf.Min(
                    leftHandDistance - turnDistance,
                    0f
                )
            );

        // Поворот вправо
        if (rightHandDistance < turnDistance)
        {
            float turnAmount =
                rightPull * turnSpeed;

            transform.Rotate(
                0,
                turnAmount * Time.deltaTime,
                0
            );
        }

        // Поворот влево
        if (leftHandDistance < turnDistance)
        {
            float turnAmount =
                leftPull * turnSpeed;

            transform.Rotate(
                0,
                -turnAmount * Time.deltaTime,
                0
            );
        }
    }

    void HandleSpeed()
    {
        whipCooldown -= Time.deltaTime;

        Vector3 leftLocal =
            reinsCenter.InverseTransformPoint(
                leftHand.position
            );

        Vector3 rightLocal =
            reinsCenter.InverseTransformPoint(
                rightHand.position
            );

        averagePull =
            (leftLocal.z + rightLocal.z) * 0.5f;

        brakeStrength = Mathf.Abs(averagePull);

        // ТОРМОЖЕНИЕ ТОЛЬКО ДВУМЯ РУКАМИ
        bool bothHandsPulling =
            leftLocal.z < brakeDistance &&
            rightLocal.z < brakeDistance;

        if (bothHandsPulling)
        {
            float stopFactor =
                Mathf.InverseLerp(
                    brakeDistance,
                    hardStopDistance,
                    averagePull
                );

            float finalBrake =
                Mathf.Lerp(
                    brakeAmount,
                    brakeAmount * 6f,
                    stopFactor
                );

            currentSpeed -=
                finalBrake * Time.deltaTime;

            // Полный стоп
            if (averagePull <= hardStopDistance)
            {
                currentSpeed = 0f;
            }
        }

        // Защита от ухода в минус
        if (currentSpeed < 0f)
        {
            currentSpeed = 0f;
        }

        // Движение рук по Y
        float leftY =
            leftHand.position.y - lastLeftPos.y;

        float rightY =
            rightHand.position.y - lastRightPos.y;

        leftHandYSpeed = leftY;
        rightHandYSpeed = rightY;

        // Ускорение
        bool whip =
            leftY < -whipSensitivity &&
            rightY < -whipSensitivity;

        if (whip && whipCooldown <= 0f)
        {
            currentSpeed += accelerationAmount;

            whipCooldown = whipCooldownTime;
        }

        currentSpeed =
            Mathf.Clamp(
                currentSpeed,
                maxBackwardSpeed,
                maxForwardSpeed
            );
    }

    void MoveHorse()
    {
        // Полный стоп
        if (Mathf.Abs(currentSpeed) < 0.01f)
        {
            currentSpeed = 0f;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            return;
        }

        Vector3 move =
            transform.forward *
            currentSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position + move
        );
    }

    void UpdateAnimation()
    {
        if (animator == null)
            return;

        // Передаем настоящий currentSpeed
        animator.SetFloat(
            "Speed",
            currentSpeed
        );
    }

    // Столкновение = стоп только при сильном ударе
    private void OnCollisionEnter(Collision collision)
    {
        // Слабые касания игнорируем
        if (collision.relativeVelocity.magnitude < 5f)
            return;

        currentSpeed = 0f;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}