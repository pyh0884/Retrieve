using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : MonoBehaviour
{
	public GameObject missile;
	public Transform spawn;
	private GameObject player;
	private Vector3 dist;
    private Animator anim;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindWithTag("Player");
        else
        {
            dist = player.transform.position - transform.position;
        }
        if (dist.magnitude < 10)
        {
			anim.speed = 1;
		}
        else
        {
			anim.speed = 0;
		}
        if (dist.x > 0) transform.eulerAngles = new Vector3(0, 180, 0);
		else transform.eulerAngles = Vector3.zero;
    }
    private void Start()
    {
		anim = GetComponent<Animator>();
	}
    public void Shoot() {
		//动画里调用此方法
		Vector2 rand = Random.insideUnitCircle.normalized;
        if (dist.magnitude<10)
        Instantiate(missile, spawn.position, transform.rotation);
	}
}
