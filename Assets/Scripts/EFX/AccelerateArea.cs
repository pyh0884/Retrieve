using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateArea : MonoBehaviour
{
    GameManager gm;
    int level;
    PlayerController pc;
    //public GameObject player;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pc = collision.GetComponent<PlayerController>();
            collision.GetComponent<Animator>().speed = 1.3f;
                pc.HorizontalSpeed =gm.HorizontalSpeed+1+level*0.75f;
                pc.jumpSpeed =gm.JumpSpeed+1+level*0.75f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Animator>().speed =1;
            pc.HorizontalSpeed = gm.HorizontalSpeed;
            pc.jumpSpeed =gm.JumpSpeed;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType <GameManager> ();
        level = gm.levels[1];
        //pc = FindObjectOfType<PlayerController>();
    }

}
