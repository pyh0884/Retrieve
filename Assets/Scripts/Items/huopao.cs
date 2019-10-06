using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huopao : MonoBehaviour
{
    public GameObject leftBullet;
    public GameObject rightBullet;
    public Transform trans;
    public bool TowardsRight;
    public float CDTime;
    float timer=100;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void shoot()
    {if (TowardsRight)
            Instantiate(rightBullet, trans.position, Quaternion.identity);
    else
            Instantiate(leftBullet, trans.position, Quaternion.identity);

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timer>CDTime&&collision.tag == "Player")
        {
            anim.SetTrigger("Attack");
            timer = 0;
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
}
