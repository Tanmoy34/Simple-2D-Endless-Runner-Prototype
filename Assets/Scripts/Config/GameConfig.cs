using UnityEngine;


[CreateAssetMenu(fileName = "GameConfig", menuName = "EndlessRunner/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Speed / Difficulty Scaling")]
    public float baseScrollSpeed = 5f;
    public float speedIncreasePerSecond = 0.05f;
    public float maxScrollSpeed = 15f;

    [Header("Obstacles")]
    public float minSpawnInterval = 1.2f;
    public float maxSpawnInterval = 2.5f;
    public int obstaclePoolSize = 6;

    [Header("Player")]
    public float jumpForce = 12f;
    public float scorePerSecond = 10f;
}
