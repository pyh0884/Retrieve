using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseATK : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            GameObject.FindObjectOfType<GameManager>().increaseATK(1);
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
