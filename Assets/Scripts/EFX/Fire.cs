using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<BossHp>() != null)
                collision.gameObject.GetComponent<BossHp>().Burn = 2;
            else
                collision.gameObject.GetComponent<MonsterHp>().Burn = 2;
        }
    }
}
