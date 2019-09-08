using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.5f);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 40)));
                collision.gameObject.GetComponent<BossHp>().Burn = 5;

            }
            else
            {
                collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 40)));
                collision.gameObject.GetComponent<MonsterHp>().Burn = 5;

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
