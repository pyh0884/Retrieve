﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class SandWorm : MonoBehaviour
{

	public float catchRange = 5.0f;
	public float lostRange = 7.5f;
	public float moveSpeed = 3.0f;
	public GameObject GroundCheck;
	public GameObject WallCheck;
	public bool Grounded;
	public bool Walled;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	private Rigidbody2D rb;
	private bool right = true;
	public bool attackOver;
	Coroutines.Coroutine _Main;

	// Use this for initialization
	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());
	}

	// Update is called once per frame
	void Update()
	{
		_Main.Update();
	}

	IEnumerable<Instruction> Main()
	{
		while (true) {
			Transform target = null;
			yield return ControlFlow.ExecuteWhileRunning(
			FindTargetInRadius(catchRange, trgt => target = trgt),
			Idle(isright=>right=isright));
			if (target != null) {
				yield return ControlFlow.ExecuteWhile(
					()=>(Vector3.Distance(target.position,transform.position)<lostRange),
					ChaseAttack(target)
					);
			}
		}
	}

	IEnumerable<Instruction> FindTargetInRadius(float radius, System.Action<Transform> targetFound)
	{
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null)
		{
			while (Vector3.Distance(playerObj.transform.position, transform.position) > radius)
			{
				yield return null;
			}

			targetFound(playerObj.transform);
		}
	}

	IEnumerable<Instruction> Idle(System.Action<bool>isRight)
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
			rb.velocity = Vector2.zero;
			isRight(isright);
		}
	}

	IEnumerable<Instruction> ChaseAttack(Transform target) {
		try
		{
			Vector3 targetPos;
			while (true)
			{
				attackOver = false;
				targetPos = new Vector3(target.position.x, transform.position.y);
				while (transform.position != targetPos)
				{
					transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
					yield return null;
				}
				//播放动画，结束帧将attackOver置true
				while (!attackOver) yield return null;
			}
		}
		finally {
			attackOver = true;
		}
		
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
