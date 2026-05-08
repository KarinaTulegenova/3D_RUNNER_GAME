using UnityEngine;

public class TrapScript : MonoBehaviour
{
    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        TryHit(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryHit(collision.gameObject);
    }

    public void TryHit(GameObject other)
    {
        if (activated)
        {
            return;
        }

        activated = true;

        HitPlayer(other);
    }

    private void HitPlayer(GameObject other)
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            gameManager = UnityEngine.Object.FindFirstObjectByType<GameManager>();
        }

        PlayerScript player = other != null ? other.GetComponentInParent<PlayerScript>() : null;
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
            activated = false;
            return;
        }

        gameManager.HitTrap(player);
    }
}
