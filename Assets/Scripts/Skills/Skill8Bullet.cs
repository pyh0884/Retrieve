using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill8Bullet : MonoBehaviour
{
    Vector3 EnemyPosition;
    public float speed;
    Collider2D nearest;
    public LayerMask enemyLayer;
    public float SlerpNum;
    public Vector3 relativeCenter;
    // Start is called before the first frame update
    void Start()
    {
        FindEnemy();
        if (nearest!=null)
        EnemyPosition = nearest.transform.position;
        else
        {
            Destroy(gameObject);
        }

    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, 20, enemyLayer);
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
                collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 10)));

            }
            else
            {
                collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 10)));
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

    }
    void Update()
    {
            //transform.up = Vector3.Slerp(transform.up- relativeCenter, EnemyPosition - transform.position - relativeCenter, SlerpNum / Vector2.Distance(this.transform.position, EnemyPosition));
        //transform.up += relativeCenter;
        transform.position= Vector3.Slerp(transform.up - relativeCenter, EnemyPosition - relativeCenter, .3f);
        transform.position += relativeCenter;
        //transform.position += transform.up * speed * Time.deltaTime;
    }
}
