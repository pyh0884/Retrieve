using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class FishPeddle : MonoBehaviour
{
	public float catchRange = 22.0f;
	public float resetRange = 0.3f;
	public float CD_Time_Strike = 3.0f;
	public float strikeSpeed = 20.0f;
	public float moveSpeed = 5.0f;
	public float extraDist = 5.0f;
	public float prepareRadius = 5.0f;
	public float gridSpeed = 2.5f;
	public Transform ViceLeft, ViceRight;
	public GameObject shadowPrefab;

	[Header("击退")]
	public float hitBackDist = 1;
	public float hitBackSpeed = 5;
	public int maxBackFrame = 10;

    public bool gothit = false;

	Coroutines.Coroutine _Main;

	// Start is called before the first frame update
	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());
	}

	// Update is called once per frame
	void Update()
	{
		_Main.Update();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PlayerAttack")) {
			gothit = true;
			StartCoroutine(GotHit(transform));
		}
	}

	IEnumerator GotHit(Transform curr)
	{
		Vector3 target;
		int frameCount = 0;
		target = curr.position + curr.right * hitBackDist;
		//if (right) target = transform.position + new Vector3(hitBackDist, 0, 0);
		//else target = transform.position + new Vector3(-hitBackDist, 0, 0);
		while (frameCount < maxBackFrame)
		{
			curr.position = Vector3.MoveTowards(curr.position, target, hitBackSpeed * Time.deltaTime);
			yield return null;
			frameCount++;
		}
	}

	IEnumerable<Instruction> Main()
	{
		while (true)
		{
			Transform target = null;
			yield return ControlFlow.Call(FindTargetInRadius(catchRange, trgt => target = trgt));
			if (target != null)
			{
                while (true)
                {
                    if (ViceLeft == null) ViceLeft = Instantiate(shadowPrefab, transform.position, Quaternion.identity,transform).transform;
                    if (ViceRight == null) ViceRight = Instantiate(shadowPrefab, transform.position, Quaternion.identity,transform).transform;
                    if (gothit)
                    {
                        gothit = false;
						break;
                    }
                    yield return ControlFlow.ExecuteWhile(() => !gothit, Cycle(target));
                }
			}
		}
	}

	IEnumerable<Instruction> Cycle(Transform target)
	{
		bool Detached = false;
		yield return ControlFlow.ExecuteWhile(
				() => !Detached,
				FocusOn(target, transform),
				Prepare(target, detached => Detached = detached)
				);
		bool needReGrid = false;
		yield return ControlFlow.ExecuteWhileRunning(
				GridLie(transform, ViceLeft, false),
				GridLie(transform, ViceRight, true),
				FocusOn(target, transform),
				FocusOn(target, ViceLeft),
				FocusOn(target, ViceRight)
				);
		needReGrid = false;
		yield return ControlFlow.ExecuteWhile(
			() => !needReGrid,
			Strike(target, transform, need => needReGrid = need),
			Strike(target, ViceLeft, need => needReGrid = need),
			Strike(target, ViceRight, need => needReGrid = need)
			);
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

	IEnumerable<Instruction> GridLie(Transform center,Transform vice,bool right) {
        //while ((vice.position - center.position).sqrMagnitude < prepareRadius * prepareRadius / 4.0f)
        //{
        //	vice.position += vice.up * Time.deltaTime * (right?gridSpeed:-gridSpeed);
        //	yield return null;
        //}
        Vector3 targetPos = center.position + center.up * (right ? prepareRadius : -prepareRadius);
        while (vice.position != targetPos) {
            vice.position = Vector3.MoveTowards(vice.position, targetPos, gridSpeed * Time.deltaTime);
            yield return null;
        }
	}

	IEnumerable<Instruction> Prepare(Transform target, System.Action<bool> detached)
	{
		Vector2 randVector = Random.insideUnitCircle.normalized;
		Vector3 moveTarget = new Vector3(randVector.x, Mathf.Abs(randVector.y)) * prepareRadius + transform.position;
		while (transform.position != moveTarget)
		{
			transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
			yield return null;
		}
		transform.DetachChildren();
		detached(true);
	}

	IEnumerable<Instruction> Strike(Transform target, Transform curr, System.Action<bool> need)
	{
        gothit = false;
        yield return ControlFlow.ExecuteWhile(() => !gothit, StrikeMove(target, curr));
        yield return ControlFlow.ExecuteWhileRunning(WaitForSecondsCr(CD_Time_Strike), FocusOn(target, curr));
        need(true);
	}
	IEnumerable<Instruction> WaitForSecondsCr(float seconds) {
		yield return Utils.WaitForSeconds(seconds);
	}

	IEnumerable<Instruction> FocusOn(Transform target,Transform curr)
	{
		while (true)
		{
			Vector3 distNorm = (target.transform.position - curr.position).normalized;
			curr.right = distNorm;
			yield return null;
		}
	}

    IEnumerable<Instruction> StrikeMove(Transform target, Transform curr) {
        Vector3 dist, strikeTarget;
        dist = target.position - curr.position;
        strikeTarget = target.position + extraDist * dist.normalized;
        while (curr.position != strikeTarget)
        {
            curr.position = Vector3.MoveTowards(curr.position, strikeTarget, strikeSpeed * Time.deltaTime);
            yield return null;
        }
        gothit = true;
    }
}