using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class stabStab : MonoBehaviour
{
	public GameObject stabPrefab;
	public float CDTime;
	public float catchRange = 3.0f;
	public float lostRange = 4.0f;
	public float moveSpeed = 3.0f;
	public GameObject GroundCheck;
	public GameObject WallCheck;
	public bool Grounded;
	public bool Walled;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private bool right = true;

	//void Start()
	//{
	//	StartCoroutine(CDCount());
	//}

	//void Update()
	//{
	//	if (!player) player = GameObject.FindWithTag("Player");
	//	else dist = player.transform.position - transform.position;
	//       if (!CD&&player)
	//	{
	//		if (dist.magnitude < catchRange)
	//		{
	//			//动作
	//			stab = Instantiate(stabPrefab, player.transform.position-Vector3.up*dist.y, new Quaternion());
	//			StartCoroutine(CDCount());
	//		}
	//	}
	//}
	//IEnumerator CDCount()
	//{
	//	CD = true;
	//	yield return new WaitForSeconds(CDTime);
	//	CD = false;
	//}

	Coroutines.Coroutine _Main;

	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());
	}

	// Update is called once per frame
	void Update()
	{
		// Just tick our root coroutine
		_Main.Update();
	}

	IEnumerable<Instruction> Main() {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		while (true) {
			Transform target = null;
			yield return ControlFlow.ExecuteWhileRunning(
				FindTargetInRadius(catchRange, trgt => target = trgt),
				Idle(isright=>right=isright)
				);
			if (target != null)
			{
				yield return ControlFlow.ExecuteWhile(
					() => Vector3.Distance(target.position, transform.position) < lostRange,
					StabAttack(target),
					TrackTarget(target,right)
					);
			}
		}
	}

	IEnumerable<Instruction> FindTargetInRadius(float radius, System.Action<Transform> targetFound)
	{
		// For now there is only a single potential target, so...
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null)
		{
			while (Vector3.Distance(playerObj.transform.position, transform.position) > radius)
			{
				yield return null;
			}

			// Got it!
			targetFound(playerObj.transform);
		}
		// Else maybe we should warn...
	}

	IEnumerable<Instruction> Idle(System.Action<bool> isRight) {
		bool isright = right;
		rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
		transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
		try
		{
			while (true)
			{
				if (!isGround||isWall)
				{
					isright = !isright;
					rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
					transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
					isRight(isright);
				}
				yield return null;
			}
		}
		finally {
			rb.velocity = Vector2.zero;
			isRight(isright);
		}
	}
	IEnumerable<Instruction> StabAttack(Transform target) {
		while (true)
		{
			var stab = GameObject.Instantiate(stabPrefab);
			stab.transform.position = new Vector3(target.position.x, transform.position.y);
			yield return Utils.WaitForSeconds(CDTime);
		}
	}

	IEnumerable<Instruction> TrackTarget(Transform target,bool right) {
		try {
			while (true) {
				Vector3 dist = target.position - transform.position;
				sr.flipX = dist.x > 0 ? right : !right;
				yield return null;
			}
		}
		finally { sr.flipX = true; }
	}

	bool isGround
	{
		get
		{
			Vector2 start = GroundCheck.transform.position;
			Vector2 end = new Vector2(GroundCheck.transform.position.x, GroundCheck.transform.position.y - 2);
			Debug.DrawLine(start, end, Color.blue);
			Grounded = Physics2D.Linecast(start, end, groundLayer);
			return Grounded;
		}
	}

	bool isWall
	{
		get
		{
			Vector2 start = WallCheck.transform.position;
			Vector2 end = new Vector2(WallCheck.transform.position.x + (right? 2:-2), WallCheck.transform.position.y);
			Debug.DrawLine(start, end, Color.red);
			Walled = Physics2D.Linecast(start, end, wallLayer);
			return Walled;
		}
	}
}
