using UnityEngine;
public class BoxCast2DDectection : MonoBehaviour
{
    public Vector2 boxSize = new Vector2(1f, 1f);
    public float maxDistance = 2.5f;
    public LayerMask obstacleLayer;
    private RaycastHit2D hitInfo;
    [SerializeField] bool IsGizmos = false;

    public RaycastHit2D PerformBoxCast()
    {
        Vector2 direction = transform.up;
        hitInfo = Physics2D.BoxCast(transform.position, boxSize, 0f, direction, maxDistance, obstacleLayer);
        if (IsGizmos)
             BoxCastDrawer2D.DrawBoxCast(transform.position, direction, boxSize, maxDistance, hitInfo.collider != null);
        return hitInfo;
    }

    public Vector2 BoxSize => boxSize;
    public float MaxDistance => maxDistance;
    public LayerMask ObstacleLayer => obstacleLayer;
}