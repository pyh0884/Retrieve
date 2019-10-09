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
    public GameObject drop2;
    public float Burn;
    int BurnDamage;
    float timer;
    GameManager gm;
    public CoinsGenerator cg;
    public float minimumX = 6;
    public float maxmumX = 9;
    public float minimumY = 6;
    public float maxmumY = 10;
    public GameObject[] xise;
    public int YellowPos;
    public int GreenPos;
    public int BluePos;
    public int RedPos;
    public bool IsElite;
    public GameObject FireEFX;
	[Header("加点值")]
	public int EXPValue = 0;
	public int EliteMultiply = 3;


	public void Damage2(int damageCount)
    {
        float MinimumX = minimumX / maxmumX;
        float MinimumY = minimumY / maxmumY;
        float xisedaoju = Random.Range(0,100);
        if (damageCount > 0) { }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        Hp -= damageCount;

        //击退
            if (anim&& !IsElite)
                anim.SetTrigger("Hit");
        DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform,2);
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
    public void Damage(int damageCount)
    {
        if (damageCount > 0) { }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        if (anim && !IsElite)
            anim.SetTrigger("Hit");
        Hp -= damageCount;
        //击退
        DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform, 2);
        }
    public void Damage2(int damageCount,int DMGtype)
    {
        float MinimumX = minimumX / maxmumX;
        float MinimumY = minimumY / maxmumY;
        float xisedaoju = Random.Range(0, 100);
        if (damageCount > 0) { }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        //anim.SetTrigger("Hit");
        if (DMGtype!=3)
            if (anim && !IsElite)
                anim.SetTrigger("Hit");
        Hp -= damageCount;
        //击退
        DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform,DMGtype);
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
		if (EXPValue > 0&&Hp<=0)
		{
			PlayerPrefs.SetInt("EXP", PlayerPrefs.GetInt("EXP", 0) + EXPValue * (IsElite ? EliteMultiply : 1));
			Debug.Log("CurrentEXP:" + PlayerPrefs.GetInt("EXP", 0));
		}
	}

	public void Damage(int damageCount, int DMGtype)
    {
        if (damageCount > 0) { }
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        //anim.SetTrigger("Hit");
        if (DMGtype != 3)
            if (anim && !IsElite)
                anim.SetTrigger("Hit");
        Hp -= damageCount;
        //击退
        DamageTextControler.CreatDamageText(damageCount.ToString(), gameObject.transform, DMGtype);
    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        HpMax= HpMax + gm.levels[0] * 15 + gm.levels[1] * 15 + gm.levels[2] * 15 + gm.levels[3] * 15;
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
        YellowPos = gm.YellowPos;
        GreenPos = gm.GreenPos;
        BluePos = gm.BluePos;
        RedPos = gm.RedPos;
        BurnDamage = gm.BurnDamage;
        timer += Time.deltaTime;
        if (Burn>0&&timer>=0.35f)
        {
            timer = 0;
            Damage(BurnDamage,3);
            Burn -= 1;
        }
        if (Burn > 0) { FireEFX.SetActive(true); }
        else
        {
            FireEFX.SetActive(false);
        }
        Hp = Mathf.Clamp(Hp, 0, HpMax);
        if (Hp <= 0 && dead == false)
        {
            cg.GenCoins();
            // TODO 帧动画事件淡出+destroy 
            // destroy the object or play the dead animation
            if (Random.Range(0, 100) > 70)
            {
                Instantiate(drop, gameObject.transform.position, Quaternion.identity);
            }
            dead = true;
            if (GetComponent<StabFish>() == null)
            {
                anim.SetBool("Die", true);
            }
		}
    }
}
