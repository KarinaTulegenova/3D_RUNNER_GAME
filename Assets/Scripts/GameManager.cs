using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerScript player;
    public GameObject winPanel;
    public GameObject losePanel;

    public int totalCoins;

    public AudioSource audioSource;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    [Header("Animation & VFX")]
    public ParticleSystem winParticles;
    public Light sceneLight;
    public Color normalLightColor = Color.white;
    public Color loseLightColor = new Color(0.25f, 0.25f, 0.35f);
    public float normalLightIntensity = 1f;
    public float loseLightIntensity = 0.35f;

    public event Action<int> ScoreChanged;

    private int collectedCoins;
    private bool isGameOver;

    public bool IsGameOver => isGameOver;
    public int CurrentScore => Player != null ? Player.score : 0;
    public PlayerScript Player
    {
        get
        {
            FindPlayerIfMissing();
            return player;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        FindMissingReferences();
        CacheNormalLightSettings();
        CountCoinsInScene();
        HideEndPanels();
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (player != null)
        {
            player.ResetScore();
            player.SetCanMove(true);
            UpdateScoreUI();
        }

        RestoreLight();

        if (winParticles != null)
        {
            winParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void CollectCoin(PlayerScript collectingPlayer)
    {
        if (isGameOver || collectingPlayer == null)
        {
            return;
        }

        collectingPlayer.AddScore(1);
        collectedCoins++;
        UpdateScoreUI();

        if (collectedCoins >= totalCoins)
        {
            WinGame(collectingPlayer);
        }
    }

    public void HitTrap(PlayerScript hitPlayer)
    {
        if (isGameOver || hitPlayer == null)
        {
            return;
        }

        hitPlayer.AddScore(-1);
        UpdateScoreUI();

        if (hitPlayer.score < 0)
        {
            LoseGame(hitPlayer);
        }
    }

    public void AddCoin()
    {
        CollectCoin(player);
    }

    public void CheckLose()
    {
        if (player != null && player.score < 0)
        {
            LoseGame(player);
        }
    }

    private void WinGame(PlayerScript winningPlayer)
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        winningPlayer.SetCanMove(false);
        winningPlayer.PlayWinAnimation();
        ShowOnlyPanel(winPanel);
        PlayWinVFX();
        PlayMusic(winMusic);
    }

    private void LoseGame(PlayerScript losingPlayer)
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        losingPlayer.SetCanMove(false);
        losingPlayer.PlayLoseAnimation();
        ShowOnlyPanel(losePanel);
        PlayLoseVFX();
        PlayMusic(loseMusic);
    }

    private void FindMissingReferences()
    {
        FindPlayerIfMissing();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (sceneLight == null)
        {
            sceneLight = UnityEngine.Object.FindFirstObjectByType<Light>();
        }
    }

    private void CacheNormalLightSettings()
    {
        if (sceneLight == null)
        {
            return;
        }

        normalLightColor = sceneLight.color;
        normalLightIntensity = sceneLight.intensity;
    }

    private void FindPlayerIfMissing()
    {
        if (player == null)
        {
            player = UnityEngine.Object.FindFirstObjectByType<PlayerScript>();
        }
    }

    private void CountCoinsInScene()
    {
        totalCoins = UnityEngine.Object.FindObjectsByType<CoinScript>(FindObjectsSortMode.None).Length;
    }

    private void HideEndPanels()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    private void ShowOnlyPanel(GameObject panelToShow)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(panelToShow == winPanel);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(panelToShow == losePanel);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void UpdateScoreUI()
    {
        ScoreChanged?.Invoke(CurrentScore);
    }

    private void PlayWinVFX()
    {
        RestoreLight();

        if (winParticles != null)
        {
            winParticles.Play();
        }
    }

    private void PlayLoseVFX()
    {
        if (sceneLight != null)
        {
            sceneLight.color = loseLightColor;
            sceneLight.intensity = loseLightIntensity;
        }
    }

    private void RestoreLight()
    {
        if (sceneLight != null)
        {
            sceneLight.color = normalLightColor;
            sceneLight.intensity = normalLightIntensity;
        }
    }
}
