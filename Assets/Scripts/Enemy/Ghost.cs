using Coroutines;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	public float catchRange = 8.0f;
	public float lostRange = 12.0f;
	public float attackDist = 1.0f;
	public float CD_Time = 1.5f;
	public bool appear = true;
	private bool right = true;
	private Vector3 dist;
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
			yield return ControlFlow.ExecuteWhileRunning(FindTargetInRadius(catchRange, trgt => target = trgt));
			if (target != null)
			{
				yield return ControlFlow.ExecuteWhile(
					() => Vector3.Distance(target.position, transform.position) < lostRange,
					Attack(target),
					TrackTarget(target, isright => right = isright)
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
				Debug.Log("Finding...");
				yield return null;
			}

			targetFound(playerObj.transform);
		}
	}

	IEnumerable<Instruction> Attack(Transform target) {

		try
		{
			while (true)
			{
				appear = true;
				//播放消失动画，播完appear=false
				while (appear)
				{
					Debug.Log("anim");
					yield return null;
				}
				transform.position = target.position + new Vector3(right ? -attackDist : attackDist, 0);
				appear = true;
				//播放出现动画、攻击动画、消失动画
				while (appear) yield return null;
				yield return Utils.WaitForSeconds(CD_Time);
			}
		}
		finally {
			//播放消失动画
		}
		
	}

	IEnumerable<Instruction> TrackTarget(Transform target, System.Action<bool> isRight)
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
}
