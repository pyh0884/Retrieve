using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int Heal = 20;
    public GameObject main;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<HealthBarControl>().Damage(-Heal);
            Destroy(main);
        }
    }
}
