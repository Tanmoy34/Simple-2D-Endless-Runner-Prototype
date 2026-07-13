using UnityEngine;

/// <summary>
/// Pure gameplay logic — no UI references at all. It only raises events;
/// UIManager is the one that listens and updates text. This is the
/// "separate gameplay logic from UI logic" requirement in practice.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameConfig config;

    private float scoreAccumulator;
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    void Start()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void OnEnable()
    {
        GameEvents.OnGameStart += ResetScore;
        GameEvents.OnGameOver += SaveHighScoreIfNeeded;
    }

    void OnDisable()
    {
        GameEvents.OnGameStart -= ResetScore;
        GameEvents.OnGameOver -= SaveHighScoreIfNeeded;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        scoreAccumulator += config.scorePerSecond * Time.deltaTime;
        int newScore = Mathf.FloorToInt(scoreAccumulator);

        if (newScore != CurrentScore)
        {
            CurrentScore = newScore;
            GameEvents.RaiseScoreChanged(CurrentScore);
        }
    }

    private void ResetScore()
    {
        scoreAccumulator = 0f;
        CurrentScore = 0;
        GameEvents.RaiseScoreChanged(0);
    }

    private void SaveHighScoreIfNeeded()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
    }
}
