﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class StabFish : MonoBehaviour
{
	public float catchRange = 3.0f;
	public float lostRange = 5.0f;
	public float CD_Time = 2.5f;
	public bool dead = false;
	public bool attackOver = false;

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

	IEnumerable<Instruction> Main() {
		Transform target = null;
		try
		{
			while (!dead)
			{
				yield return ControlFlow.ExecuteWhileRunning(
					FindTargetInRadius(catchRange, trgt => target = trgt),
					Idle());
				if (target != null)
				{
					yield return ControlFlow.ExecuteWhile(
						() => Vector3.Distance(target.position, transform.position) < lostRange,
						Attack()
					);
				}
			}
			Destroy(this);
		}
		finally {
			//播放爆炸动画,生成遗物
			Destroy(this);
		}
	}

	IEnumerable<Instruction> Idle(){
		//暂时不动，播放idle动画
		while(true)yield return null;
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

	IEnumerable<Instruction> Attack() {
		while (true)
		{
			//播放攻击动画,结尾attackOver置true
			while (!attackOver) yield return null;
			yield return Utils.WaitForSeconds(CD_Time);
		}
	}
}
