using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    public int Hp = 5;
    public int HpMax = 5;
    public int HeartCapacity;
    public Image[] HeartImages;
    public Sprite[] HeartSprites;
    private Animator anim;
    public GameManager gm;
    public float invincibleCD;
    public bool cheat;
    public float JumpTimer;
	public float vibrationStrength = 1.5f;
	public float vibrationTime = 0.3f;
    void Awake()
    {
        gm= FindObjectOfType<GameManager>();
        Hp = gm.HP;
        HpMax = gm.MAXHP;
        HeartCapacity = 10;
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
            Hp -= damageCount;
            Hp = Mathf.Clamp(Hp, 0, HpMax);
            currentHealth();

        }//回血特效 
        if ((damageCount > 0) && (invincibleCD > 0.7f))
        {
			StartCoroutine("Vibration");
			StartCoroutine("timeStop");
            if (!cheat)
            {
                Hp -= damageCount;
                anim.SetTrigger("Hit");
            }
            Hp = Mathf.Clamp(Hp, 0, HpMax);
            currentHealth();
            invincibleCD = 0;
        }
     }

    public void DashInvincible()
    {
        invincibleCD = 0;
    }

    public void IncreaseMax()
    {
        if (HpMax <= 9) 
        HpMax += 1;
        Hp = HpMax;
        currentHealth();
        gm.HpCapacity = HpMax;
    }
    IEnumerator timeStop()
    {
        Time.timeScale = 0.15f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
    }
    void currentHealth()
    {
        if (Hp <= 0)
        {
            StartCoroutine("Die");
        }
        for (int j = 0; j < HeartCapacity; j++)
        {
            if (HpMax <= j)
            {
                HeartImages[j].enabled = false;
            }
            else
            {
                HeartImages[j].enabled = true;
            }
        }

        int i = 0;
        foreach (Image image in HeartImages)
        {
            i++;
            if (Hp >= i)
            {
                image.sprite = HeartSprites[1];
            }
            else
            {
                image.sprite = HeartSprites[0];
            }
        }

    }

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
