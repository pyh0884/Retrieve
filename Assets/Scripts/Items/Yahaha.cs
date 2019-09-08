using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yahaha : MonoBehaviour
{
    public AudioSource ac;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            GameObject.FindWithTag("Player").GetComponent<HealthBarControl>().IncreaseMax();
            ac.Play();
        }
    }

}
