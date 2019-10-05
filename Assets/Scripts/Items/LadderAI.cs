using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderAI : MonoBehaviour
{
    //public Collider2D col;
    void Start()
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag=="Player"&& Input.GetAxisRaw("Vertical")<0)
        {
            //col.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //col.enabled = true;
            if (GameObject.FindWithTag("Player").GetComponent<Animator>() != null)
                GameObject.FindWithTag("Player").GetComponent<Animator>().SetBool("Climb", false);
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
