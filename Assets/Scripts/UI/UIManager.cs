using TMPro;
using UnityEngine;

/// <summary>
/// Sole owner of UI logic. Subscribes to GameEvents rather than being
/// called directly by gameplay scripts -- gameplay has zero knowledge
/// that UI even exists.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private ScoreManager scoreManager;

    void OnEnable()
    {
        GameEvents.OnScoreChanged += UpdateScoreText;
        GameEvents.OnGameOver += ShowGameOverScreen;
        GameEvents.OnGameStart += HideGameOverScreen;
    }

    void OnDisable()
    {
        GameEvents.OnScoreChanged -= UpdateScoreText;
        GameEvents.OnGameOver -= ShowGameOverScreen;
        GameEvents.OnGameStart -= HideGameOverScreen;
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
    }

    private void ShowGameOverScreen()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null && scoreManager != null)
            finalScoreText.text = $"Score: {scoreManager.CurrentScore}";
        if (highScoreText != null && scoreManager != null)
            highScoreText.text = $"Best: {scoreManager.HighScore}";
    }

    private void HideGameOverScreen()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    /// <summary>Hook this up to the Restart Button's OnClick() in the Inspector.</summary>
    public void OnRestartButtonPressed()
    {
        GameManager.Instance.RestartGame();
    }
}
