using UnityEngine;

public class SmoothedSupernaturalCameraControl : MonoBehaviour
{
    public Transform target;
    public float mouseSensitivity = 100f;
    public float supernaturalStrength = 2f;
    public float intervalDuration = 5f;
    public float pullDuration = 0.5f;
    public float staminaRecoveryRate = 0.1f;
    public float staminaDepletionRate = 0.2f;

    private float xRotation = 0f;
    private float timeSinceLastPull = 0f;
    private float pullStartTime = -1f;
    private float supernaturalStamina = 1f;
    private Vector2 lastMouseMovement = Vector2.zero;
    private bool isPulling = false;
    private float pullStrength = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseMovement = HandleMouseInput();

        if (isPulling)
        {
            float resistance = (mouseMovement - lastMouseMovement).magnitude;
            supernaturalStamina = Mathf.Max(0f, supernaturalStamina - resistance * staminaDepletionRate * Time.deltaTime);
            pullStrength = Mathf.Min(pullStrength + (Time.deltaTime / pullDuration), 1f); // Smoothly increase pull strength
        }
        else
        {
            supernaturalStamina = Mathf.Min(1f, supernaturalStamina + staminaRecoveryRate * Time.deltaTime);
            timeSinceLastPull += Time.deltaTime;
            pullStrength = 0f; // Reset pull strength
        }

        if (timeSinceLastPull >= intervalDuration && !isPulling)
        {
            isPulling = true;
            pullStartTime = Time.time;
        }

        if (isPulling)
        {
            ApplySupernaturalForce();

            if (Time.time - pullStartTime > pullDuration)
            {
                isPulling = false;
                timeSinceLastPull = 0f;
                pullStartTime = -1f;
            }
        }

        lastMouseMovement = mouseMovement;
    }

    Vector2 HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y + mouseX, 0f);

        return new Vector2(mouseX, mouseY);
    }

    void ApplySupernaturalForce()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        float adjustedStrength = supernaturalStrength * supernaturalStamina * pullStrength; // Now includes pullStrength for gradual increase
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, adjustedStrength * Time.deltaTime);
    }
}
