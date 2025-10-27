using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Transform player;
    private float score = 0f;
    [SerializeField] float multiplier = 1f;

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * multiplier * player.GetComponent<PlayerController>().forwardSpeed;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public void IncreaseMultiplier(float amount)
    {
        multiplier += amount;
    }
}
