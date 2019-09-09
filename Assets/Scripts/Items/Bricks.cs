using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{//可破坏墙壁，timer后恢复
    public float timer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            timer = 0;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
        }

    }
}
