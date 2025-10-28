using UnityEngine;
using UnityEngine.Pool;

public class Pickup : MonoBehaviour
{
    private ScoreManager scoreManager;
    private ObjectPool objectPool;
    private float lifetime = 10f; // Time in seconds before the pickup returns to the pool

    void OnEnable()
    {
        CancelInvoke();
        Invoke("AutoReturn", lifetime);
    }

    void AutoReturn()
    {
        if (objectPool != null)
        {
            objectPool.ReturnToPool(this.gameObject);
        }
    }

    public void SetPool(ObjectPool pool)
    {
        objectPool = pool;
    }

    void Start()
    {
        // transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Logic for when the player collects the pickup. Play sound/effects here.
            Debug.Log("Pickup collected!");

            if (objectPool != null)
            {
                objectPool.ReturnToPool(this.gameObject);
            }
            else
            {
                gameObject.SetActive(false); // Deactivate the pickup
            }
            
            scoreManager.pickupCount += 1;
        }
    }
}
