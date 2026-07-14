using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameConfig config;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;

    private Rigidbody2D rb;
    private int jumpsRemaining;
    private const int MaxJumps = 2; // ground jump + 1 air (double) jump
    private Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void OnEnable() => GameEvents.OnGameStart += ResetPlayer;
    void OnDisable() => GameEvents.OnGameStart -= ResetPlayer;

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        bool jumpPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space);

        // Bonus: basic mobile tap-to-jump support.
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            jumpPressed = true;
        }

        if (IsGrounded())
        {
            jumpsRemaining = MaxJumps;
        }

        if (jumpPressed)
        {
            TryJump();
        }
    }

    private void TryJump()
    {
        if (jumpsRemaining <= 0) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * config.jumpForce, ForceMode2D.Impulse);
        jumpsRemaining--;

        GameEvents.RaisePlayerJumped();
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void ResetPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPosition;
        jumpsRemaining = MaxJumps;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}
