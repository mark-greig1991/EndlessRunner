using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI highScoreText;

    [Header("Score settings")]
    [SerializeField] float scoreMultiplier = 1.5f;

    private float score = 0f;
    private int highScore;

    private bool isGameOver = false;
    private float startZ;

    void Start()
    {
        startZ = player.position.z;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        // calculate score based on distance travelled
        float distance = player.position.z - startZ;
        score = distance * scoreMultiplier;

        UpdateScoreUI();

        // score += Time.deltaTime * scoreMultiplier * player.GetComponent<PlayerController>().forwardSpeed;
        // scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
    }

    void UpdateHighScoreUI()
    {
        highScoreText.text = $"High Score: {highScore}";
    }
    
    public void OnGameOver()
    {
        isGameOver = true;
        int finalScore = Mathf.FloorToInt(score);

        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateHighScoreUI();
    }
}
