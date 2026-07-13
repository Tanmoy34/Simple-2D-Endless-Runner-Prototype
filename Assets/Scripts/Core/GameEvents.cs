using System;


public static class GameEvents
{
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action<int> OnScoreChanged;
    public static event Action OnPlayerJumped;

    public static void RaiseGameStart() => OnGameStart?.Invoke();
    public static void RaiseGameOver() => OnGameOver?.Invoke();
    public static void RaiseScoreChanged(int score) => OnScoreChanged?.Invoke(score);
    public static void RaisePlayerJumped() => OnPlayerJumped?.Invoke();
}
