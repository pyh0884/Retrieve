﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHp : MonoBehaviour
{
    public float Hp;
    public float HpMax;
    private Animator anim;
    public bool dead = false;
    public bool CauseDMG=false;
    public GameObject drop;
    public GameObject drop2;
    public float Burn;
    int BurnDamage;
    float timer;
    GameManager gm;

    public void Damage(int damageCount)
    {
        if (damageCount > 0) { }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        if (anim)
        anim.SetTrigger("Hit");
        Hp -= damageCount;
        //击退
        DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform,2);

    }
    public void Damage(int damageCount,int DMGtype)
    {
        if (damageCount > 0) { }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        //anim.SetTrigger("Hit");
        if (DMGtype!=3)
            if(anim)
        anim.SetTrigger("Hit");
        Hp -= damageCount;
        //击退
        DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform,DMGtype);

    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Hp = HpMax;
        anim = GetComponent<Animator>();
        DamageTextControler.Initialize();
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        BurnDamage = gm.BurnDamage;
        timer += Time.deltaTime;
        if (Burn>0&&timer>=0.35f)
        {
            timer = 0;
            Damage(BurnDamage,3);
            Burn -= 1;
        }
        Hp = Mathf.Clamp(Hp, 0, HpMax);
        if (Hp <= 0 && dead == false)
        {
            // TODO 帧动画事件淡出+destroy 
            // destroy the object or play the dead animation
            if (Random.Range(0, 100) > 60)
            {
                Instantiate(drop, gameObject.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(drop2, gameObject.transform.position, Quaternion.identity);
            }
            dead = true;
            if (GetComponent<StabFish>() == null) anim.SetBool("Die", true);
		}
    }
}
