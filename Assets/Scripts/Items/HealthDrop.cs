using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<HealthBarControl>().Damage(-1);
            Destroy(gameObject);
        }
    }
}
