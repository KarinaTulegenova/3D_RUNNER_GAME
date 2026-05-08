using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public PlayerScript player;

    private GameManager gameManager;
    private bool isSubscribed;

    private void Awake()
    {
        if (scoreText == null)
        {
            scoreText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (player == null)
        {
            player = UnityEngine.Object.FindFirstObjectByType<PlayerScript>();
        }
    }

    private void OnEnable()
    {
        SubscribeToGameManager();
    }

    private void Start()
    {
        SubscribeToGameManager();
        UpdateScoreText(gameManager != null ? gameManager.CurrentScore : GetPlayerScore());
    }

    private void OnDisable()
    {
        if (gameManager != null && isSubscribed)
        {
            gameManager.ScoreChanged -= UpdateScoreText;
            isSubscribed = false;
        }
    }

    private void SubscribeToGameManager()
    {
        if (isSubscribed)
        {
            return;
        }

        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            gameManager = UnityEngine.Object.FindFirstObjectByType<GameManager>();
        }

        if (gameManager != null)
        {
            gameManager.ScoreChanged += UpdateScoreText;
            isSubscribed = true;
        }
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private int GetPlayerScore()
    {
        return player != null ? player.score : 0;
    }
}
