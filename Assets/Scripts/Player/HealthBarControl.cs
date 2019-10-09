using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    public Slider slider;
    public float Hp = 150;
    public float HpMax = 150;
    public Transform PlayerTransform;
    public int IncreaseHp;
    //public int HeartCapacity;
    //public Image[] HeartImages;
    //public Sprite[] HeartSprites;
    private Animator anim;
    public GameManager gm;
    public float invincibleCD;
    public bool cheat;
    public float JumpTimer;
	public float vibrationStrength = 1.5f;
	public float vibrationTime = 0.3f;
    public float TargetHp = 150;
    public float lerpSpeed=5;
    public float lerpSpeed2 = 5;
    public Text textHp;
    public Text textMoney;


    void Awake()
    {
        gm= FindObjectOfType<GameManager>();
        Hp = gm.HP;
        HpMax = gm.MAXHP;
        //HeartCapacity = 10;
        currentHealth();
        anim = GetComponent<Animator>();
        invincibleCD = 0;
        cheat = gm.CHEAT;
    }

    public void Damage(int damageCount)
    {
        //伤害特效 
        //       FindObjectOfType<AudioManager>().Play("Player_Hit");
        //       anim.SetTrigger("Hit");
        if (damageCount < 0) {
            TargetHp -= damageCount;
            TargetHp = Mathf.Clamp(TargetHp, 0, HpMax);
            //currentHealth();

        }//回血特效 
        if ((damageCount > 0) && (invincibleCD > 0.7f))
        {
            if (!cheat)
            {
                StartCoroutine("Vibration");
                StartCoroutine("timeStop");
                TargetHp -= damageCount;
                anim.SetTrigger("Hit");
            }
            TargetHp = Mathf.Clamp(TargetHp, 0, HpMax);
            //currentHealth();
            invincibleCD = 0;
        }
     }

    public void DashInvincible()
    {
        invincibleCD = 0;
    }

    public void IncreaseMax()
    {
        //if (HpMax <= 9)
        HpMax += IncreaseHp;
        TargetHp += IncreaseHp+20;
        //currentHealth();
        gm.HpCapacity = HpMax;
    }
    IEnumerator timeStop()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
    }

    void currentHealth()
    {
        if (TargetHp <= 0||Hp<=0)
        {
            StartCoroutine("Die");
        }
        if (TargetHp < Hp)
        {
            Hp = Mathf.Lerp(Hp, TargetHp, Time.deltaTime * lerpSpeed);
        }
        else
        {
            Hp = Mathf.Lerp(Hp, TargetHp, Time.deltaTime * lerpSpeed2);
        }
        slider.value = (float)(Hp / HpMax);
    }

    //void currentHealth()
    //{
    //    if (Hp <= 0)
    //    {
    //        StartCoroutine("Die");
    //    }
    //    for (int j = 0; j < HeartCapacity; j++)
    //    {
    //        if (HpMax <= j)
    //        {
    //            HeartImages[j].enabled = false;
    //        }
    //        else
    //        {
    //            HeartImages[j].enabled = true;
    //        }
    //    }

    //    int i = 0;
    //    foreach (Image image in HeartImages)
    //    {
    //        i++;
    //        if (Hp >= i)
    //        {
    //            image.sprite = HeartSprites[1];
    //        }
    //        else
    //        {
    //            image.sprite = HeartSprites[0];
    //        }
    //    }

    //}

    IEnumerator Die()
    {
        GetComponent<PlayerController>().controllable = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Time.timeScale = 1;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1.5f);
        gm.Respawn();
    }

    private void Update()
    {
        HpMax = gm.MAXHP;
        currentHealth();
        textHp.text = Mathf.RoundToInt(Hp) + " / " + HpMax;
        textMoney.text = " "+Mathf.RoundToInt(gm.money);
        JumpTimer += Time.deltaTime;
        cheat = gm.CHEAT;

        if (Input.GetKeyDown(KeyCode.F11))
        {
            StartCoroutine("Die");
        }
        invincibleCD += Time.deltaTime;
    }

	IEnumerator Vibration()
	{
        try
        {
            XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, vibrationStrength, vibrationStrength);
            yield return new WaitForSeconds(vibrationTime);
            XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0, 0);
        }
        finally
        {
            XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0, 0);
        }
    }
}
