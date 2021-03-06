﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill9Bullet : MonoBehaviour
{
    public GameManager gm;
    public int DmgPerLevel = 3;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Destroy(gameObject, 2);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            GetComponent<Animator>().SetTrigger("Hit");
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(7, 11) + gm.levels[3] * DmgPerLevel) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt(Random.Range(7, 11) + gm.levels[3] * DmgPerLevel));
                collision.gameObject.GetComponent<BossHp>().Burn = 3;
                //Destroy(gameObject);
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(7, 11) + gm.levels[3] * DmgPerLevel) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt(Random.Range(7, 11) + gm.levels[3] * DmgPerLevel));
                collision.gameObject.GetComponent<MonsterHp>().Burn = 3;
                //Destroy(gameObject);

            }
        }
    }
}
