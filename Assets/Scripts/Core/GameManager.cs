using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Playing,
    GameOver
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameConfig config;

    public GameState CurrentState { get; private set; } = GameState.Playing;
    public bool IsPlaying => CurrentState == GameState.Playing;

    private float elapsedPlayTime;
    public float CurrentScrollSpeed { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!IsPlaying) return;

        elapsedPlayTime += Time.deltaTime;
        CurrentScrollSpeed = Mathf.Min(
            config.maxScrollSpeed,
            config.baseScrollSpeed + elapsedPlayTime * config.speedIncreasePerSecond);
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        elapsedPlayTime = 0f;
        CurrentScrollSpeed = config.baseScrollSpeed;
        Time.timeScale = 1f;
        GameEvents.RaiseGameStart();
    }

    public void TriggerGameOver()
    {
        if (CurrentState == GameState.GameOver) return;
        CurrentState = GameState.GameOver;
        GameEvents.RaiseGameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public GameConfig Config => config;
}
