using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birds : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private float circleRadius;
    public float minCircleRadius = 3f;
    public float maxCircleRadius = 8f;
    public float circleSpeed = 2f;
    private float angle = 0f;
    private float changeRadiusInterval = 5f; // Change radius every 5 seconds
    private float nextRadiusChangeTime;
    private bool isAgentOnNavMesh = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.autoBraking = false; // Disable auto braking so the bird doesn't stop at waypoints
        isAgentOnNavMesh = agent.isOnNavMesh; // Check if agent is initially on the NavMesh
        SetRandomCircleDestination();
        nextRadiusChangeTime = Time.time + changeRadiusInterval;
    }

    void Update()
    {
        // Check if the agent is on the NavMesh
        if (!isAgentOnNavMesh)
        {
            isAgentOnNavMesh = agent.isOnNavMesh;
            return;
        }

        // Check if it's time to change the radius
        if (Time.time >= nextRadiusChangeTime)
        {
            SetRandomCircleDestination();
            nextRadiusChangeTime = Time.time + changeRadiusInterval;
        }

        // Calculate next position on the circle path
        Vector3 nextPosition = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * circleRadius;

        // Set the next position as the destination
        agent.SetDestination(nextPosition);

        // Rotate the bird smoothly around the circle
        angle += circleSpeed * Time.deltaTime;

        // Wrap angle between 0 and 360 degrees
        angle %= 360f;
    }

    void SetRandomCircleDestination()
    {
        // Randomize circle radius
        circleRadius = Random.Range(minCircleRadius, maxCircleRadius);

        // Set the initial angle randomly
        angle = Random.Range(0f, 360f);

        // Set the initial position on the circle path
        Vector3 initialPosition = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * circleRadius;

        // Check if the agent is on the NavMesh before setting the destination
        if (isAgentOnNavMesh)
        {
            agent.SetDestination(initialPosition);
        }
    }
}