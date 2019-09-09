﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class SKill2 : MonoBehaviour
{
	public int damageAmt = 5;
	public float maxRadius = 8.0f;
	public float rushSpeed = 25.0f;
	public float liveTime = 0.5f;
	public LayerMask enemyLayer;
	private bool hit = false;
	Coroutines.Coroutine _Main;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 30)));
            }
            else
            {
                collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 30)));
            }
        }
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
        }
    }

    IEnumerable<Instruction> Main() {
		bool gotFirst = false;
		bool finished = false;
		yield return ControlFlow.ExecuteWhile(
			() => !finished,
			Throw(transform.rotation.eulerAngles.y == 0),
			CheckFirst(success => { gotFirst = success; finished = success; }),
			TimeOut(liveTime, finish => finished = finish)
			);
		if (gotFirst) {
			List<GameObject> aims = new List<GameObject>();
			bool gotOthers = false;
			yield return ControlFlow.Call(Search(success => gotOthers = success, result => aims = result));
			if (gotOthers) {
				foreach (GameObject aim in aims) {
					yield return ControlFlow.Call(Attack(aim));
				}
			}
		}
		Destroy(gameObject);
	}

	IEnumerable<Instruction> Throw(bool right) {
		Vector3 delta = new Vector3((right ? rushSpeed : -rushSpeed) * Time.deltaTime, 0);
		while (true)
		{
			transform.position += delta;
			yield return null;
		}
	}

	IEnumerable<Instruction> TimeOut(float waitTime,System.Action<bool> finish) {
		yield return Utils.WaitForSeconds(waitTime);
		finish(true);
	}

	IEnumerable<Instruction> CheckFirst(System.Action<bool>success) {
		//try
		//{
			while (true)
			{
				if (hit)
				{
					Debug.Log("yeah");
				//collidedWith.GetComponent<MonsterHp>().Damage(damageAmt);
				success(true);
					yield break;
				}
				else yield return null;
			}
		//}
	}

	IEnumerable<Instruction> Search(System.Action<bool> success,System.Action<List<GameObject>> result)
	{
		Collider2D[] candidates = Physics2D.OverlapCircleAll(transform.position, maxRadius, enemyLayer);
		List<GameObject> targets = new List<GameObject>();
		if (candidates.Length == 0)
		{
			success(false);
			yield break;
		}
		else {
			foreach (Collider2D candidate in candidates)
            targets.Add(candidate.gameObject);
			success(true);
			result(targets);
		}
	}

	IEnumerable<Instruction> Attack(GameObject target) {
		while (transform.position != target.transform.position) {
			transform.position = Vector3.MoveTowards(
				transform.position,
				target.transform.position,
				rushSpeed * Time.deltaTime
				);
			yield return null;
		}
		target.GetComponent<MonsterHp>().Damage(damageAmt);
	}
}