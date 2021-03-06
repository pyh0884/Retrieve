﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;
//章鱼Boss
public class Boss4Ai : MonoBehaviour
{
	public float CD_Time = 5;
	[Header("鱼叉")]
	public GameObject Fork1;
	public GameObject Fork3;
	public float forkLife = 15;
	public float forkXDist = 3;
	public float forkYDist = 2;
	[Header("落石")]
	public List<Transform> stoneSpawnPos;
	public GameObject stonePrefab;
	public int phase1Num = 5;
	public int Phase2Num = 7;
	[Header("喷墨")]
	public GameObject inkObject;
	public GameObject inkObject_Left;
	public GameObject inkObject_Naive;
	public GameObject inkObject_Naive_Left;
    private Animator anim;
    public RuntimeAnimatorController AngryAnim;

	GameObject st;

    List<IEnumerable<Instruction>> SkillList;
	Coroutines.Coroutine _Main;
    // Start is called before the first frame update
    void Start()
    {
		st = new GameObject();
		st.AddComponent<ShakeTest>();
		anim = GetComponent<Animator>();
		_Main = new Coroutines.Coroutine(Main());

	}

    // Update is called once per frame
    void Update()
    {
		_Main.Update();
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetInt("BlueBossBeaten", 1);
		Debug.Log(PlayerPrefs.GetInt("EXP", 0));
	}

	//主循环
	IEnumerable<Instruction> Main()
	{
		SkillList = new List<IEnumerable<Instruction>>();
		Transform Target = null;
		yield return ControlFlow.Call(FindPlayer(target => Target = target));
		if (Target != null)
		{
			var hpObject = GetComponent<BossHp>();
			Fork1.SetActive(true);
			yield return ControlFlow.ExecuteWhile(
				() => hpObject.Hp > hpObject.HpMax * 0.5,
				Phase(Target));
			yield return ControlFlow.Call(Change());
			yield return ControlFlow.ExecuteWhile(
				() => hpObject.Hp > 0,
				Phase(Target, true));
			yield return ControlFlow.Call(BeforeDie());
		}
	}

	//阶段主循环
	IEnumerable<Instruction> Phase(Transform target, bool aftermath = false) {
		try
		{
			SkillList.Clear();
			SkillList.Add(StoneFall(target, aftermath));
			SkillList.Add(Splash(target, aftermath));
			int curr = SkillSelect(SkillList.Count);
			while (true)
			{
				yield return ControlFlow.Call(SkillList[curr]);
				yield return Utils.WaitForSeconds(CD_Time);
				curr = SkillSelect(SkillList.Count, curr);
			}
		}
		finally {
			SkillList.Clear();
		}
	}

	//技能容器
	//IEnumerable<Instruction> Fork(Transform target,bool aftermath=false) {
	//	GameObject fork = aftermath ? Fork3 : Fork1;
	//	fork.SetActive(true);
	//	fork.transform.position = target.transform.position + new Vector3(target.position.x < transform.position.x ? forkXDist : -forkXDist, forkYDist);
	//	yield return Utils.WaitForSeconds(forkLife);
	//	Destroy(fork);
	//}
	IEnumerable<Instruction> StoneFall(Transform target,bool aftermath=false) {
        anim.SetTrigger("Attack1");
        yield return Utils.WaitForSeconds(1);
		int[] waitList = isSelected(stoneSpawnPos.Count, aftermath ? Phase2Num : phase1Num);
		st.GetComponent<ShakeTest>().StartVibration(0.2f, 0.3f, 0.4f);
		for (int i = 0; i < (aftermath ? Phase2Num : phase1Num); i++) {
			Instantiate(stonePrefab, stoneSpawnPos[waitList[i]]);
		}
		yield break;
	}
	IEnumerable<Instruction> Splash(Transform target,bool aftermath=false) {
        anim.SetTrigger("Attack2");
		bool left = target.position.x < transform.position.x;
		GameObject obj = aftermath ? (left ? inkObject_Left : inkObject) : (left ? inkObject_Naive_Left : inkObject_Naive);
	    obj.SetActive(true);
		yield return ControlFlow.ExecuteWhile(() => obj.activeSelf, Donothing());
		anim.SetTrigger("Stop");
	}
	
	//过渡状态
	IEnumerable<Instruction> Change()
    {
        anim.runtimeAnimatorController = AngryAnim as RuntimeAnimatorController;
		Fork3.SetActive(true);
		Fork3.transform.position = Fork1.transform.position;
		Destroy(Fork1);
        Debug.Log("血不到一半了");
		yield break;
	}
	IEnumerable<Instruction> BeforeDie()
	{
		PlayerPrefs.SetInt("BlueBossBeaten", 1);
		Destroy(Fork3);
		Debug.Log("我要死了");
		yield break;
	}

	//工具方法
	IEnumerable<Instruction> FindPlayer(System.Action<Transform> targetFound)
	{
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		while (!playerObj)
		{
			playerObj = GameObject.FindGameObjectWithTag("Player");
			yield return null;
		}
		targetFound(playerObj.transform);
	}
	IEnumerable<Instruction> Donothing() {
		while (true) yield return null;
	}

	int SkillSelect(int amount, int current = 0)
	{
		int temp = Random0ToN(amount);
		while (temp == current)
			temp = Random0ToN(amount);
		return temp;
	}
	public int Random0ToN(int n)
	{
		int temp = Mathf.FloorToInt(Random.value * n);
		while (temp == n) temp = Mathf.FloorToInt(Random.value * n);
		return temp;
	}
	public int[] isSelected(int n, int m)
	{//N选M
		int[] result = new int[m];
		bool[] flags = new bool[n];
		for (int i = 0; i < n; i++) flags[i] = false;
		for (int i = 0; i < m; i++)
		{
			int temp = Random0ToN(n);
			while (flags[temp] == true)
			{
				temp = Random0ToN(n);
			}
			flags[temp] = true;
			result[i] = temp;
		}
		return result;
	}
}
