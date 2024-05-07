using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.AI;

public class Fishes : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 targetPosition;
    public float wanderRadius = 5f;
    public float wanderTimer = 2f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        timer = wanderTimer;
        SetRandomDestination();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // If it's time to pick a new destination, pick one
        if (timer <= 0f)
        {
            SetRandomDestination();
            timer = wanderTimer;
        }

        // Check if the fish has reached the current destination
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            SetRandomDestination();
        }
    }

    void SetRandomDestination()
    {
        // Generate a random point within the wander radius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            targetPosition = hit.position;

            // Set the random position as the destination
            agent.SetDestination(targetPosition);
        }
    }
}
