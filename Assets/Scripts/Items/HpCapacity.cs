using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpCapacity : TresureChest
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack")|| collision.CompareTag("Pet"))
        {
            GameObject.FindWithTag("Player").GetComponent<HealthBarControl>().IncreaseMax();
			SaveOpenState();
			Destroy(gameObject);
        }
    }

}
