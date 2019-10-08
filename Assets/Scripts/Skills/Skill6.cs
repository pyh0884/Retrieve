using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill6 : MonoBehaviour
{
    public GameManager gm;

    Rigidbody2D rb;
    public LayerMask enemyLayer;
    Collider2D nearest;
    public bool moving;
    HealthBarControl hbc;
    public int HealPerLevel = 2;
    public int DmgPerLevel = 10;
    public GameObject AccArea;
    public void SpeedUp()
    {
        Instantiate(AccArea, new Vector3(Mathf.RoundToInt(transform.position.x / 4f) * 4f, transform.position.y), Quaternion.Euler(0, 0, 0));
    }
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        hbc = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBarControl>();
        FindEnemy();
        if (nearest!=null)
        {
            transform.right = nearest.transform.position - transform.position;
        }
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 1);
    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, 10, enemyLayer);
        if (list.Length == 0)
        {
            nearest = null;
        }
        else
        {
            nearest = list[0];
            foreach (Collider2D col in list)
            {
                if (Vector2.Distance(new Vector2(col.transform.position.x, col.transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)) <= Vector2.Distance(new Vector2(nearest.transform.position.x, nearest.transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)))
                    nearest = col;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            hbc.Damage(-HealPerLevel * (gm.levels[2] + 1));
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 25 + gm.levels[2] * DmgPerLevel) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 25 + gm.levels[2] * DmgPerLevel)));
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 25 + gm.levels[2] * DmgPerLevel) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 25 + gm.levels[2] * DmgPerLevel)));
            }
        }
    }
        private void Update()
    {if (moving)
        rb.velocity = transform.right*26;
    }
}