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
    public float minimumX = 6;
    public float maxmumX = 9;
    public float minimumY = 6;
    public float maxmumY = 10;
    public GameObject[] xise;
    public int YellowPos;
    public int GreenPos;
    public int BluePos;
    public int RedPos;
	public int EXPValue = 0;

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
    public void Damage2(int damageCount)
    {
        float MinimumX = minimumX / maxmumX;
        float MinimumY = minimumY / maxmumY;
        float xisedaoju = Random.Range(0, 100);
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
        //生成吸色道具
        {
            float rand_VelocityX = -Mathf.Abs(Mathf.Sqrt(-2 * Mathf.Log(Random.value)) * Mathf.Sin(2 * Mathf.PI * Random.value));
            float rand_VelocityY = Mathf.Abs(Mathf.Sqrt(-2 * Mathf.Log(Random.value)) * Mathf.Sin(2 * Mathf.PI * Random.value));
            if (xisedaoju > 0 && xisedaoju < YellowPos)
            {
                var ins1 = Instantiate(xise[0], transform.position, Quaternion.identity);
                ins1.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
            if (xisedaoju > 25 && xisedaoju < 25 + GreenPos)
            {
                var ins2 = Instantiate(xise[1], transform.position, Quaternion.identity);
                ins2.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
            if (xisedaoju > 50 && xisedaoju < 50 + BluePos)
            {
                var ins3 = Instantiate(xise[2], transform.position, Quaternion.identity);
                ins3.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
            if (xisedaoju > 75 && xisedaoju < 75 + RedPos)
            {
                var ins4 = Instantiate(xise[3], transform.position, Quaternion.identity);
                ins4.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
        }

    }

	private void OnDestroy()
	{
		if (EXPValue > 0) {
			PlayerPrefs.SetInt("EXP", PlayerPrefs.GetInt("EXP", 0) + EXPValue);
			Debug.Log("CurrentEXP:" + PlayerPrefs.GetInt("EXP", 0));
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
    public void Damage2(int damageCount, int DMGtype)
    {
        float MinimumX = minimumX / maxmumX;
        float MinimumY = minimumY / maxmumY;
        float xisedaoju = Random.Range(0, 100);
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
        //生成吸色道具
        {
            float rand_VelocityX = -Mathf.Abs(Mathf.Sqrt(-2 * Mathf.Log(Random.value)) * Mathf.Sin(2 * Mathf.PI * Random.value));
            float rand_VelocityY = Mathf.Abs(Mathf.Sqrt(-2 * Mathf.Log(Random.value)) * Mathf.Sin(2 * Mathf.PI * Random.value));
            if (xisedaoju > 0 && xisedaoju < YellowPos)
            {
                var ins1 = Instantiate(xise[0], transform.position, Quaternion.identity);
                ins1.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
            if (xisedaoju > 25 && xisedaoju < 25 + GreenPos)
            {
                var ins2 = Instantiate(xise[1], transform.position, Quaternion.identity);
                ins2.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
            if (xisedaoju > 50 && xisedaoju < 50 + BluePos)
            {
                var ins3 = Instantiate(xise[2], transform.position, Quaternion.identity);
                ins3.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
            if (xisedaoju > 75 && xisedaoju < 75 + RedPos)
            {
                var ins4 = Instantiate(xise[3], transform.position, Quaternion.identity);
                ins4.GetComponent<Rigidbody2D>().velocity = new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
            }
        }


    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        HpMax = HpMax + gm.levels[0] * 70 + gm.levels[1] * 70 + gm.levels[2] * 70 + gm.levels[3] * 70;
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
        YellowPos = gm.YellowPos;
        GreenPos = gm.GreenPos;
        BluePos = gm.BluePos;
        RedPos = gm.RedPos;
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
