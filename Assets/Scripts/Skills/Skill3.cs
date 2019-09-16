using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : MonoBehaviour
{
    public GameManager gm;
    Animator anim;
    public PlayerController pc;
    float timer=0;
    public float LastTime;
    public float dmgMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        pc = GetComponentInParent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 15) * dmgMultiplier) * 2, 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 15) * dmgMultiplier));
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 15) * dmgMultiplier) * 2, 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 15) * dmgMultiplier));
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
