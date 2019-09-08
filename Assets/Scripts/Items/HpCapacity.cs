using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpCapacity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            GameObject.FindWithTag("Player").GetComponent<HealthBarControl>().IncreaseMax();
            Destroy(gameObject);
        }
    }

}
