﻿using System.Collections;
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
	public float moveRadius = 5.0f;
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
			Vector3 moveTarget = new Vector3(randVector.x, randVector.y) * moveRadius + transform.position;
			yield return ControlFlow.Call(RotateFor(randVector, rotateSpeedMove));
			while (transform.position != moveTarget)
			{
				transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
				yield return null;
			}
			yield return Utils.WaitForSeconds(CD_Time_Idle);
		}
	}

	IEnumerable<Instruction> Strike(Transform target) {
		Vector3 dist,strikeTarget,currentVector;
		while (true)
		{
			dist = target.position - transform.position;
			strikeTarget = target.position + extraDist * dist.normalized;
			currentVector = transform.right;
			yield return ControlFlow.Call(RotateFor(dist, rotateSpeedStrike));
			while (transform.position != strikeTarget)
			{
				transform.position = Vector3.MoveTowards(transform.position, strikeTarget, strikeSpeed * Time.deltaTime);
				yield return null;
			}
			yield return Utils.WaitForSeconds(CD_Time_Strike);
		}
	}

	IEnumerable<Instruction> RotateFor(Vector3 dist, float rotationSpeed)
	{
		Quaternion targetRot = Quaternion.LookRotation(dist, Vector3.forward);
		if (dist.x < 0) targetRot.eulerAngles = new Vector3(0, 0, targetRot.eulerAngles.x);
		else targetRot.eulerAngles = new Vector3(0, 0, 180f - targetRot.eulerAngles.x);
		while (transform.rotation != targetRot)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
			yield return null;
		}
	}
}