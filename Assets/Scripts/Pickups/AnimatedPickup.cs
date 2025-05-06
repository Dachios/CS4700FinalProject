// Written by Dachi
using UnityEngine;

public class AnimatedPickup : MonoBehaviour
{

    private float FREQUENCY = 1f;
    private float AMPLITUDE = 0.25f;

    float currentY;
    float newY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store original local Y value
        float currentY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Handles Rotation
        transform.Rotate(0.0f, 0.5f, 0.0f);

        // Handles the bouncing motion
        newY = (currentY + (Mathf.Sin(Time.time * FREQUENCY) * AMPLITUDE));
        var position = transform.localPosition;
        position.y = newY;
        transform.localPosition = position;

    }
}
