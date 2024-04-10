using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcefulCameraLockOn : MonoBehaviour
{
    public Transform target; // The target object to lock onto
    public float maxLockStrength = 5f; // The maximum strength of the lock effect, adjust as needed

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = GetComponent<Transform>(); // Assuming this script is attached to the camera
    }

    void Update()
    {
        // Calculate the strength of the attraction based on how far the camera is pointing away from the target
        float lockStrength = CalculateLockStrength();

        // Determine the direction to the target
        Vector3 targetDirection = (target.position - cameraTransform.position).normalized;

        // Calculate the new rotation, applying the lock strength
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, lockStrength * Time.deltaTime);
    }

    float CalculateLockStrength()
    {
        // Get the current forward direction of the camera and the direction to the target
        Vector3 forward = cameraTransform.forward;
        Vector3 toTarget = (target.position - cameraTransform.position).normalized;

        // The dot product gives a value between -1 and 1, where 1 means the directions are the same
        float dotProduct = Vector3.Dot(forward, toTarget);

        // Convert the dot product to a value that inversely represents how aligned the directions are
        float alignmentValue = 1 - dotProduct;

        // Use this value to calculate the lock strength, ensuring it does not exceed the maximum
        float lockStrength = Mathf.Min(alignmentValue * maxLockStrength, maxLockStrength);

        return lockStrength;
    }
}
