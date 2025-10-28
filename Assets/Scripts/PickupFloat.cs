using UnityEngine;

public class PickupFloat : MonoBehaviour
{
    public float floatAmplitude = 0.25f; // Amplitude of the floating effect
    public float floatFrequency = 2f;   // Frequency of the floating effect
    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.Rotate(Vector3.up, 90f * Time.deltaTime); // Rotate around Y axis at 45 degrees per second
    }
}
