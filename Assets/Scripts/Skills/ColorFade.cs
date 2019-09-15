using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFade : MonoBehaviour
{
    private Collider2D col;
    public Animator anim;
    //2Dguangxiao
    void Start()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pet")
        {
            col.enabled = false;
            anim.SetTrigger("Fade");
        }
    }
}
