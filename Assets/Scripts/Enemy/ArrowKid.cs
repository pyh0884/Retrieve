using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class ArrowKid : MonoBehaviour
{
	public float catchRange = 8.0f;
	public float attackRange = 3.0f;
	public float runAmt = 2.0f;
	public float moveSpeed = 2.0f;
    private float tempSpeed;
    public float _ProjectileInitialVelocity = 2.5f;
	public float shootGap = 1.5f;
	public Projectile _ProjectilePrefab;
	public Transform _ProjectileExit;
	public bool right = false;
	public bool gotHit = false;
	private Animator anim;
	private Rigidbody2D rb;
	Coroutines.Coroutine _Main;
	public GameObject GroundCheck1;
	public GameObject GroundCheck2;
	public GameObject WallCheck;
	public bool Grounded1;
	public bool Grounded2;
	public bool Walled1;
	public bool Walled2;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
    public float SlowTimer;
    private float timer;
    GameManager gm;
    bool slowed;
    AnimatorStateInfo animatorInfo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            moveSpeed =tempSpeed/gm.SlowMultiplier;
            anim.speed = 1f/gm.SlowMultiplier;
            slowed = true;
            timer = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            moveSpeed = tempSpeed / gm.SlowMultiplier;
            anim.speed = 1f / gm.SlowMultiplier;
            slowed = true;
            timer = 0;
        }
    }
    private void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		right = transform.right.x > 0 ? true : false;
        gm = FindObjectOfType<GameManager>();
        tempSpeed = moveSpeed;
    }
	void Start()
	{
		_Main = new Coroutines.Coroutine(Main());
	}

    // Update is called once per frame
    public GameObject slowEFX;
    void Update()
	{   if (slowed)
            slowEFX.SetActive(true);
        else
            slowEFX.SetActive(false);
        _Main.Update();
        animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
        timer += Time.deltaTime;
        if (animatorInfo.IsName("Green1_Hit")||animatorInfo.IsName("Green1_Die"))
        {
            anim.speed = 1;
        }
        else if (slowed)
        {
            anim.speed = 1f / gm.SlowMultiplier;
        }
        if (timer > SlowTimer&&slowed)
        {
            moveSpeed = tempSpeed;
            anim.speed = 1;
            timer = 0;
            slowed = false;
        }
	}
    public void Des()
    {
        rb.velocity = Vector2.zero;
        Destroy(this);
    }
    IEnumerable<Instruction> Main()
	{
				
		while (true)
		{
			Transform target = null;
			yield return ControlFlow.ExecuteWhileRunning(
				FindTargetInRadius(catchRange, trgt => target = trgt),
				Idle()
				);
			if (target != null)
			{
				yield return ControlFlow.ExecuteWhile(
					() => Mathf.Abs(target.position.x - transform.position.x) > attackRange,
					ChaseTarget(target),
					TrackTarget(target, isright => right = isright)
					);
				yield return ControlFlow.ExecuteWhile(
					() => Mathf.Abs(target.position.x - transform.position.x) < catchRange&&!gotHit,
                    log(),
                    ArrowAttack(target),
					TrackTarget(target, isright => right = isright)
					);
				if (gotHit) gotHit = false;
			}
		}
	}

	IEnumerable<Instruction> FindTargetInRadius(float radius, System.Action<Transform> targetFound)
	{
		// For now there is only a single potential target, so...
		var playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null)
		{
			Vector3 dist = playerObj.transform.position - transform.position;
			while (dist.magnitude > radius)
			{
				yield return null;
				dist = playerObj.transform.position - transform.position;
			}

			// Got it!
			targetFound(playerObj.transform);
		}
		// Else maybe we should warn...
	}

	IEnumerable<Instruction> Idle() {
		while (true) {
			//Debug.Log("Idle");
			yield return null;
		}
	}
    IEnumerable<Instruction> log()
    {
        //Debug.Log(1);
        yield return null;
    }
    IEnumerable<Instruction> ChaseTarget(Transform target)
	{
		try
		{
			while (true)
			{
				yield return null;
				rb.velocity = new Vector2(right ? moveSpeed : -moveSpeed, 0);
				//Debug.Log("Chasing");
				if (isWall1 || !isGround1)yield break;
			}
		}
		finally
		{
			rb.velocity = Vector2.zero;
		}
	}

	IEnumerable<Instruction> TrackTarget(Transform target, System.Action<bool> isRight) {
		bool isright = right;
		Vector3 dist;
		try
		{
			while (true)
			{
				dist = target.position - transform.position;
				if (isright != dist.x > 0 ? true : false)
				{
					isright = dist.x > 0 ? true : false;
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

	IEnumerable<Instruction> ArrowAttack(Transform target)
	{
		while (true)
		{
			anim.SetTrigger("Attack");
			yield return Utils.WaitForSeconds(shootGap);
			Vector3 targetTrans = transform.position + new Vector3(transform.right.x < 0 ? runAmt : -runAmt, 0);
			while (transform.position != targetTrans)
			{
                if (isWall2 || !isGround2)  break; 
				transform.position = Vector3.MoveTowards(transform.position, targetTrans, moveSpeed * Time.deltaTime);
				yield return null;
			}
			yield return Utils.WaitForSeconds(shootGap);
		}
	}
	public void Shoot() {
		var proj = GameObject.Instantiate<Projectile>(_ProjectilePrefab);
		proj.transform.position = _ProjectileExit.position;
		proj.transform.right = transform.right;
		proj.InitialForce = transform.right * _ProjectileInitialVelocity;
	}

	bool isGround1 {
		get {
			Vector2 start = GroundCheck1.transform.position;
			Vector2 end = new Vector2(GroundCheck1.transform.position.x,GroundCheck1.transform.position.y-1);
			Debug.DrawLine(start, end, Color.magenta);
			Grounded1 = Physics2D.Linecast(start, end, groundLayer);
			return Grounded1;
		}
	}

	bool isGround2
	{
		get
		{
			Vector2 start = GroundCheck2.transform.position;
			Vector2 end = new Vector2(GroundCheck2.transform.position.x, GroundCheck2.transform.position.y - 1);
			Debug.DrawLine(start, end, Color.magenta);
			Grounded2 = Physics2D.Linecast(start, end, groundLayer);
			return Grounded2;
		}
	}

	bool isWall1
	{
		get
		{
			Vector2 start = WallCheck.transform.position;
			Vector2 end = new Vector2(WallCheck.transform.position.x + transform.right.x, WallCheck.transform.position.y);
			//Debug.DrawLine(start, end, Color.cyan);
			Walled1 = Physics2D.Linecast(start, end, wallLayer);
			return Walled1;
		}
	}
	bool isWall2
	{
		get
		{
			Vector2 start = WallCheck.transform.position;
			Vector2 end = new Vector2(WallCheck.transform.position.x - transform.right.x, WallCheck.transform.position.y);
			Debug.DrawLine(start, end, Color.cyan);
			Walled2 = Physics2D.Linecast(start, end, wallLayer);
			return Walled2;
		}
	}
}