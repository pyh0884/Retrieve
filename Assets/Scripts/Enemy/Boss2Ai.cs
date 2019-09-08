using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class Boss2Ai : MonoBehaviour
{
	public Animator anim;
	private SpriteRenderer sr;
	public Boss2Shadow shadow;
	public Transform stop;
	public bool isAwake;
	[Header("技能CD")]
	public float pubCDTime;
	//	public List<bool> skillCD;
	//	public List<int> skillCDTime;
	[Header("默认速度")]
	public static float defaultSpeed = 7.0f;
	[Header("冲刺速度")]
	public float sprintSpeed;
	public float sprintSpeedFast;
	public float paraHalfTime;
	public float waitTime;

	[Header("技能表演出生点、释放点、终结点")]
	public List<Transform> skillSpawnPos;
	public List<Transform> skillActPos;
	public List<Transform> skillEndPos;

	[Header("地刺")]
	public List<Transform> stabPosList;
	public GameObject stabPrefab;
	public int stabCallNum1 = 3;
	public int stabCallNum2 = 5;
	public int stabGapFrame = 5;

	[Header("鬼火")]
	public GameObject missileSpawner;
	public GameObject missileSpawnerVice;
	public int phase1MissileAmount = 5;
	public int phase2MissileAmount = 7;
	public int viceMissileAmount = 3;

	[Header("抛物线运动：起始点与最低点")]
	public List<Transform> paraStartPosList;
	public List<Transform> paraBottomPosList;
	[Header("冲刺锚点")]
	public List<Transform> sprintPosList;

	public GameObject player;
	public Vector3 dist;
	public Collider2D skill3;


	List<IEnumerable<Instruction>> SkillList;
	Coroutines.Coroutine _Main;//协程根节点

	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());//根节点初始化
	}
	void Update()
	{
		_Main.Update();//根节点刷新
	}
	//主循环
	IEnumerable<Instruction> Main()
	{
		SkillList = new List<IEnumerable<Instruction>>();
		SkillList.Clear();
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		Transform Target = null;
		yield return ControlFlow.Call(FindPlayer(target => Target = target));
		if (Target != null)
		{
			var hpObject = GetComponent<BossHp>();
			yield return ControlFlow.ExecuteWhile(
				() => hpObject.Hp / hpObject.HpMax > 1 / 2,
				Phase(Target));
			yield return ControlFlow.Call(Change());
			yield return ControlFlow.ExecuteWhile(
				() => hpObject.Hp > 0,
				Phase(Target, true));
			yield return ControlFlow.Call(BeforeDie());
		}
	}

	//阶段主循环
	IEnumerable<Instruction> Phase(Transform target, bool aftermath = false)
	{
		try
		{
			if (!aftermath)
			{
				SkillList.Add(Sprint(target, sprintSpeed));//0
				SkillList.Add(GhostFire(target, phase1MissileAmount));//1
				SkillList.Add(Stab(target, stabCallNum1));//2
			}
			else
			{
				SkillList.Add(Sprint(target, sprintSpeedFast));//0
				SkillList.Add(GhostFire(target, phase2MissileAmount));//1
				SkillList.Add(Stab(target, stabCallNum2));//2
				SkillList.Add(Sprint(target,sprintSpeedFast ,aftermath));//3
				SkillList.Add(GhostFire(target, viceMissileAmount, aftermath));//4
				SkillList.Add(Stab(target, stabGapFrame, aftermath));//5
			}
			int curr = SkillSelect(SkillList.Count);
			while (true)
			{
				transform.position = skillSpawnPos[curr % (SkillList.Count / 2)].position;
				yield return ControlFlow.ExecuteWhileRunning(
					MoveTo(skillActPos[curr % (SkillList.Count / 2)].position),
					TrackTarget(target, sr.flipX, isright => sr.flipX = isright)
					);
				yield return ControlFlow.Call(SkillList[curr]);
				yield return ControlFlow.ExecuteWhileRunning(
					Restore(waitTime, stop.position),
					TrackTarget(target, sr.flipX, isright => sr.flipX = isright)
					);
				curr = SkillSelect(SkillList.Count, curr);
			}
		}
		finally
		{
			SkillList.Clear();
		}
	}

	//过渡状态
	IEnumerable<Instruction> Change()
	{
		yield break;
	}
	IEnumerable<Instruction> BeforeDie()
	{
		yield break;
	}

	//技能容器
	IEnumerable<Instruction> Sprint(Transform target,float speedOrTime, bool aftermath = false)
	{
		try
		{
			skill3.enabled = true;
			anim.SetBool("Skill3", true);
			if (!aftermath)
			{
				//一阶段技能表现
				for (int i = 0; i < paraStartPosList.Count; i++) {
					yield return ControlFlow.ExecuteWhileRunning(
						MoveTo(paraStartPosList[i].position, true),
						TrackTarget(target,sr.flipX,isright=>sr.flipX=isright)
						);
					if (paraBottomPosList[i].position.x > paraStartPosList[i].position.x) sr.flipX = true;
					else sr.flipX = false;
					yield return ControlFlow.Call(Para(paraBottomPosList[i], speedOrTime));
				}
			}
			else
			{
				//二阶段技能表现
				for (int i = 0; i < sprintPosList.Count; i++) {
					if (sprintPosList[i].position.x > transform.position.x) sr.flipX = true;
					else sr.flipX = false;
					yield return ControlFlow.Call(MoveTo(sprintPosList[i].position, speedOrTime));
				}
			}
			yield break;
		}
		finally {
			skill3.enabled = false;
		}
	}
	IEnumerable<Instruction> GhostFire(Transform target, int num, bool aftermath = false)
	{
		if (!aftermath)
		{
			var sp = Instantiate(missileSpawner, transform.position, new Quaternion());
			sp.GetComponent<MissilesSpawn>().numOfMissle = num;
			yield return null;
			sp.GetComponent<MissilesSpawn>().isOn = true;
		}
		else
		{
			for (int i = 0; i < num; i++)
			{
				Instantiate(missileSpawnerVice, transform.position, Quaternion.Euler(360 * i / num, 0, 0));
				yield return Utils.WaitForSeconds(pubCDTime / num);
			}
		}
	}
	IEnumerable<Instruction> Stab(Transform target, int num, bool aftermath = false)
	{
		try
		{
			anim.SetBool("Skill1", true);
			if (!aftermath)
			{
				int[] stabNumList = isSelected(stabPosList.Count, num);
				//使用地刺
				for (int i = 0; i < num; i++)
				{
					Instantiate(stabPrefab, stabPosList[stabNumList[i]].position, new Quaternion());
				}
				yield return Utils.WaitForSeconds(waitTime);
			}
			else
			{
				for (int i = 0; i < stabPosList.Count; i++)
				{
					Instantiate(stabPrefab, stabPosList[target.position.x > 0 ? (stabPosList.Count - i - 1) : i].position, new Quaternion());
					yield return Utils.WaitForFrames(num);
				}
			}
			yield break;
		}
		finally
		{
			anim.SetBool("Skill1", false);
		}
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
	IEnumerable<Instruction> Restore(float time, Vector3 end)
	{
		yield return ControlFlow.ConcurrentCall(
			WaitForSecondsCr(time),
			MoveTo(end));
		anim.SetTrigger("Rest");
	}
	IEnumerable<Instruction> WaitForSecondsCr(float seconds)
	{
		yield return Utils.WaitForSeconds(seconds);
	}
	IEnumerable<Instruction> TrackTarget(Transform target, bool right, System.Action<bool> isRight)
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
					//transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
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
	IEnumerable<Instruction> Para(Transform bottom, float halfTime) {
		float tempX = transform.position.x;
		float tempY = transform.position.y;
		float prob = (tempY - bottom.position.y) / ((tempX - bottom.position.x) * (tempX - bottom.position.x));
		float speed = (bottom.position.x - tempX) / halfTime;
		while (bottom.position.x < tempX ? transform.position.x >= (2 * bottom.position.x - tempX) : transform.position.x <= (2 * bottom.position.x - tempX))
		{
			transform.position += new Vector3(speed * Time.deltaTime, 0);
			float temp = transform.position.x - bottom.position.x;
			transform.position = new Vector3(transform.position.x, bottom.position.y + prob * temp * temp);
			yield return null;
		}
	}
	IEnumerable<Instruction> MoveTo(Vector3 target, Transform curr, float speed)
	{
		while (curr.position != target)
		{
			curr.position = Vector3.MoveTowards(curr.position, target, speed * Time.deltaTime);
			yield return null;
		}
	}
	IEnumerable<Instruction> MoveTo(Vector3 target, float speed)
	{
		while (transform.position != target)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			yield return null;
		}
	}
	IEnumerable<Instruction> MoveTo(Vector3 target, bool isSprint = false)
	{
		while (transform.position != target)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, (isSprint ? sprintSpeed : defaultSpeed) * Time.deltaTime);
			yield return null;
		}
	}
	IEnumerable<Instruction> MoveTo(Vector3 target, Transform curr, bool isSprint = false)
	{
		while (curr.position != target)
		{
			curr.position = Vector3.MoveTowards(curr.position, target, (isSprint ? sprintSpeed : defaultSpeed) * Time.deltaTime);
			yield return null;
		}
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