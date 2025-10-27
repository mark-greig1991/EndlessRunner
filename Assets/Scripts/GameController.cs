using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Transform player;

    [Header("Difficulty Settings")]
    [SerializeField] float baseSpeed = 10f;
    [SerializeField] float maxSpeed = 25f;
    [SerializeField] float accelerationRate = 0.05f;

    private float currentSpeed;

    private bool isGameOver = false;
    private float score = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = baseSpeed;
        gameOverPanel.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        // Gradually increase speed up to maxSpeed
        if (!isGameOver)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, accelerationRate * Time.deltaTime);
            player.GetComponent<PlayerController>().SetForwardSpeed(currentSpeed);
        }
        if (isGameOver)
        {
            score = player.position.z;
            scoreText.text = "Final Score: " + Mathf.FloorToInt(score).ToString();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
