using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
        {
            return;
        }

        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            gameManager = UnityEngine.Object.FindFirstObjectByType<GameManager>();
        }

        PlayerScript player = other.GetComponentInParent<PlayerScript>();
        if (player == null && gameManager != null)
        {
            player = gameManager.Player;
        }

        if (player == null)
        {
            player = UnityEngine.Object.FindFirstObjectByType<PlayerScript>();
        }

        if (gameManager == null || gameManager.IsGameOver)
        {
            return;
        }

        collected = true;
        gameManager.CollectCoin(player);
        Destroy(gameObject);
    }
}
