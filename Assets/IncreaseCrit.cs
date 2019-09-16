using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseCrit : TresureChest
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") || collision.CompareTag("Pet"))
        {
            GameObject.FindObjectOfType<GameManager>().increaseCrit(5);
            SaveOpenState();
            Destroy(gameObject);
        }
    }
}
