using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss0Ai : MonoBehaviour
{
	public float pubCDTime;
	public float nearDist;
	public float moveSpeed;
	public float playerCheckDistance;
	public LayerMask playerLayer;
	public Transform playerCheck;
	public Transform wormSpawn;
	public GameObject wormPrefab;

	private Animator anim;
	private Rigidbody2D enemyRigidbody;
	private Vector3 dist;
	public GameObject player;
	public bool isSkill;
	public bool pubCD;
	public bool HitPlayer;

	// Start is called before the first frame update
	void Awake()
    {
		anim = GetComponent<Animator>();
		enemyRigidbody = GetComponent<Rigidbody2D>();
		isSkill = false;
		pubCD = false;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
		if (!player) player = GameObject.FindWithTag("Player");
        else
        {
            dist = player.transform.position - transform.position;
        }
		if (dist.x > 0) transform.eulerAngles = new Vector3(0, -180);
		else transform.eulerAngles = Vector3.zero;
		if (!isSkill) {
			Move();
			if (!pubCD) {
				isSkill = true;
				enemyRigidbody.velocity = Vector2.zero;
				SkillUse();
			}
		}
	}

	bool isPlayer
	{
		get
		{
			Vector2 start = playerCheck.position;
			//Vector2 end = new Vector2(start.x + direction.x > 0 ? distance:-distance,start.y);
			Vector2 end;
			// = new Vector2(start.x + direction.x > 0 ? distance * 15: -distance * 15,start.y);
			if (transform.rotation.y ==0)
				end = new Vector2(start.x - playerCheckDistance, start.y);
			else
				end = new Vector2(start.x + playerCheckDistance, start.y);
			Debug.DrawLine(start, end, Color.blue);
			HitPlayer = Physics2D.Linecast(start, end, playerLayer);
			return HitPlayer;
		}
	}

	void SkillUse() {
		if (!isPlayer) {
			GameObject instance= Instantiate(wormPrefab, wormSpawn.position, Quaternion.Euler(0, transform.rotation.y * 180, 0));
            Destroy(instance, 4);
			//播放投掷动画/音效
		}
		else {
			//播放近战动画/音效
			anim.SetTrigger("Attack");
		}
		isSkill = false;
		StartCoroutine(PubCDCounter());
	}

	void Move() {
		if (Mathf.Abs(dist.x) > nearDist)
			enemyRigidbody.velocity = dist.x > 0 ? new Vector2(moveSpeed, 0) : new Vector2(-moveSpeed, 0);
		else enemyRigidbody.velocity = Vector2.zero;
	}

	IEnumerator PubCDCounter() {
		pubCD = true;
		yield return new WaitForSeconds(pubCDTime);
		pubCD = false;
	}
}
