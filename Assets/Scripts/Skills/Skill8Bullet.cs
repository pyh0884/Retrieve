﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill8Bullet : MonoBehaviour
{
    public GameManager gm;

    Vector3 EnemyPosition;
    public float speed;
    Collider2D nearest;
    public LayerMask enemyLayer;
    public Vector3 DefaultDir;
    public GameObject balls;
    public int numbers = 5;
    public GameObject AccArea;
    public void SpeedUp()
    {
        Instantiate(AccArea, new Vector3(Mathf.RoundToInt(transform.position.x / 4f) * 4f, transform.position.y), Quaternion.Euler(0, 0, 0));
    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        FindEnemy();
        if (nearest != null)
        {
            EnemyPosition = nearest.transform.position;
            DefaultDir = EnemyPosition - transform.position;
        }
        transform.up = -DefaultDir;
        GetComponent<Rigidbody2D>().velocity = DefaultDir.normalized * speed;
        numbers = 5 + gm.levels[1] * 2;
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
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10) + 10) * 1.5f) , 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10) + 10)));
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10) + 10) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10) + 10)));
            }
        }
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
        {
            SpeedUp();
            for (int i = 0; i < numbers; i++)
            {
                Instantiate(balls, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }

    }

}
