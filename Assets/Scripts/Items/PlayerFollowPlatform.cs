using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowPlatform : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerFoot")
        {
            player.transform.SetParent(gameObject.transform.parent);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerFoot")
        {
            player.transform.SetParent(gameObject.transform.parent);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerFoot")
        {
            player.transform.SetParent(null);
        }

    }
    void Update()
    {
        if (!player) player = GameObject.FindWithTag("Player");

    }
}
