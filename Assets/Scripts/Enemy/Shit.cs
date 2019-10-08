using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : MonoBehaviour
{
	public GameObject missile;
	public Transform spawn;
    public float AttackRange = 10;
	private GameObject player;
	private Vector3 dist;
    private Animator anim;
    public float CDTime;
    private float timer=0;
    public float SlowTimer;
    private float timer2;
    GameManager gm;
    public bool slowed;
    AnimatorStateInfo animatorInfo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer2 = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer2 = 0;
        }
    }
    // Update is called once per frame
    public GameObject slowEFX;
    void Update()
    {
        if (slowed)
            slowEFX.SetActive(true);
        else
            slowEFX.SetActive(false);
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!player) player = GameObject.FindWithTag("Player");
        else
        {
            dist = player.transform.position - transform.position;
        }
        if (dist.magnitude < AttackRange && timer>CDTime)
        {
            anim.SetTrigger("Attack");
            timer = 0;
		}
        if (dist.x > 0) transform.eulerAngles = new Vector3(0, 180, 0);
		else transform.eulerAngles = Vector3.zero;
        if (animatorInfo.IsName("Yellow2_Hit")|| animatorInfo.IsName("Yellow2_Die"))
        {
            anim.speed = 1;
        }
        else if (slowed)
        {
            anim.speed = 1f / gm.SlowMultiplier;
        }
        if (timer2 > SlowTimer && slowed)
        {
            anim.speed = 1;
            timer2 = 0;
            slowed = false;
        }

    }
    private void Start()
    {
		anim = GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();

    }
    public void Shoot() {
        Instantiate(missile, spawn.position, transform.rotation);
	}
}
