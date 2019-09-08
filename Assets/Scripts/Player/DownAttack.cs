using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttack : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Enemy"||collision.tag=="EnemyAttack")
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.GetComponent<PlayerController>().jumpSpeed*0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
