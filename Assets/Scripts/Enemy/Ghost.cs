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
	public float yOffset;
	private bool right = true;
	private Vector3 dist;
	Animator anim;
	Coroutines.Coroutine _Main;
    public float SlowTimer;
    private float timer;
    GameManager gm; bool slowed;
    AnimatorStateInfo animatorInfo;
    Transform target = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer = 0;
        }
    }
    // Use this for initialization
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
        animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
        timer += Time.deltaTime;
        if (animatorInfo.IsName("Yellow3_Hit")|| animatorInfo.IsName("Yellow3_Die"))
        {
            anim.speed = 1;
        }
        else if (slowed)
        {
            anim.speed = 1f / gm.SlowMultiplier;
        }
        if (timer > SlowTimer && slowed)
        {
            anim.speed = 1;
            timer = 0;
            slowed = false;
        }

    }
    public void Des()
    {
        Destroy(this);
    }
    IEnumerable<Instruction> Main()
	{	
		while (true) {
			target = null;
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
				//Debug.Log("Finding...");
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
				//while (appear)
				//{
				//	Debug.Log("anim");
				//	yield return null;
				//}
				//transform.position = target.position + new Vector3(right ? -attackDist : attackDist, yOffset);
				anim.SetBool("Attack",true);
				while (appear) yield return null;
                //yield return Utils.WaitForSeconds(CD_Time);
                //yield return null;
			}
		}
		finally {
            anim.SetBool("Attack", false);
		}
		
	}
    public void Tp()
    {
        transform.position = target.position + new Vector3(right ? -attackDist : attackDist, yOffset);
    }
    IEnumerable<Instruction> TrackTarget(Transform target, System.Action<bool> isRight)
	{
		bool isright = right;
		try
		{
			while (true)
			{
				dist = target.position - transform.position;
				if (isright != dist.x < 0 ? true : false)
				{
					isright = dist.x < 0 ? true : false;
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
