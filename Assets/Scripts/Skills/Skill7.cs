using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill7 : MonoBehaviour
{
    public GameManager gm;
    public GameObject efx;
    public int DmgPerLevel = 5;

    public void playEFX()
    {
        Instantiate(efx, new Vector3(transform.position.x, transform.position.y+0.3f), Quaternion.Euler(-90, 0, 0));
    }
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Destroy(gameObject, 4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + DmgPerLevel * gm.levels[3]) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + DmgPerLevel * gm.levels[3])));
                collision.gameObject.GetComponent<BossHp>().Burn = 5;
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + DmgPerLevel * gm.levels[3]) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + DmgPerLevel * gm.levels[3])));
                collision.gameObject.GetComponent<MonsterHp>().Burn = 5;
            }
        }
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
        }

    }
    public GameObject AccArea;
    public void SpeedUp()
    {
        Instantiate(AccArea, new Vector3(Mathf.RoundToInt(transform.position.x / 4f) * 4f, transform.position.y), Quaternion.Euler(0, 0, 0));
    }
}
