using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    public HealthBarControl hbc;
    private float LastTime;
    public Rigidbody2D rb;
    float timer;
    public Animator anim;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StopMoving");
        gm = FindObjectOfType<GameManager>();
        LastTime = 5 + gm.levels[0] * 1.5f;

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
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= LastTime+1)
        {
            anim.SetTrigger("End");
        }
    }
}
