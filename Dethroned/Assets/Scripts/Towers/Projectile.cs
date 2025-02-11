using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public Vector2 offset, bounds;
    public LayerMask EnemyLayer;

    private void FixedUpdate()
    {
        var collision = Physics2D.OverlapBox(transform.position + (Vector3)offset, bounds, Vector2.Angle(Vector2.zero, target.transform.position), EnemyLayer);
        if (collision)
            if (collision.gameObject.Equals(target))
                gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if ((bounds.x == 0 && bounds.y == 0) && (transform.lossyScale.x != 0 && transform.lossyScale.y != 0)) bounds = transform.lossyScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)offset, (Vector3)bounds);
    }
}
