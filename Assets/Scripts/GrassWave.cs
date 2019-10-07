using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassWave : MonoBehaviour
{
    Animator anim;
    public bool canHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canHit && collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.x <= transform.position.x)
                anim.SetTrigger("Wave");
            else
                anim.SetTrigger("Left");
        }
        else if (canHit && collision.gameObject.tag == "PlayerAttack")
        {
            anim.SetTrigger("Hit");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Des()
    {
        Destroy(gameObject);
    }
}
