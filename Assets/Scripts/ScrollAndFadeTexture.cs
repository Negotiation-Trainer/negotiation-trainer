using UnityEngine;

public class ScrollAndFadeTexture : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public float fadeInTime = 3f;
    public float fadeOutTime = 3f;
    private Renderer rend;
    private float currentAlpha = 0f;
    private float targetAlpha = 0f;
    private float fadeTimer = 0f;
    public bool fadeInRequested = false;
    public bool fadeOutRequested = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Scroll texture
        float offset = Time.time * scrollSpeed;
        rend.material.mainTextureOffset = new Vector2(offset, 0);

        // Update alpha based on fading
        if (fadeTimer < fadeInTime && targetAlpha == 255f)
        {
            currentAlpha = Mathf.Lerp(0f, targetAlpha / 255f, fadeTimer / fadeInTime);
        }
        else if (fadeTimer < fadeOutTime && targetAlpha == 0f)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, 0f, fadeTimer / fadeOutTime);
        }

        // Apply the alpha to the material
        Color color = rend.material.color;
        color.a = currentAlpha;
        rend.material.color = color;

        // Increment the fade timer
        fadeTimer += Time.deltaTime;

        // Check if fading in or out is requested
        if (fadeInRequested)
        {
            StartFade(true);
            fadeInRequested = false;
        }
        else if (fadeOutRequested)
        {
            StartFade(false);
            fadeOutRequested = false;
        }
    }

    public void StartFade()
    {
        StartFade(true);
    }

    public void StopFade()
    {
        StartFade(false);
    }

    private void StartFade(bool fadeIn)
    {
        fadeTimer = 0f;
        targetAlpha = fadeIn ? 255f : 0f;
    }
}