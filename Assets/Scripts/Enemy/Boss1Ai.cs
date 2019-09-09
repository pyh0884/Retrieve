using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class Boss1Ai : MonoBehaviour
{
    [Header("距离阈值")]
    public float nearDist;
	public float midDist;
	public float farDist;
	public float stopDist = 1.5f;
	public float stopstopDist = 0.5f;
	public GameObject player;
	public GameObject stonePrefab;
    [Header("投掷起始点")]
    public List<Transform> handTrans;
	public GameObject quakePrefab0;
	public GameObject quakePrefab1;
	[Header("跳跃参数")]
	public float jumpTime;
	public float acceleration;
	public float jumpSpeed;
	public float jumphighAmt;
	[Header("地震波起始点")]
    public List<Transform> footTrans;
	public float stoneWaitTime;
	public GameObject skill2Object;
	[Header("护盾子物体")]
	public GameObject shield;
	public float shieldLifeTime;
	[Header("墙壁检测")]
	public bool HitWall;
	public Transform wallCheck;
	public float wallCheckDistance;
	public LayerMask wallLayer;
    [Header("技能CD")]
    public float pubCDTime;
    [Header("Boss1速度")]
	public float speed;
	public float sprintSpeed;
	public float sprintDistance;


	private Rigidbody2D enemyRigidBody;
	private Vector3 dist;
    public Animator anim;
    public GameObject Boss1Echo;

	List<IEnumerable<Instruction>> SkillList;
	Coroutines.Coroutine _Main;
    void Awake()
    {
		enemyRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());
	}
	void Update()
    {
		//      if (!player) player = GameObject.FindWithTag("Player");
		//      else
		//      {
		//          dist = player.transform.position - transform.position;
		//      }
		//      if (!isAwake)
		//{
		//	if (Mathf.Abs(dist.x) < farDist) isAwake = true;
		//}
		//else
		//{
		//	if (!isSkill && !isSkillActing[1] && !isSkillActing[2] && !isSkillActing[0])
		//	{
		//		Move();
		//		if (!pubCD)
		//		{
		//			skillNum = SkillSelect();
		//			isSkill = true;
		//		}
		//	}
		//	else
		//	{
		//		if (!pubCD)
		//		{
		//			enemyRigidBody.velocity = Vector2.zero;
		//			SkillUse(skillNum);
		//		}
		//	}
		//}
		_Main.Update();
	}

	IEnumerable<Instruction> Main() {
		SkillList = new List<IEnumerable<Instruction>>();
		SkillList.Clear();
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
			SkillList.Add(StoneSpawn(target,aftermath));
			SkillList.Add(Rush(target,aftermath));
			SkillList.Add(JumpInParabola(target,aftermath));
			SkillList.Add(OpenShield(target, aftermath));
			//增加技能列表内的项目，有新技能就在这里加上

			float y = transform.position.y;
			int curr = SkillSelect(SkillList.Count);
			while (true) {
				yield return ControlFlow.ExecuteWhileRunning(
					WaitForSecondsCr(pubCDTime),
					Idle(target));
				yield return ControlFlow.Call(SkillList[curr]);
				curr = SkillSelect(SkillList.Count, curr);
				yield return ControlFlow.Call(MoveTo(new Vector3(transform.position.x, y)));
			}
		}
		finally {
			SkillList.Clear();
		}
	}

	//闲置状态
	IEnumerable<Instruction> Idle(Transform target)
	{
		bool right = transform.rotation.eulerAngles != Vector3.zero;
		try
		{
			while (true)
			{
				yield return ControlFlow.ExecuteWhile(
					() => Mathf.Abs(target.position.x - transform.position.x) > stopDist,
					Move(right, isright => right = isright)
					);
				yield return ControlFlow.ExecuteWhile(
					() => Mathf.Abs(target.position.x - transform.position.x) > stopstopDist,
					TrackTarget(target, right, isright => right = isright)
					);
				yield return null;
			}
		}
		finally
		{
			transform.eulerAngles = new Vector3(0, right ? 180 : 0);
		}
	}
	IEnumerable<Instruction> Move(bool isright, System.Action<bool> isRight)
	{
		{
			//if (dist.x > 0)
			//{
			//	transform.eulerAngles = -180 * Vector3.up;
			//}
			//else
			//{
			//	transform.eulerAngles = Vector3.zero;
			//}
			//if (!isWall)
			//{
			//	if (Mathf.Abs(dist.x) > 1.5f)
			//	{
			//		enemyRigidBody.velocity = dist.x > 0 ? new Vector2(speed, 0) : new Vector2(-speed, 0);
			//	}
			//	else enemyRigidBody.velocity = Vector2.zero;
			//}
			//else
			//{
			//	enemyRigidBody.velocity = Vector2.zero;
			//	//if (transform.eulerAngles.y < 0) transform.position += new Vector3(-wallCheckDistance, 0);
			//	//else transform.position += new Vector3(wallCheckDistance, 0);
			//}
		}
		enemyRigidBody.velocity = new Vector2(isright ? speed : -speed, 0);
		transform.eulerAngles = new Vector3(0, !isright ? 0f : -180f, 0);
		try
		{
			while (true)
			{
				if (isWall)
				{
					isright = !isright;
					enemyRigidBody.velocity = new Vector2(isright ? speed : -speed, 0);
					transform.eulerAngles = new Vector3(0, !isright ? 0f : -180f, 0);
					isRight(isright);
				}
				yield return null;
			}
		}
		finally
		{
			enemyRigidBody.velocity = Vector2.zero;
			isRight(isright);
		}
	}
	IEnumerable<Instruction> TrackTarget(Transform target, bool isright, System.Action<bool> isRight)
	{
		try
		{
			while (true)
			{
				Vector3 dist = target.position - transform.position;
				if (isright != dist.x > 0 ? true : false)
				{
					isright = dist.x > 0 ? true : false;
					transform.eulerAngles = new Vector3(0, !isright ? 0f : -180f, 0);
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
	IEnumerable<Instruction> StoneSpawn(Transform target,bool aftermath= false)
	{
		anim.SetTrigger("Skill1");
		for (int i = 0; i < handTrans.Count*(aftermath?2:1); i++)
		{
			Instantiate(stonePrefab, handTrans[i%handTrans.Count].position, new Quaternion());
			yield return Utils.WaitForSeconds(stoneWaitTime/(aftermath?2:1));
		}
	}
	IEnumerable<Instruction> Rush(Transform target,bool aftermath =false)
	{
		try
		{
			skill2Object.GetComponent<Collider2D>().enabled = true;
			anim.SetBool("Skill2", true);
			Vector3 targetPos = transform.position + new Vector3((target.position.x > transform.position.x ? sprintDistance : -sprintDistance)*(aftermath?1.5f:1), 0);
			yield return ControlFlow.ExecuteWhile(
				() => !(isWall || transform.position == targetPos),
				MoveTo(targetPos, sprintSpeed*(aftermath?1.5f:1)),
				CreateShadow()
				);
			anim.SetBool("Skill2", false);
			skill2Object.GetComponent<Collider2D>().enabled = false;
		}
		finally
		{
			anim.SetBool("Skill2", false);
			skill2Object.GetComponent<Collider2D>().enabled = false;
		}
	}
	IEnumerable<Instruction> JumpInParabola(Transform target,bool aftermath=false)
	{
		try
		{
			Vector3 targetPos = target.position;
			float temp = Time.fixedDeltaTime;
			float tempY = transform.position.y;
			float tempX = transform.position.x;
			float prob = jumphighAmt / ((targetPos.x - tempX) * (targetPos.x - tempX));
			skill2Object.GetComponent<Collider2D>().enabled = true;
			//enemyRigidBody.bodyType = RigidbodyType2D.Dynamic;
			//enemyRigidBody.gravityScale = acceleration;
			anim.SetBool("Skill3", true);
			int num = aftermath ? 3 : 1;//这行代码决定了两个阶段分别跳几次
			for(int i=0;i<num;i++)
			{
				enemyRigidBody.velocity = new Vector2((targetPos.x - tempX) / (jumpTime * num), 0);
				
				Time.fixedDeltaTime = Time.deltaTime;
				while (transform.right.x>0? (transform.position.x >= targetPos.x):(transform.position.x<=targetPos.x)) {
					transform.position = new Vector3(transform.position.x, tempY+jumphighAmt - prob * (targetPos.x - transform.position.x) * (targetPos.x - transform.position.x));
					yield return null;
				}
				FindObjectOfType<AudioManager>().Play("BossLand");
				Instantiate(quakePrefab0, footTrans[0], false);
				Instantiate(quakePrefab1, footTrans[1], false);
			}
			skill2Object.GetComponent<Collider2D>().enabled = false;
			//enemyRigidBody.bodyType = RigidbodyType2D.Kinematic;
			enemyRigidBody.velocity = Vector2.zero;
			Time.fixedDeltaTime = temp;
			anim.SetBool("Skill3", false);
		}
		finally
		{
			anim.SetBool("Skill3", false);
			skill2Object.GetComponent<Collider2D>().enabled = false;
			//enemyRigidBody.bodyType = RigidbodyType2D.Kinematic;
			enemyRigidBody.velocity = Vector2.zero;
		}
	}
	IEnumerable<Instruction> OpenShield(Transform target, bool aftermath=false) {
		try
		{
			shield.SetActive(true);
			yield return Utils.WaitForSeconds(shieldLifeTime*(aftermath?2:1));
			shield.SetActive(false);
		}
		finally {
			shield.SetActive(false);
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
	IEnumerable<Instruction> WaitForSecondsCr(float seconds)
	{
		yield return Utils.WaitForSeconds(seconds);
	}
	IEnumerable<Instruction> MoveTo(Vector3 target, bool isSprint = false)
	{
		while (transform.position != target)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, (isSprint ? sprintSpeed : speed) * Time.deltaTime);
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
	IEnumerable<Instruction> CreateShadow() {
		while (true)
		{
			GameObject instance1 = Instantiate(Boss1Echo, transform.position, Quaternion.identity);
			Destroy(instance1, 0.3f);
			yield return null;
		}
	}
	bool isWall
	{
		get
		{
			Vector2 start = wallCheck.position;
			//Vector2 end = new Vector2(start.x + direction.x > 0 ? distance:-distance,start.y);
			Vector2 end;
			// = new Vector2(start.x + direction.x > 0 ? distance * 15: -distance * 15,start.y);
			if(transform.rotation.y<0)
					end = new Vector2(start.x + wallCheckDistance, start.y);
			else
					end = new Vector2(start.x - wallCheckDistance, start.y);			
			Debug.DrawLine(start, end, Color.blue);
			HitWall = Physics2D.Linecast(start, end, wallLayer);
			return HitWall;
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

}
