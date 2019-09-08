using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill9 : MonoBehaviour
{
    Rigidbody2D rb;
    public LayerMask enemyLayer;
    Collider2D nearest;
    private void Start()
    {
        FindEnemy();
        if (nearest != null)
        {
            transform.right = nearest.transform.position - transform.position;
        }
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, 8, enemyLayer);
        if (list.Length == 0)
        {
            nearest = null;
        }
        else
        {
            nearest = list[0];
            foreach (Collider2D col in list)
            {
                if (Vector2.Distance(new Vector2(col.transform.position.x, col.transform.position.y), new Vector2(gameObject.transform.position.x, col.transform.position.y)) <= Vector2.Distance(new Vector2(nearest.transform.position.x, nearest.transform.position.y), new Vector2(gameObject.transform.position.x, col.transform.position.y)))
                    nearest = col;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13)-4 )));

            }
            else
            {
                collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13)))-4);

            }
        }
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
        }

    }

    private void Update()
    {
            rb.velocity = transform.right * 5;
    }
}
