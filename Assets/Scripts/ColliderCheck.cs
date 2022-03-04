using UnityEngine;
using System.Collections;

public class ColliderCheck : MonoBehaviour
{
    public bool CheckCollision(float radius, LayerMask whatIsGround, GameObject ignoreObject = null)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsGround);
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject != ignoreObject)
            {
                return true;
            }
        }
        return false;
    }
}