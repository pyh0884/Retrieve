using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseATK : TresureChest
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            GameObject.FindObjectOfType<GameManager>().increaseATK(1);
			SaveOpenState();
            Destroy(gameObject);
        }
    }
}
