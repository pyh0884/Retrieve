﻿using System.Collections;
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
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            //减速
        }
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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
