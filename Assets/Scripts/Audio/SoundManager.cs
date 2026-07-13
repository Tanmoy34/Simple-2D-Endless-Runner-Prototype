using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip gameOverClip;

    void OnEnable()
    {
        GameEvents.OnPlayerJumped += PlayJump;
        GameEvents.OnGameOver += PlayGameOver;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerJumped -= PlayJump;
        GameEvents.OnGameOver -= PlayGameOver;
    }

    private void PlayJump()
    {
        if (audioSource != null && jumpClip != null)
            audioSource.PlayOneShot(jumpClip);
    }

    private void PlayGameOver()
    {
        if (audioSource != null && gameOverClip != null)
            audioSource.PlayOneShot(gameOverClip);
    }
}
