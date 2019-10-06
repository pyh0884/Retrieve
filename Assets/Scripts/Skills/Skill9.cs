﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill9 : MonoBehaviour
{
    public GameManager gm;
    public LayerMask enemyLayer;
    Collider2D nearest;
    public float LastTime = 6;
    public GameObject MainObj;
    public Transform paokou;
    private Vector3 direction;
    public GameObject Bullet;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        LastTime = 6 + gm.levels[2] * 1.2f;
        Destroy(MainObj,LastTime);
    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, 15, enemyLayer);
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
    public void shoot()
    {
        GameObject obj = Instantiate(Bullet,paokou.position,Quaternion.FromToRotation(transform.position,direction));
        obj.transform.up = direction;
        obj.GetComponent<Rigidbody2D>().velocity = obj.transform.up * 10;
    }


    private void Update()
    {
        FindEnemy();
        if (nearest)
        {
            GetComponent<Animator>().SetBool("Attack", true);
            direction = nearest.transform.position - transform.position;
            transform.right = direction;
        }
        else
        {
            GetComponent<Animator>().SetBool("Attack", false);
        }
    }
}
