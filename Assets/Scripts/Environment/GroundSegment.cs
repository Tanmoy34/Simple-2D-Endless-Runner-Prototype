using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class GroundSegment : MonoBehaviour
{
    public float Width { get; private set; }

    void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        Width = col.bounds.size.x;
    }
}
