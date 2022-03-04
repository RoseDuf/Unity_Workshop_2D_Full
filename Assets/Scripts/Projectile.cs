using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(lifetimeRoutine());
    }

    public void Shoot(Vector2 direction)
    {
        transform.localScale = new Vector3(transform.localScale.x, direction.x * transform.localScale.y, 0);
        m_Rigidbody.AddForce(direction * m_Speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator lifetimeRoutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
