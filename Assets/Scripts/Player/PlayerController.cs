using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    #region variables
    public GameObject DashEcho;
    public GameObject DustEFX;
    public bool HighFall;
    public float highFallTimer;
    //public Transform dustPos;
    //private float echoTime;
    //public float StartEchoTime;
	Rigidbody2D playerRigidbody2D;
    public Collider2D AttackCollider;
    public Collider2D DownAttackCollider;
    Animator anim;
	private bool doubleJump = false;
	public float fallMultiplier = 1.4f;
	[Header("current speed on X axis")]
	public float speedX;
	[Header("current speed on Y axis")]
	public float speedY;
	[Header("current direction on X axis")]
	public float horizontalDirection;//between -1~1
    private float tempHorizontalDirection;
    public AudioManager am;
    private Scene scene;
	private GameObject st;

	/// <summary>
	/// dash movement variables
	/// </summary>
	[Header("Dash variables")]
	public bool airDashed = false;
	public bool isDashing = false;
    public bool isWallJump = false;
	public float DashTime;
	public float DashSpeed;

	[Header("distance from the ground")]
	[Range(0, 0.5f)]
	public float distance;
	[Header("start point of the ray")]
	public Transform groundCheck;
    public Transform groundCheck2;

    [Header("ground mask")]
	public LayerMask groundLayer;
	public bool grounded;

    [Header("start point of the ray")]
    public Transform WallCheck;
    public LayerMask WallLayer;
    public bool HitWall;
    public LayerMask BossLayer;
    public bool OnBoss;
    public GameObject GameOver;
	public bool controllable = true;
	public GameObject dashEFX;

	#endregion

    [Header("Speed")]
    public float HorizontalSpeed;
    public float jumpSpeed;
	Vector3 LeftDirection = new Vector3(0, 180, 0);
	Vector3 RightDirection = new Vector3(0, 0, 0);
	public Vector3 currentSpeed;
	private float DashTimer;
    public float slowMultiplier = 1;
    bool WallDirection; //true=wall is on the right, false=left
    public bool grabed;
    public bool isAttacking;
    public bool canTurnAround;

	private bool isPressing = false;
    GameManager gm;

    void MoveABit()
    {if(!isGround2&&horizontalDirection!=0)
        playerRigidbody2D.transform.position = new Vector3(playerRigidbody2D.transform.rotation.y==0?playerRigidbody2D.transform.position.x+0.5f: playerRigidbody2D.transform.position.x - 0.5f, playerRigidbody2D.transform.position.y, 0);
    }
    void MoveTwoBit()
    {
        if (!isGround2 && horizontalDirection != 0)
            playerRigidbody2D.transform.position = new Vector3(playerRigidbody2D.transform.rotation.y == 0 ? playerRigidbody2D.transform.position.x + 0.8f : playerRigidbody2D.transform.position.x - 0.8f, playerRigidbody2D.transform.position.y, 0);
    }
    void StopMoving()
    {
        playerRigidbody2D.velocity = Vector2.zero;
    }
    void MovementX()
	{
        horizontalDirection = Input.GetAxisRaw("Horizontal");
        if (horizontalDirection < 0) horizontalDirection = -1;
        else if (horizontalDirection > 0) horizontalDirection = 1;
        if (Time.timeScale != 0 && controllable)
        {
            if (horizontalDirection != 0 && isGround && !isAttacking)
            {
                anim.SetBool("Run", true);
                am.UnMute("PlayerRun");
            }
            else
            {
                anim.SetBool("Run", false);
                am.Mute("PlayerRun");
            }
            if (isAttacking&&canTurnAround)
            {
                if (horizontalDirection < 0)
                {
                    playerRigidbody2D.transform.eulerAngles = LeftDirection;
                }
                else if (horizontalDirection > 0)
                {
                    playerRigidbody2D.transform.eulerAngles = RightDirection;
                }
            }
            if (!isAttacking||isDashing)
            {
                if (!isDashing && !isWallJump) //!isAttacking  !isWallJump
                {
                    if (horizontalDirection < 0)
                    {
                        playerRigidbody2D.transform.eulerAngles = LeftDirection;
                    }
                    else if (horizontalDirection > 0)
                    {
                        playerRigidbody2D.transform.eulerAngles = RightDirection;
                    }
                    currentSpeed = new Vector3(HorizontalSpeed * horizontalDirection, playerRigidbody2D.velocity.y, 0);
                }
                if ((isGround || OnBoss) && !jumping) //((isGround || OnBoss) && !jumping)
                    playerRigidbody2D.velocity = new Vector2(currentSpeed.x, 0);
                else
                    playerRigidbody2D.velocity = new Vector2(currentSpeed.x, playerRigidbody2D.velocity.y);
            }
        }

	}

    public bool JumpKey
	{
		get
		{
			return Input.GetButtonDown("Jump");
		}
	}
	public bool DashKey
	{
		get
		{
			return Input.GetButtonDown("Fire2");
		}
	}
    void TryDash()
	{
		if (isGround && airDashed)
		{
			airDashed = false;

		}
		if (DashKey && !isDashing && !airDashed &&DashTimer>=0.8f &&horizontalDirection!=0)
		{
            DashTimer = 0;
            tempHorizontalDirection = horizontalDirection;
			Dash();
		}
	}
	bool isGround
	{
		get
		{
			Vector2 start = groundCheck.position;
			Vector2 end = new Vector2(start.x, start.y - distance);
			Debug.DrawLine(start, end, Color.blue);
            Debug.DrawLine(groundCheck2.position, new Vector2(groundCheck2.position.x,groundCheck2.position.y-distance), Color.blue);
            grounded = (Physics2D.Linecast(start, end, groundLayer)|| Physics2D.Linecast(groundCheck2.position, new Vector2(groundCheck2.position.x, groundCheck2.position.y - distance), groundLayer));
			return grounded;
		}
	}
    bool isBoss
    {
        get
        {
            Vector2 start = groundCheck.position;
            Vector2 end = new Vector2(start.x, start.y - distance);
            Debug.DrawLine(start, end, Color.blue);
            OnBoss = Physics2D.Linecast(start, end, BossLayer);
            return OnBoss;
        }
    }
    //float WallX;
    RaycastHit2D hitPoint;
    bool isWall
    {
        get
        {
            //Vector2 start = WallCheck.position;           
            //Vector2 end = new Vector2(start.x + horizontalDirection<0? 1.5f:-1.5f, start.y);
            Debug.DrawLine(new Vector2(gameObject.transform.position.x-0.95f,gameObject.transform.position.y+0.27f), WallCheck.position, Color.blue);
            //HitWall = Physics2D.Linecast(new Vector2(gameObject.transform.position.x - 0.95f, gameObject.transform.position.y + 0.27f), WallCheck.position, WallLayer);
            hitPoint = Physics2D.Linecast(new Vector2(gameObject.transform.position.x - 0.95f, gameObject.transform.position.y + 0.27f), WallCheck.position, WallLayer);

            //if (HitWall)
            //WallX =Physics2D.Linecast(new Vector2(gameObject.transform.position.x - 0.95f, gameObject.transform.position.y + 0.27f), WallCheck.position, WallLayer).collider.gameObject.transform.position.x;
            HitWall = hitPoint.collider == null ? false : true;
            return HitWall;
        }
    }
    bool isGround2
    {
        get
        {
            Debug.DrawLine(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.27f), WallCheck.position, Color.blue);
            return Physics2D.Linecast(new Vector2(gameObject.transform.position.x - 0.95f, gameObject.transform.position.y + 0.27f), WallCheck.position, groundLayer);
        }
    }
    void Dash()
	{
		am.Play("Dash");
		//Instantiate(dashEFX, transform.position, Quaternion.identity);

			if (Time.timeScale != 0 && controllable)
		{

            if (!isGround)
            {
                playerRigidbody2D.gravityScale = 0;
                GetComponent<Animator>().SetTrigger("Dash");
                airDashed = true;
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Dash");
            }
            isDashing = true;
		}
	}
	public void die()
	{
		Time.timeScale = 0;
		//GameOver.SetActive(true);
		//CursorVis.SetActive(true);
		//Destroy(gameObject);
	}
    bool jumping;
    void TryJump()
	{
		if (Time.timeScale != 0 && controllable)
		{
            if (isGround)//|| OnBoss)
            {
                anim.SetBool("Land", true);
                InsDust();
                highFallTimer = 0;
            }
            else
            {
                anim.SetBool("Land", false);
            }
			if ((isGround || OnBoss) && JumpKey && !isAttacking) //((isGround || OnBoss) && JumpKey &&!isAttacking)
            {
                jumping = true;
                anim.SetTrigger("Jump");
                //am.Play("Jump");
                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, jumpSpeed);
				doubleJump = true;
			}
            if (!(isGround || OnBoss) && JumpKey && doubleJump) //(!(isGround || OnBoss) && JumpKey && doubleJump)
            {
                jumping = true;
                //am.Play("Jump");
                anim.SetTrigger("Jump");
                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x * 0.8f, jumpSpeed);
                //				Instantiate(dashEFX, transform.position, Quaternion.identity);
                doubleJump = false;
            }
            if (!isGround && playerRigidbody2D.velocity.y <= 0) //||(isGround&&playerRigidbody2D.velocity.y==0))
            {
                jumping = false;
                anim.SetBool("Fall", true);
                playerRigidbody2D.gravityScale = fallMultiplier;
            }
            else
            {
                anim.SetBool("Fall", false);
                playerRigidbody2D.gravityScale = 1;
            }

        }
	}
    public void AttackOn()
    {
        AttackCollider.enabled=true;
    }
    public void AttackOff()
    {
        AttackCollider.enabled = false;
    }
    //public void DownAttackOn()
    //{
    //    DownAttackCollider.enabled = true;
    //}
    //public void DownAttackOff()
    //{
    //    DownAttackCollider.enabled = false;
    //}
    public void InsDust()
    {

        if (highFallTimer >= 0.25f)
        {
            am.Play("PlayerLand");
            Instantiate(DustEFX, transform.position, Quaternion.Euler(-90, 0, 0));
            if (highFallTimer >= 1.2f)
            {
                st.GetComponent<ShakeTest>().StartVibration(0.05f, 0.05f, 0.1f);
            }
            highFallTimer = 0;
        }
    }
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
        
    //}
    void TryAttack()
    {
		if (controllable && !grabed)
		{
			//if ((Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Fire1") == 1))
			if (Input.GetAxisRaw("Fire1") != 0|| Input.GetButtonDown("Fire1"))
			{
				if (isPressing == false)
				{
					// Call your event function here.
					//st.GetComponent<ShakeTest>().StartVibration(0.01f, 0.02f, 0.1f);
					if (isGround)
					{
						//slowMultiplier = 0.7f;
						//if (Random.Range(0, 2) < 1)
						//    anim.SetTrigger("Attack1");
						//else
						//    anim.SetTrigger("Attack2");
						playerRigidbody2D.velocity = Vector2.zero;
						anim.SetTrigger("Attack");
					}
					else
						anim.SetTrigger("AirAttack");
					isPressing = true;
				}
			}
			if (Input.GetAxisRaw("Fire1") == 0)
			{
				isPressing = false;
			}
			{

			}
		}
       
    }
    public void AtkAudio()
    {
        st.GetComponent<ShakeTest>().StartVibration(0.01f, 0.02f, 0.1f);
        am.Play("Nothing");
    }
    public void controllableOn()
    {
        controllable = true;
    }
    public void controllableOff()
    {
        controllable = false;
    }
    public void RecoverSpeed()
    {
        slowMultiplier = 1;
    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        HorizontalSpeed = gm.HorizontalSpeed;
        jumpSpeed = gm.JumpSpeed;
        jumping = false;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
		controllable = true;
        DashTime = 0.2f;
        DashTimer = 2.5f;
        anim = GetComponent<Animator>();
		st = new GameObject();
		st.AddComponent<ShakeTest>();
	}
    private void Awake()
    {
		Time.timeScale = 1;
        am=FindObjectOfType<AudioManager>();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 3:
                am.UnMute("BGM1");
                am.Mute("BGM0");
                am.Mute("BGM2");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
            case 4:
                am.UnMute("BGM1");
                am.Mute("BGM0");
                am.Mute("BGM3");
                am.Mute("BGM2");
                am.Mute("BGM4");
                break;
            case 5:
                am.UnMute("BGM3");
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM2");
                am.Mute("BGM4");
                break;
            case 6:
                am.UnMute("BGM3");
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM2");
                am.Mute("BGM4");
                break;
            case 7:
                am.UnMute("BGM3");
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM2");
                am.Mute("BGM4");
                break;
            case 8:
                am.UnMute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM2");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
            case 9:
                am.UnMute("BGM2");
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
            case 10:
                am.UnMute("BGM2");
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
            case 11:
                am.UnMute("BGM0");
                am.Mute("BGM2");
                am.Mute("BGM1");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
            case 12:
                am.UnMute("BGM4");
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM3");
                am.Mute("BGM2");
                break;
            case 13:
                am.UnMute("BGM4");
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM3");
                am.Mute("BGM2");
                break;
            case 14:
                am.UnMute("BGM0");
                am.Mute("BGM2");
                am.Mute("BGM1");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
            case 15:
                am.UnMute("BGM0");
                am.Mute("BGM2");
                am.Mute("BGM1");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
            default:
                am.Mute("BGM0");
                am.Mute("BGM1");
                am.Mute("BGM2");
                am.Mute("BGM3");
                am.Mute("BGM4");
                break;
        }
    }
    private void OnEnable()
    {
            GetComponentInChildren<Climb>().enabled = false;
        GetComponentInChildren<ClimbDown>().enabled = false;

        anim = GetComponent<Animator>();

        anim.speed = 1;
        airDashed = false;
        isDashing = false;

    }
    void Update()
    {
        if (am==null)
            am = FindObjectOfType<AudioManager>();

        if (!isGround&&playerRigidbody2D.velocity.y!=0)
        {
            highFallTimer += Time.deltaTime;
        }
        DashTimer += Time.deltaTime;
        TryJump();
        TryAttack();
        TryDash();
        if (isDashing && (DashTime > 0))
        {
            DashTime -= Time.deltaTime;
            //if (echoTime <= 0)
            //{
                GameObject instance = Instantiate(DashEcho, transform.position, Quaternion.Euler(0,transform.rotation.y*180,0));
                Destroy(instance, 0.5f);
               // echoTime = StartEchoTime;
           // }
            //else
           // {
           //     StartEchoTime -= Time.deltaTime;
           // }
            currentSpeed.x = DashSpeed * tempHorizontalDirection * HorizontalSpeed;
            currentSpeed.y = 0;

        }
        else if (DashTime <= 0)
        {
            isDashing = false;
            playerRigidbody2D.gravityScale = 1;
            DashTime = 0.2f;
        }
        //             Wall Jump
        if (!isGround && isWall)
        {
            if (playerRigidbody2D.velocity.y < 0)
            {
                grabed = true;
                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, playerRigidbody2D.velocity.y * 0.9f); //挂墙
                anim.SetBool("Grab", true);
            }
            else
            {
                anim.SetBool("Grab", false);
                grabed = false; 
            }
            if (JumpKey)
            {
                anim.SetTrigger("WallJump");
                if (hitPoint.point.x > playerRigidbody2D.position.x)
                {
                    WallDirection = true;
                    playerRigidbody2D.transform.eulerAngles = LeftDirection;
                }
                else
                {
                    WallDirection = false;
                    playerRigidbody2D.transform.eulerAngles = RightDirection;
                }
                //anim.SetTrigger("WallJump");
                isWallJump = true;
                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, jumpSpeed);
            }

        }
        if (grounded||!isWall)
        {
            anim.SetBool("Grab", false);
            grabed = false;
        }
        if (isWallJump)
        {
            //WallTime -= Time.deltaTime;
            if (WallDirection)
                currentSpeed.x = -10;
            else
                currentSpeed.x = 10;
            
            if (playerRigidbody2D.velocity.y < 0)
                isWallJump = false;

        }
        //else if (playerRigidbody2D.velocity.y<0)
        //{
        //    isWallJump = false;
        //    //WallTime = 0.3f;
        //}
        //if (isGround)
        //    playerRigidbody2D.gravityScale = 0;
        //else playerRigidbody2D.gravityScale = 1;
        MovementX();
    }
    private void FixedUpdate()
    {
        MovementX();

    }
    //public float WallTime=0.3f;
    //void wallJump()
    //{
    //    //controllable = false;
    //    if (WallX > playerRigidbody2D.position.x)
    //        currentSpeed = new Vector3(-20,jumpSpeed,0);
    //        //playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x * 0.8f-50, jumpSpeed);
    //    else
    //        currentSpeed = new Vector3(20, jumpSpeed, 0);
    //    //playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x * 0.8f + 50, jumpSpeed);

    //    //yield return new WaitForSeconds(0.8f);
    //    //controllable = true;
    //}
}
