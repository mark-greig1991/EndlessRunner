using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] FadeController fadeController;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Transform player;

    [Header("Difficulty Settings")]
    [SerializeField] float baseSpeed = 10f;
    [SerializeField] float maxSpeed = 25f;
    [SerializeField] float accelerationRate = 0.05f;

    private float currentSpeed;

    private bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(fadeController.FadeIn(0.5f));
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
    }

    public void GameOver()
    {
        FindFirstObjectByType<ScoreManager>().OnGameOver();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // resume the game
        StartCoroutine(RestartWithFade());
    }

    IEnumerator RestartWithFade()
    {
        yield return StartCoroutine(fadeController.FadeOut(0.5f));
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
