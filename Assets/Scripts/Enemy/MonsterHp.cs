using System.Collections;
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
    public int Burn;
    float timer;

    public void Damage(int damageCount)
    {
        if (damageCount > 0) { }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
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
        Hp -= damageCount;
        //击退
        DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform,DMGtype);

    }
    void Start()
    {
        Hp = HpMax;
        anim = GetComponent<Animator>();
        DamageTextControler.Initialize();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"&&CauseDMG)
        {
            collision.gameObject.GetComponent<HealthBarControl>().Damage(1);

        }
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Burn>0&&timer>=0.3f)
        {
            timer = 0;
            Damage(3,3);
            Burn -= 1;
        }
        Hp = Mathf.Clamp(Hp, 0, HpMax);
        if (Hp <= 0 && dead == false)
        {
            // TODO 帧动画事件淡出+destroy 
            // destroy the object or play the dead animation
            Instantiate(drop, gameObject.transform.position, Quaternion.identity);
            dead = true;
			if (GetComponent<StabFish>() == null) anim.SetBool("Die", true);
		}
    }
}
