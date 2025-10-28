using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool isDead;
    [SerializeField] GameObject mainCamera;

    // Camera shake for collisions.  
    IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    // Game Over logic
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Unjumpable") && !isDead)
        {
            StartCoroutine(CameraShake(0.2f, 0.15f));
            isDead = true;
            Debug.Log("Player has collided with an obstacle and is dead.");
            FindFirstObjectByType<GameController>().GameOver();
        }
    }
}
