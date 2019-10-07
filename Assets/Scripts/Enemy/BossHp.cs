using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    public Slider slider;
    public float Hp;
    public float TargetHp;
    public float lerpSpeed = 5;
    public float HpMax;
    private Animator anim;
    private bool dead = false;
	public Transform bossTransform;
    public ParticleSystem ps;
	public bool isBoss;
    public int BossIndex;
    public float Burn;
    public bool Shield=false;
    float timer;
    private GameManager gm;
    int BurnDamage;

    public void Damage(int damageCount)
    {
        if (!Shield)
        {
            if (damageCount > 0)
            {
                ps.Play();
            }
            TargetHp -= damageCount;
            TargetHp = Mathf.Clamp(TargetHp, 0, HpMax);
            if (bossTransform != null)
                DamageTextControler.CreatDamageText(damageCount.ToString(), bossTransform, 2);
            else
                DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform, 2);

        }
        else
        {
            if (damageCount > 0)
            {
                ps.Play();
            }
            TargetHp -= 1;
            TargetHp = Mathf.Clamp(TargetHp, 0, HpMax);
            if (bossTransform != null)
                DamageTextControler.CreatDamageText(1.ToString(), bossTransform, 2);
            else
                DamageTextControler.CreatDamageText(1.ToString(), gameObject.transform, 2);

        }
    }
    public void Damage(int damageCount,int DMGtype)
    {
        if (!Shield)
        {

            if (damageCount > 0)
            {
                ps.Play();
            }
            TargetHp -= damageCount;
            TargetHp = Mathf.Clamp(TargetHp, 0, HpMax);
            if (bossTransform != null)
                DamageTextControler.CreatDamageText(damageCount.ToString(), bossTransform, DMGtype);
            else
                DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform, DMGtype);
        }
        else
        {
            if (damageCount > 0)
            {
                ps.Play();
            }
            TargetHp -= 1;
            TargetHp = Mathf.Clamp(TargetHp, 0, HpMax);
            if (bossTransform != null)
                DamageTextControler.CreatDamageText(1.ToString(), bossTransform, DMGtype);
            else
                DamageTextControler.CreatDamageText(1.ToString(), gameObject.transform, DMGtype);

        }


    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        HpMax += gm.DAMAGE*100;
        Hp = HpMax;
        TargetHp = Hp;
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
        BurnDamage = gm.BurnDamage;
        timer += Time.deltaTime;
        if (Burn > 0 && timer >= 0.35f)
        {
            timer = 0;
            Damage(BurnDamage, 3);
            Burn -= 1;
        }
        Hp = Mathf.Clamp(Hp, 0, HpMax);
        if ((Hp <= 0|| TargetHp<=0) && dead == false)
        {
            // TODO 帧动画事件淡出+destroy 
            // destroy the object or play the dead animation
            
            FindObjectOfType<GameManager>().SetBossIndex(BossIndex);
            dead = true;
            gm.GetMoney(150);
            StartCoroutine("Die");
        }

        if (isBoss&&slider)
        {
            Hp = Mathf.Lerp(Hp, TargetHp, Time.deltaTime * lerpSpeed);
            slider.value = (float)(Hp / HpMax);
        }

    }

}
