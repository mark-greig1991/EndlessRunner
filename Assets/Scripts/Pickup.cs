using UnityEngine;

public class Pickup : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        // transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Logic for when the player collects the pickup
            Debug.Log("Pickup collected!");
            gameObject.SetActive(false); // Deactivate the pickup
            scoreManager.pickupCount += 1;
        }
    }
}
