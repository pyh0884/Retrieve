using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class stabStab : MonoBehaviour
{
	public GameObject stabPrefab;
	public float CDTime;
	public float catchRange = 3.0f;
	public float lostRange = 4.0f;
	public float moveSpeed = 3.0f;
    private float tempSpeed;
    public GameObject GroundCheck;
	public GameObject WallCheck;
	public bool Grounded;
	public bool Walled;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	private Rigidbody2D rb;
	private Animator anim;
	private bool right = true;
    private Transform target = null;

    //public bool gothit = false;

    //void Start()
    //{
    //	StartCoroutine(CDCount());
    //}

    //void Update()
    //{
    //	if (!player) player = GameObject.FindWithTag("Player");
    //	else dist = player.transform.position - transform.position;
    //       if (!CD&&player)
    //	{
    //		if (dist.magnitude < catchRange)
    //		{
    //			//动作
    //			stab = Instantiate(stabPrefab, player.transform.position-Vector3.up*dist.y, new Quaternion());
    //			StartCoroutine(CDCount());
    //		}
    //	}
    //}
    //IEnumerator CDCount()
    //{
    //	CD = true;
    //	yield return new WaitForSeconds(CDTime);
    //	CD = false;
    //}
    public float SlowTimer;
    private float timer2;
    GameManager gm;
    bool slowed;
    AnimatorStateInfo animatorInfo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            moveSpeed = tempSpeed / gm.SlowMultiplier;
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer2 = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            moveSpeed = tempSpeed / gm.SlowMultiplier;
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer2 = 0;
        }
    }


    public void Des()
    {
        rb.velocity = Vector2.zero;
        Destroy(this);
    }
    Coroutines.Coroutine _Main;

	void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		_Main = new Coroutines.Coroutine(Main());
        gm = FindObjectOfType<GameManager>();
        tempSpeed = moveSpeed;

    }

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
        if (animatorInfo.IsName("Green3_Hit")||animatorInfo.IsName("Green3_Die"))
        {
            anim.speed = 1;
        }
        else if (slowed)
        {
            anim.speed = 1f / gm.SlowMultiplier;
        }
        if (timer2 > SlowTimer && slowed)
        {
            moveSpeed = tempSpeed;
            anim.speed = 1;
            timer2 = 0;
            slowed = false;
        }

    }

    IEnumerable<Instruction> Main() {

		while (true) {
			Transform target = null;
			yield return ControlFlow.ExecuteWhileRunning(
				FindTargetInRadius(catchRange, trgt => target = trgt),
				Idle(isright=>right=isright)
				);
			if (target != null)
			{
				//gothit = false;
				yield return ControlFlow.ExecuteWhile(
					() => Vector3.Distance(target.position, transform.position) < lostRange,//&&!gothit,
					StabAttack(target),
					TrackTarget(target,right)
					);
			}
		}
	}

	IEnumerable<Instruction> FindTargetInRadius(float radius, System.Action<Transform> targetFound)
	{
		// For now there is only a single potential target, so...
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null)
		{
			while (Vector3.Distance(playerObj.transform.position, transform.position) > radius)
			{
				yield return null;
			}

			// Got it!
			targetFound(playerObj.transform);
		}
		// Else maybe we should warn...
	}

	IEnumerable<Instruction> Idle(System.Action<bool> isRight) {
		bool isright = right;
		rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
		transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
		try
		{
			while (true)
			{
				if (!isGround||isWall)
				{
					isright = !isright;
					rb.velocity = new Vector2(isright ? moveSpeed : -moveSpeed, 0);
					transform.eulerAngles = new Vector3(0, isright ? 0f : -180f, 0);
					isRight(isright);
				}
				yield return null;
			}
		}
		finally {
			rb.velocity = Vector2.zero;
			isRight(isright);
		}
	}
	IEnumerable<Instruction> StabAttack(Transform target) {
        //try
        //{
        this.target = target;
			while (true)
			{			
				anim.SetTrigger("Attack");
				//if (!gothit)
				//{

				//}
				yield return Utils.WaitForSeconds(CDTime);
			}
		//}
		//finally {
		//	gothit = false;
		//}
	}

    public void Attack()
    {
        var stab = GameObject.Instantiate(stabPrefab);
        stab.transform.position = new Vector3(target.position.x, transform.position.y);
    }

	IEnumerable<Instruction> TrackTarget(Transform target,bool right) {
		try {
			while (true) {
				Vector3 dist = target.position - transform.position;
				transform.eulerAngles = new Vector3( 0,dist.x > 0 ? 0 : -180);
				yield return null;
			}
		}
		finally { transform.eulerAngles = new Vector3(0, right ? 0 : -180); }
	}

	bool isGround
	{
		get
		{
			Vector2 start = GroundCheck.transform.position;
			Vector2 end = new Vector2(GroundCheck.transform.position.x, GroundCheck.transform.position.y - 1);
			Debug.DrawLine(start, end, Color.blue);
			Grounded = Physics2D.Linecast(start, end, groundLayer);
			return Grounded;
		}
	}

	bool isWall
	{
		get
		{
			Vector2 start = WallCheck.transform.position;
			Vector2 end = new Vector2(WallCheck.transform.position.x + (right? 2:-2), WallCheck.transform.position.y);
			Debug.DrawLine(start, end, Color.red);
			Walled = Physics2D.Linecast(start, end, wallLayer);
			return Walled;
		}
	}
}
