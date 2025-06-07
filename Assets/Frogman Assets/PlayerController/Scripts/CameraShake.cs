using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeDuration, shakeMagnitude;
    public int pixelsPerUnit = 32; // Match your Pixel Perfect Camera's PPU
    private Vector3 originalPosition;
    private float shakeTimer;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude;

            // Snap to pixel grid
            shakeOffset.x = Mathf.Round(shakeOffset.x * pixelsPerUnit) / pixelsPerUnit;
            shakeOffset.y = Mathf.Round(shakeOffset.y * pixelsPerUnit) / pixelsPerUnit;

            transform.localPosition = originalPosition + (Vector3)shakeOffset;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimer = duration;
    }
}
