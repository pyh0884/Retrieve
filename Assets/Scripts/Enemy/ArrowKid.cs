using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class ArrowKid : MonoBehaviour
{


	public float catchRange = 8.0f;
	public float attackRange = 3.0f;
	public float runAmt = 2.0f;
	public float moveSpeed = 2.0f;
	public float _ProjectileInitialVelocity = 2.5f;
	public float shootGap = 1.5f;
	public Projectile _ProjectilePrefab;
	public Transform _ProjectileExit;

	private bool right = false;
	private Rigidbody2D rb;
	private Vector3 dist;

	Coroutines.Coroutine _Main;
	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());
	}

	// Update is called once per frame
	void Update()
	{
		_Main.Update();
	}

	IEnumerable<Instruction> Main() {
		rb = GetComponent<Rigidbody2D>();
		while (true)
		{
			Transform target = null;
			yield return ControlFlow.ExecuteWhileRunning(
				FindTargetInRadius(catchRange, trgt => target = trgt),
				Idle()
				);
			if (target != null)
			{
				yield return ControlFlow.ExecuteWhile(
					() => Mathf.Abs(dist.x) > attackRange,
					ChaseTarget(target, isright=>right=isright)
					);
				yield return ControlFlow.ExecuteWhile(
					() => Mathf.Abs(dist.x) < catchRange,
					ArrowAttack(target),
					TrackTarget(target, isright => right = isright)
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
			dist = playerObj.transform.position - transform.position;
			while (dist.magnitude > radius)
			{
				yield return null;
				dist = playerObj.transform.position - transform.position;
			}

			// Got it!
			targetFound(playerObj.transform);
		}
		// Else maybe we should warn...
	}

	IEnumerable<Instruction> Idle() {
		yield break;
	}

	IEnumerable<Instruction> ChaseTarget(Transform target, System.Action<bool> isRight)
	{
		bool isright = right;
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
		finally
		{
			rb.velocity = Vector2.zero;
			isRight(isright);
		}
	}

	IEnumerable<Instruction> TrackTarget(Transform target, System.Action<bool> isRight) {
		bool isright = right;
		try
		{
			while (true)
			{
				dist = target.position - transform.position;
				if (isright != dist.x > 0 ? true : false)
				{
					isright = dist.x > 0 ? true : false;
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

	IEnumerable<Instruction> ArrowAttack(Transform target) {
		try
		{
			while (true)
			{
				var proj = GameObject.Instantiate<Projectile>(_ProjectilePrefab);
				proj.transform.position = _ProjectileExit.position;
				proj.transform.right = -transform.right;
				proj.InitialForce = -transform.right * _ProjectileInitialVelocity;
				yield return Utils.WaitForSeconds(shootGap);
				Vector3 targetTrans = transform.position + new Vector3(right ? runAmt : -runAmt, 0);
				while (transform.position != targetTrans)
				{
					transform.position = Vector3.MoveTowards(transform.position, targetTrans, moveSpeed * Time.deltaTime);
					yield return null;
				}
			}
		}
		finally {

		}
	}
}
