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
	public bool right = false;
	public bool gotHit = false;
	private Animator anim;
	private Rigidbody2D rb;
	Coroutines.Coroutine _Main;
	MonsterHitBack hb;

	private void Awake()
	{
		hb = GetComponent<MonsterHitBack>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		right = transform.right.x > 0 ? true : false;
	}
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
					() => Mathf.Abs(target.position.x - transform.position.x) > attackRange,
					ChaseTarget(target),
					TrackTarget(target, isright => right = isright)
					);
				yield return ControlFlow.ExecuteWhile(
					() => Mathf.Abs(target.position.x - transform.position.x) < catchRange&&!gotHit,
					ArrowAttack(target),
					TrackTarget(target, isright => right = isright)
					);
				if (gotHit) gotHit = false;
			}
		}
	}

	IEnumerable<Instruction> FindTargetInRadius(float radius, System.Action<Transform> targetFound)
	{
		// For now there is only a single potential target, so...
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null)
		{
			Vector3 dist = playerObj.transform.position - transform.position;
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
		while (true) {
			Debug.Log("Idle");
			yield return null;
		}
	}

	IEnumerable<Instruction> ChaseTarget(Transform target)
	{
		try
		{
			while (true)
			{
				yield return null;
				rb.velocity = new Vector2(right ? moveSpeed : -moveSpeed, 0);
				Debug.Log("Chasing");
			}
		}
		finally
		{
			rb.velocity = Vector2.zero;
		}
	}

	IEnumerable<Instruction> TrackTarget(Transform target, System.Action<bool> isRight) {
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

	IEnumerable<Instruction> ArrowAttack(Transform target)
	{
		while (true)
		{
			anim.SetTrigger("Attack");
			yield return Utils.WaitForSeconds(shootGap);
			Vector3 targetTrans = transform.position + new Vector3(transform.right.x < 0 ? runAmt : -runAmt, 0);
			while (transform.position != targetTrans&&!hb.isWall&&hb.isGround)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetTrans, moveSpeed * Time.deltaTime);
				yield return null;
			}
			yield return Utils.WaitForSeconds(shootGap);
		}
	}
	public void Shoot() {
		var proj = GameObject.Instantiate<Projectile>(_ProjectilePrefab);
		proj.transform.position = _ProjectileExit.position;
		proj.transform.right = transform.right;
		proj.InitialForce = transform.right * _ProjectileInitialVelocity;
	}


}
