using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public float startYLocalPosition1;
    public float targetYLocalPosition1;
    public float startYLocalPosition2;
    public float targetYLocalPosition2;
    public float duration = 3.0f; // Duration of movement in seconds

    private float elapsedTime = 0.0f; // Time elapsed since the movement started

    void Start()
    {
        startYLocalPosition1 = object1.transform.localPosition.y;
        startYLocalPosition2 = object2.transform.localPosition.y;
    }

    void Update()
    {
        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the percentage of time elapsed
        float t = Mathf.Clamp01(elapsedTime / duration);

        // Move object1 towards its target local position
        float newYLocalPosition1 = Mathf.Lerp(startYLocalPosition1, targetYLocalPosition1, t);
        object1.transform.localPosition = new Vector3(object1.transform.localPosition.x, newYLocalPosition1, object1.transform.localPosition.z);

        // Move object2 towards its target local position
        float newYLocalPosition2 = Mathf.Lerp(startYLocalPosition2, targetYLocalPosition2, t);
        object2.transform.localPosition = new Vector3(object2.transform.localPosition.x, newYLocalPosition2, object2.transform.localPosition.z);

        // Check if the movement duration has been exceeded
        if (elapsedTime >= duration)
        {
            // Reset elapsed time and stop the movement
            elapsedTime = 0.0f;
            enabled = false; // Disable this script to stop further updates
        }
    }
}