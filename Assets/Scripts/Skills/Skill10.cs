using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10 : MonoBehaviour
{
    public GameManager gm;
    public GameObject efx;
    public int DmgPerLevel = 10;

    public void playEFX()
    {
        Instantiate(efx, new Vector3(transform.position.x, transform.position.y + 0.3f), Quaternion.Euler(-90, 0, 0));
    }

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Destroy(gameObject, 1.5f);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 40 + gm.levels[3] * DmgPerLevel) * 1.4f), 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 40 + gm.levels[3] * DmgPerLevel)));

                collision.gameObject.GetComponent<BossHp>().Burn = 7;
                
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 40 + gm.levels[3] * DmgPerLevel) * 1.4f), 1);
                else
                collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 40 + gm.levels[3] * DmgPerLevel)));
                collision.gameObject.GetComponent<MonsterHp>().Burn = 7;

            }
        }
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
