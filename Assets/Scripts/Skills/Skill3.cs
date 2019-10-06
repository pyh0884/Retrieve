using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : MonoBehaviour
{
    public GameManager gm;
    Animator anim;
    public PlayerController pc;
    float timer=0;
    private float LastTime;
    public float dmgMultiplier;
    public int DmgPerLevel = 10;
    public int HealPerLevel = 3;
    private HealthBarControl hbc;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        pc = GetComponentInParent<PlayerController>();
        LastTime = 8 + gm.levels[2] * 2;
        hbc = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBarControl>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            hbc.Damage(-HealPerLevel * (gm.levels[2] + 1));
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 5 + DmgPerLevel * gm.levels[0]) * dmgMultiplier * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 5 + DmgPerLevel * gm.levels[0]) * dmgMultiplier));
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 5 + DmgPerLevel * gm.levels[0]) * dmgMultiplier * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 5 + DmgPerLevel * gm.levels[0]) * dmgMultiplier));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > LastTime)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
        if (pc.controllable && !pc.grabed)
        {
            //if ((Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Fire1") == 1))
            if (Input.GetAxisRaw("Fire1") != 0 || Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("Attack");

            }

        }
    }
}
