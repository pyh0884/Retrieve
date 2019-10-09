using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFade : MonoBehaviour
{
    private Collider2D col;
    //public float fadeSpeed=2;
    public float timer;
    private float Timer;
    public GameObject ps;
    //2Dguangxiao
    void Start()
    {
        col = GetComponent<Collider2D>();
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Pet")
    //    {
    //        //col.enabled = false;
    //        //GetComponent<SpriteRenderer>().enabled = false;
    //        //ps.SetActive(false);
    //        //Timer = 0;
    //        Destroy(gameObject);
    //    }
    //}
    private void Update()
    {
        //Timer += Time.deltaTime;
        //if (Timer >= timer)
        //{
        //    GetComponent<SpriteRenderer>().enabled = true;
        //    ps.SetActive(true);
            
        //    col.enabled = true;
        //}
    }
}
