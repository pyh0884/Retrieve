using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateArea : MonoBehaviour
{
    GameManager gm;
    int level;
    PlayerController pc;
    Animator anim;
    public bool enter;
    public GameObject main;
    //public GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enter = false;
            anim.speed =1;
            pc.HorizontalSpeed = gm.HorizontalSpeed;
            pc.jumpSpeed =gm.JumpSpeed;
        }
    }
    IEnumerator Des()
    {
        yield return new WaitForSecondsRealtime(8);
        anim.speed = 1;
        pc.HorizontalSpeed = gm.HorizontalSpeed;
        pc.jumpSpeed = gm.JumpSpeed;
        Destroy(main);
    }
    IEnumerator Accl()
    {
        while (true)
        {
            if (enter)
            {
                anim.speed = 1.3f;
                pc.HorizontalSpeed = gm.HorizontalSpeed + 1 + level * 0.75f;
                pc.jumpSpeed = gm.JumpSpeed + 1 + level * 0.75f;
            }
            yield return null;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType <GameManager> ();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        level = gm.levels[1];
        StartCoroutine("Accl");
        StartCoroutine("Des");


        //pc = FindObjectOfType<PlayerController>();
    }

}
