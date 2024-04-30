using UnityEngine;
using Cinemachine;

public class DollyCartMover : MonoBehaviour
{
    public CinemachineDollyCart dollyCart;
    public float speed = 1f;
    public float trackLength = 100f; // Length of the dolly track

    private float distanceTraveled = 0f;

    void Update()
    {
        // Move the dolly cart along the track
        dollyCart.m_Position += speed * Time.deltaTime;

        // Check if the dolly cart has reached the end of the track
        if (dollyCart.m_Position >= trackLength)
        {
            // Reset the position to the beginning
            dollyCart.m_Position = 0f;
        }
    }
}
