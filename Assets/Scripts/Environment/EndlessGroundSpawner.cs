using System.Collections.Generic;
using UnityEngine;


public class EndlessGroundSpawner : MonoBehaviour
{
    [SerializeField] private GroundSegment groundPrefab;
    [SerializeField] private int segmentsOnScreen = 4;
    [SerializeField] private float recycleOffsetX = -12f;

    private readonly List<GroundSegment> activeSegments = new List<GroundSegment>();
    private float segmentWidth;

    void Start()
    {
        GroundSegment first = Instantiate(groundPrefab, Vector3.zero, Quaternion.identity, transform);
        segmentWidth = first.Width;
        activeSegments.Add(first);

        for (int i = 1; i < segmentsOnScreen; i++)
        {
            SpawnNextSegment();
        }
    }

    void OnEnable() => GameEvents.OnGameStart += ResetGround;
    void OnDisable() => GameEvents.OnGameStart -= ResetGround;

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        float speed = GameManager.Instance.CurrentScrollSpeed * Time.deltaTime;
        foreach (GroundSegment seg in activeSegments)
        {
            seg.transform.position += Vector3.left * speed;
        }

        if (activeSegments[0].transform.position.x < recycleOffsetX)
        {
            RecycleFirstSegment();
        }
    }

    private void SpawnNextSegment()
    {
        Vector3 spawnPos = activeSegments[activeSegments.Count - 1].transform.position
                            + Vector3.right * segmentWidth;
        GroundSegment seg = Instantiate(groundPrefab, spawnPos, Quaternion.identity, transform);
        activeSegments.Add(seg);
    }

    private void RecycleFirstSegment()
    {
        GroundSegment seg = activeSegments[0];
        activeSegments.RemoveAt(0);

        Vector3 newPos = activeSegments[activeSegments.Count - 1].transform.position
                          + Vector3.right * segmentWidth;
        seg.transform.position = newPos;
        activeSegments.Add(seg);
    }

    private void ResetGround()
    {
        for (int i = 0; i < activeSegments.Count; i++)
        {
            activeSegments[i].transform.position = new Vector3(i * segmentWidth, 0f, 0f);
        }
    }
}
