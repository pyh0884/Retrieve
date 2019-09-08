using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class Slime : MonoBehaviour
{
	//需要给prefab里设置GroundCheck、WallCheck点，坐标参考地刺怪
	public float catchXRange = 7.0f;
	public float catchYRange = 2.0f;
	public float attackRange = 2.0f;
	public float CD_Time = 2.0f;
	public float moveSpeed = 3.0f;
	public float chaseSpeed = 5.0f;

	public GameObject GroundCheck;
	public GameObject WallCheck;
	public bool Grounded;
	public bool Walled;
	public LayerMask groundLayer;
	public LayerMask wallLayer;

	public bool attackOver;

	private Rigidbody2D rb;
	private bool right = true;

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
		Transform target = null;
		yield return ControlFlow.ExecuteWhileRunning(
			FindTargetInRectangle(catchXRange,catchYRange, trgt => target = trgt),
			Idle(isright => right = isright)
			);
		while (true)
		{
			if (target != null)
			{
				yield return ControlFlow.ExecuteWhile(
					() => Vector3.Distance(target.position, transform.position) > attackRange,
					TrackTarget(target, isright=>right=isright)
					);
				yield return ControlFlow.Call(Attack());
				yield return Utils.WaitForSeconds(CD_Time);
			}
		}
	}

	IEnumerable<Instruction> FindTargetInRectangle(float xRange,float yRange, System.Action<Transform> targetFound)
	{
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null)
		{
			while (Mathf.Abs((playerObj.transform.position-transform.position).x) >xRange||Mathf.Abs((playerObj.transform.position - transform.position).y)>yRange)
			{
				yield return null;
			}
			targetFound(playerObj.transform);
		}
	}

	IEnumerable<Instruction> Idle(System.Action<bool> isRight)
	{
		bool isright = right;
		rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
		transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
		try
		{
			while (true)
			{
				if (!isGround || isWall)
				{
					isright = !isright;
					rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
					transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
					isRight(isright);
				}
				yield return null;
			}
		}
		finally
		{
			isRight(isright);
		}
	}

	IEnumerable<Instruction> TrackTarget(Transform target, System.Action<bool> isRight)
	{
		bool isright = right;
		Vector3 dist;
		try
		{
			while (true)
			{
				dist = target.position - transform.position;
				if (isright != dist.x > 0 ? true : false)
				{
					isright = dist.x > 0 ? true : false;
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

	IEnumerable<Instruction> Attack() {
		attackOver = false;
		//播放动画,动画结束时将attackOver置true;
		while (!attackOver) yield return null;
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
			Vector2 end = new Vector2(WallCheck.transform.position.x + (right ? 2 : -2), WallCheck.transform.position.y);
			Debug.DrawLine(start, end, Color.red);
			Walled = Physics2D.Linecast(start, end, wallLayer);
			return Walled;
		}
	}
}
