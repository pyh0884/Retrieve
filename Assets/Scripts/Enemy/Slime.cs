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
	Animator anim;
	private Rigidbody2D rb;
	private bool right = true;

	Coroutines.Coroutine _Main;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

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
		while (true)
		{
			Transform target = null;
			yield return ControlFlow.ExecuteWhileRunning(
				FindTargetInRectangle(catchXRange, catchYRange, trgt => target = trgt),
				Idle(isright => right = isright)
				);
			if (target != null)
			{
				yield return ControlFlow.ExecuteWhileRunning(
					WaitForSecondsCr(CD_Time),
					TrackTarget(target, isright=>right=isright)
					);
				attackOver = false;				
				yield return ControlFlow.ExecuteWhile(()=>!attackOver,Attack());
				attackOver = true;
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
				Debug.Log("finding");
				yield return null;
			}
			targetFound(playerObj.transform);
		}
	}

	IEnumerable<Instruction> Idle(System.Action<bool> isRight)
	{
		bool isright = right;	
		try
		{
			transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
			rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
			while (true)
			{
				if (!isGround || isWall)
				{
					isright = !isright;
					rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
					transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
					isRight(isright);
				}
				Debug.Log("Idle");
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
		rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
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
				Debug.Log("Tracking");
				yield return null;
			}
		}
		finally {
			rb.velocity = Vector2.zero;
			isRight(isright);
		}
	}

	IEnumerable<Instruction> Attack() {
		//播放动画,动画结束时将attackOver置true;
		anim.SetTrigger("Attack");
		while (!attackOver)
		{
			Debug.Log("Attacking");
			yield return null;
		}
	}

	IEnumerable<Instruction> WaitForSecondsCr(float seconds)
	{
		yield return Utils.WaitForSeconds(seconds);
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
