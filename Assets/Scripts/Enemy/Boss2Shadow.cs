using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Shadow : MonoBehaviour
{
	[Header("0:下;1:上;2:右;3:左")]
	public List<Transform> movePos;
    public Vector2 dist;
	public float farDist;
    public GameObject player;
	public GameObject boss2;
	[Header("0:初始速度;1-3:技能1-3速度")]
	public List<float> moveSpeed;
	public List<float> waitTime;
	public bool isActive;
    public Animator anim;
    void Start()
    {
		isActive = false;
        anim = GetComponent<Animator>();
    }

	IEnumerator Move1() {
		transform.position = movePos[2].position;
		while (transform.position != movePos[3].position)
		{
			transform.position = Vector3.MoveTowards(transform.position, movePos[3].position, moveSpeed[1] * Time.deltaTime);
			yield return 0;
		}
	}

	IEnumerator Move2() {
		transform.position = movePos[3].position;
		while (transform.position != movePos[0].position)
		{
			transform.position = Vector3.MoveTowards(transform.position, movePos[0].position, moveSpeed[2] * Time.deltaTime);
			yield return 0;
		}
		while (transform.position != movePos[1].position)
		{
			transform.position = Vector3.MoveTowards(transform.position, movePos[1].position, moveSpeed[0] * Time.deltaTime);
			yield return 0;
		}
	}

	IEnumerator Move3() {
		transform.position = movePos[2].position;
		while (transform.position != movePos[3].position)
		{
			transform.position = Vector3.MoveTowards(transform.position, movePos[3].position, moveSpeed[3] * Time.deltaTime);
			yield return 0;
		}
	}

    void Update()
    {
		if(!player) player = GameObject.FindWithTag("Player"); 
		dist = player.transform.position - transform.position;
		if (!isActive) {
			if (Mathf.Abs(dist.x) < farDist)
			{
                anim.SetBool("Active",true);
				isActive = true;
				transform.position = new Vector3(20, 20);
				boss2.GetComponent<Boss2Ai>().isAwake = true;
			}
		}
    }
}
