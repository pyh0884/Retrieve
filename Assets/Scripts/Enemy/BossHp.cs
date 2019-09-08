using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    public Slider slider;
    public float Hp;
    public float HpMax;
    private Animator anim;
    private bool dead = false;
	public Transform bossTransform;
    public ParticleSystem ps;
	public bool isBoss;
    public int BossIndex;
    public int Burn;
    float timer;
    public void Damage(int damageCount)
    {
        if (damageCount > 0) {
            ps.Play();
        }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        //       anim.SetTrigger("Hit");
        Hp -= damageCount;
		if (bossTransform != null)
			DamageTextControler.CreatDamageText(damageCount.ToString(), bossTransform);
		else
			DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform);


	}
    void Start()
    {
        Hp = HpMax;
        anim = GetComponent<Animator>();
        DamageTextControler.Initialize();
        
    }
    IEnumerator Die()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (Burn > 0 && timer >= 0.3f)
        {
            timer = 0;
            Damage(3);
            Burn -= 1;
        }
        Hp = Mathf.Clamp(Hp, 0, HpMax);
        if (Hp <= 0 && dead == false)
        {
            // TODO 帧动画事件淡出+destroy 
            // destroy the object or play the dead animation
            
            FindObjectOfType<GameManager>().SetBossIndex(BossIndex);
            dead = true;
            StartCoroutine("Die");
        }

		if(isBoss)slider.value = (float)(Hp / HpMax);

    }

}
