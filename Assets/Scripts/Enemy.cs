using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "arrow")
        {
            Destroy(collision.collider.gameObject);
            StartCoroutine("DieRoutine");
        }
    }

    private IEnumerator DieRoutine()
    {
        m_Animator.SetBool("isDead", true);
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.31f);
        Destroy(gameObject);

    }
}
