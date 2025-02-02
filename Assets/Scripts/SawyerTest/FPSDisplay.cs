using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Assign your TextMeshPro UI component in the Inspector
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculate delta time and FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Update the TextMeshPro text 
        if (fpsText != null)
        {
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}