using System.Collections;
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

	Animator anim;
	MonsterHp hp;
	Coroutines.Coroutine _Main;
    public float SlowTimer;
    private float timer2;
    GameManager gm;
    bool slowed;
    AnimatorStateInfo animatorInfo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer2 = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer2 = 0;
        }
    }
    // Use this for initialization
    private void Awake()
	{
		hp = GetComponent<MonsterHp>();
		anim = GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();

    }
	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());
	}

    // Update is called once per frame
    public GameObject slowEFX;
    void Update()
    {
        if (slowed)
            slowEFX.SetActive(true);
        else
            slowEFX.SetActive(false);
        _Main.Update();
        animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
        timer2 += Time.deltaTime;
        if (timer2 > SlowTimer && slowed)
        {
            anim.speed = 1;
            timer2 = 0;
            slowed = false;
        }

    }

    IEnumerable<Instruction> Main() {
		
		try
		{
			yield return ControlFlow.ExecuteWhile(
				()=>!hp.dead,
				Cycle()
				);
			yield break;
		}
		finally {
			anim.SetTrigger("Die");
		}
	}

	IEnumerable<Instruction> Cycle() {
		Transform target = null;
		while (true)
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
	}

	IEnumerable<Instruction> Idle(){
		//暂时不动，播放idle动画
		while (true)
		{
			yield return null;
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

	IEnumerable<Instruction> Attack() {
		while (true)
		{
			//播放攻击动画,结尾attackOver置true
			while (!attackOver) yield return null;
			yield return Utils.WaitForSeconds(CD_Time);
		}
	}
}
