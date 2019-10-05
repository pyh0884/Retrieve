using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class SwordFish : MonoBehaviour
{
	public float catchRange = 5.0f;
	public float lostRange = 7.0f;
	public float rotateSpeedStrike = 180.0f;
	public float rotateSpeedMove = 90.0f;
	public float CD_Time_Strike = 3.0f;
	public float CD_Time_Idle = 2.0f;
	public float strikeSpeed = 20.0f;
	public float moveSpeed = 5.0f;
	public float extraDist = 5.0f;
	public float backDist = 2.0f;
	public float moveRadius = 5.0f;
	Coroutines.Coroutine _Main;
	Animator anim;
    public float SlowTimer;
    private float timer2;
    GameManager gm;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            //减速
        }
    }
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
		_Main = new Coroutines.Coroutine(Main());
        gm = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
		_Main.Update();
	}
    public void Des()
    {
        Destroy(this);
    }
    IEnumerable<Instruction> Main() {
		while (true) {
			Transform target = null;
			yield return ControlFlow.ExecuteWhileRunning(
				FindTargetInRadius(catchRange,trgt=>target=trgt),
				Idle()
				);
			if (target != null) {
				yield return ControlFlow.Call(Strike(target));
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

	IEnumerable<Instruction> Idle() {
		while (true)
		{
			Vector2 randVector = Random.insideUnitCircle.normalized;
			Vector3 startPos = transform.position;
			Vector3 moveTarget = new Vector3(randVector.x, randVector.y) * moveRadius + transform.position;
			yield return ControlFlow.Call(RotateFor(randVector, rotateSpeedMove));
			while (transform.position != moveTarget)
			{
				transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
				yield return null;
			}
			yield return Utils.WaitForSeconds(CD_Time_Idle);
			yield return ControlFlow.Call(RotateFor(-randVector, rotateSpeedMove));
			while (transform.position != startPos) {
				transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
				yield return null;
			}
			yield return Utils.WaitForSeconds(CD_Time_Idle);
		}
	}

	IEnumerable<Instruction> Strike(Transform target) {
		Vector3 dist,strikeTarget;
		while (true)
		{
			dist = target.position - transform.position;
			strikeTarget = target.position + extraDist * dist.normalized;
			yield return ControlFlow.ExecuteWhileRunning(WaitForSecondsCr(0.5f),RotateFor(dist, rotateSpeedStrike));
			yield return ControlFlow.Call(BackShake(backDist));
			anim.SetTrigger("Attack");
			while (transform.position != strikeTarget)
			{
				transform.position = Vector3.MoveTowards(transform.position, strikeTarget, strikeSpeed * Time.deltaTime);
                //Debug.Log("Striking");
				yield return null;
			}
			yield return Utils.WaitForSeconds(CD_Time_Strike);
		}
	}

	IEnumerable<Instruction> RotateFor(Vector3 dist, float rotationSpeed)
	{
		//Debug.Log("旋转开始");
		Quaternion targetRot = Quaternion.LookRotation(dist, Vector3.forward);
		if (dist.x < 0) targetRot.eulerAngles = new Vector3(0, 0, targetRot.eulerAngles.x);
		else targetRot.eulerAngles = new Vector3(0, 0, 180f - targetRot.eulerAngles.x);
		while (transform.rotation != targetRot)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
			yield return null;
		}
		//Debug.Log("旋转结束");
	}

	IEnumerable<Instruction> WaitForSecondsCr(float seconds)
	{
		yield return Utils.WaitForSeconds(seconds);
	}

	IEnumerable<Instruction> BackShake(float length) {
		Vector3 target = transform.position + transform.right * length;
		while (transform.position != target) {
			transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
			yield return null;
		}
	}
}