using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    public HealthBarControl hbc;
    public float LastTime;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, LastTime);
        StartCoroutine("StopMoving");
    }
    IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(1);
        Destroy(rb);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hbc.invincibleCD = 0.3f;
        }
    }

    private void Awake()
    {
        hbc = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBarControl>();

    }
}
