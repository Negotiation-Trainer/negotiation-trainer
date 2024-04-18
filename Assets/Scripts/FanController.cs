using UnityEngine;

public class FanController : MonoBehaviour
{
    public bool isActive = false;
    public float inactiveSpeed = 0f; // Speed when inactive
    public float activeSpeed = 500f; // Speed when active
    public float maxSpeed = 1000f; // Maximum speed allowed in editor

    private float currentSpeed; // Current speed of rotation

    void Update()
    {
        // Calculate the target speed based on the isActive boolean
        float targetSpeed = isActive ? activeSpeed : inactiveSpeed;
        
        // Clamp the target speed to the maximum speed
        targetSpeed = Mathf.Clamp(targetSpeed, 0f, maxSpeed);

        // Lerping the current speed towards the target speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 5f);

        // Rotate the fan on the Z-axis
        transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
}
