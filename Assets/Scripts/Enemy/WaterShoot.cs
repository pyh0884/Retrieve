using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class WaterShoot : MonoBehaviour
{
	//// Start is called before the first frame update
	//public GameObject stabPrefab;
	//public float CDTime;
	//public bool CD;
	//public float distance=1f;
	//private GameObject player;
	//private Vector3 dist;
	//private GameObject stab;
	//// Start is called before the first frame update
	//void Start()
	//{
	//	Moving = true;
	//	transform.position = startPos.position;
	//	StartCoroutine(Move(endPos.position));
	//}

	//// Update is called once per frame
	//void Update()
	//{
	//	if (!player) player = GameObject.FindWithTag("Player");
	//	else
	//	{
	//		dist = player.transform.position - transform.position;
	//	}
	//	if (!CD && player)
	//	{
	//		if (Mathf.Abs(dist.x) < 6)
	//		{
	//			//动作
	//			stab = Instantiate(stabPrefab, transform.position-new Vector3(0,distance), new Quaternion());
	//			StartCoroutine(CDCount());
	//			Destroy(stab, CDTime - 0.5f);
	//		}
	//	}
	//}

	public float waitTime = 1.0f;
	public float shootGap = 1.0f;
	public float moveSpeed = 3.0f;
	//public bool Moving;
	//public Transform startPos;
	public List<Transform> endPos;
	public Transform _ProjectileExit;
	public float _ProjectileInitialVelocity = 2.0f;
	public Projectile _ProjectilePrefab;
	//IEnumerator Move(Vector3 target)
	//{
	//	while (Moving)
	//	{
	//		yield return new WaitForSeconds(waitTime);
	//		Vector3 current = transform.position;
	//		while (transform.position != target)
	//		{
	//			transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
	//			yield return 0;
	//		}
	//		yield return new WaitForSeconds(waitTime);
	//		while (transform.position != current)
	//		{
	//			transform.position = Vector3.MoveTowards(transform.position, current, moveSpeed * Time.deltaTime);
	//			yield return 0;
	//		}
	//	}
	//}

	//   IEnumerator CDCount()
	//{
	//	CD = true;
	//	yield return new WaitForSeconds(CDTime);
	//	CD = false;
	//}

	public Transform Child;
	public float _LockOnRadius = 3.0f;
	public float _LockOffRadius = 4.0f;
	public float _RandomLookRotationSpeed = 90.0f;
	public float _MaxRotationSpeed = 90.0f;
	private int goalNum = 0;
	Coroutines.Coroutine _Main;
	Animator anim;
    public void Des()
    {
        
        Destroy(this);
    }
    // Use this for initialization
    void Start()
	{
		anim = GetComponent<Animator>();
		_Main = new Coroutines.Coroutine(Main());
	}

	// Update is called once per frame
	void Update()
	{
		_Main.Update();
	}

	IEnumerable<Instruction> Main()
	{
		float startZAngle = Child.localRotation.eulerAngles.z;
		while (true)
		{
			Transform target = null;

			yield return ControlFlow.ExecuteWhileRunning(
				FindTargetInRadius(_LockOnRadius, trgt => target = trgt),

				Idle(startZAngle ,num => goalNum = num));

			if (target != null)
			{
				yield return ControlFlow.ExecuteWhile(

					() => Vector3.Distance(target.position, transform.position) < _LockOffRadius,

					TrackTarget(target),

					FireProjectiles(),

					Idle(startZAngle, num => goalNum = num));
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

	IEnumerable<Instruction> TrackTarget(Transform target)
	{
		// While we track the player, Turn our tracking light on
		//_TrackingLight.enabled = true;
		try
		{
			while (true)
			{
				Vector3 dist = target.position - transform.position;
				Quaternion targetRot = Quaternion.LookRotation(dist, Vector3.forward);
				if (dist.x < 0) targetRot.eulerAngles = new Vector3(0, 0, targetRot.eulerAngles.x);
				if (dist.x > 0) targetRot.eulerAngles = new Vector3(0, 0, 180 - targetRot.eulerAngles.x);
				Child.localRotation = Quaternion.RotateTowards(Child.localRotation, targetRot, _MaxRotationSpeed * Time.deltaTime);
				yield return null;
			}
		}
		finally
		{
			// If we get interrupted (player left the tacking area), turn off the light
			//_TrackingLight.enabled = false;
		}
	}

	IEnumerable<Instruction> FireProjectiles()
	{
		while (true)
		{
			anim.SetTrigger("Attack");
			yield return Utils.WaitForFrames(12);
			var proj = GameObject.Instantiate<Projectile>(_ProjectilePrefab);
			proj.transform.position = _ProjectileExit.position;
			proj.transform.right = -Child.right;
			proj.InitialForce = - Child.right * _ProjectileInitialVelocity;

			yield return Utils.WaitForSeconds(shootGap);
		}
	}

	IEnumerable<Instruction> Idle(float defaultAngle,System.Action<int> goalCount)
	{
		// 将炮管旋转回初始位置
		//yield return ControlFlow.Call(RotateTo(defaultAngle, _RandomLookRotationSpeed));
		int num = goalNum;
		try
		{
			while (true) {
				yield return Utils.WaitForSeconds(waitTime);
				while (transform.position != endPos[num].position)
				{
					transform.position = Vector3.MoveTowards(transform.position, endPos[num].position, moveSpeed * Time.deltaTime);
					yield return null;
				}
				num = (num + 1) % endPos.Count;
			}
		}
		finally
		{
			goalCount(num);
		}
	}

	//IEnumerable<Instruction> RotateTo(float targetZAngle, float rotationSpeed)
	//{
	//	// Extract current euler angles
	//	float startZAngle = Child.localRotation.eulerAngles.z;

	//	// Wrap if necessary
	//	float deltaZAngle = targetZAngle - startZAngle;
	//	if (deltaZAngle < -180.0f)
	//	{
	//		deltaZAngle += 360.0f;
	//	}
	//	else if (deltaZAngle > 180.0f)
	//	{
	//		deltaZAngle -= 360.0f;
	//	}

	//	if (deltaZAngle > 0.0f)
	//	{
	//		float currentDelta = rotationSpeed * Time.deltaTime;
	//		while (currentDelta < deltaZAngle)
	//		{
	//			Child.localRotation = Quaternion.Euler(0.0f, 0.0f, startZAngle + currentDelta);
	//			yield return null;
	//			currentDelta += rotationSpeed * Time.deltaTime;
	//		}
	//		Child.localRotation = Quaternion.Euler(0.0f, 0.0f, targetZAngle);
	//	}
	//	else
	//	{
	//		float currentDelta = -rotationSpeed * Time.deltaTime;
	//		while (currentDelta > deltaZAngle)
	//		{
	//			Child.localRotation = Quaternion.Euler(0.0f, 0.0f, startZAngle + currentDelta);
	//			yield return null;
	//			currentDelta -= rotationSpeed * Time.deltaTime;
	//		}
	//		Child.localRotation = Quaternion.Euler(0.0f, 0.0f, targetZAngle);
	//	}
	//}
}