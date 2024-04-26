using UnityEngine;

public class DarkenLighting : MonoBehaviour
{
    // Boolean flags to control whether to darken or lighten the light
    public bool darkenLight = false;
    public bool lightenLight = false;

    // Duration of the intensity change
    public float changeDuration = 1f;

    // Starting intensity of the directional light
    private float startIntensity;

    void Start()
    {
        // Get the reference to the directional light attached to the same game object
        Light directionalLight = GetComponent<Light>();

        if (directionalLight == null)
        {
            Debug.LogWarning("No directional light component found attached to this game object. Darkening effect may not work properly.");
            return;
        }

        // Store the starting intensity of the directional light
        startIntensity = directionalLight.intensity;

        // Check if either darkenLight or lightenLight flag is true
        if (darkenLight)
        {
            // Smoothly lerp the intensity of the directional light to 0
            StartCoroutine(LerpIntensity(directionalLight, startIntensity, 0f));
        }
        else if (lightenLight)
        {
            // Smoothly lerp the intensity of the directional light to 1
            StartCoroutine(LerpIntensity(directionalLight, startIntensity, 1f));
        }
    }

    // Coroutine to smoothly lerp the intensity of the directional light
    private System.Collections.IEnumerator LerpIntensity(Light directionalLight, float startIntensity, float targetIntensity)
    {
        float elapsedTime = 0f;
        while (elapsedTime < changeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / changeDuration);
            directionalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }
    }
}
