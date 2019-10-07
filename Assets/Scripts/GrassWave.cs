using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassWave : MonoBehaviour
{
    Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.x <= transform.position.x)
                anim.SetTrigger("Wave");
            else
                anim.SetTrigger("Left");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
