using UnityEngine;

/// <summary>
/// A single obstacle instance. It doesn't know or care whether it was
/// freshly instantiated or reused from the pool — it just moves and
/// returns itself once off-screen.
/// </summary>
public class Obstacle : MonoBehaviour
{
    private const float DespawnX = -12f;
    private ObjectPool<Obstacle> owningPool;

    public void Init(ObjectPool<Obstacle> pool)
    {
        owningPool = pool;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        float speed = GameManager.Instance.CurrentScrollSpeed;
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < DespawnX)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (owningPool != null)
        {
            owningPool.Return(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
