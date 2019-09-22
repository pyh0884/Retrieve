using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDown : MonoBehaviour
{

    #region variables
    public Transform player;
    public float ClimbSpeed = 6.5f;
    public Rigidbody2D playerRigidbody2D;
    public Animator anim;
    public float fallMultiplier = 1.4f;
    [Header("current direction on X axis")]
    public float horizontalDirection;//between -1~1
    public float verticalDirection;
    private float tempHorizontalDirection;

    [Header("distance from the ground")]
    [Range(0, 0.5f)]
    public float distance;
    [Header("start point of the ray")]
    public Transform groundCheck;
    [Header("ground mask")]
    public LayerMask groundLayer;
    public bool grounded;

    [Header("start point of the ray")]
    public Transform WallCheck;
    public bool HitWall;
    public LayerMask BossLayer;
    public bool OnBoss;
    public bool controllable = true;
    //private bool onLadder;

    #endregion

    [Header("Speed")]
    public float HorizontalSpeed = 8;
    public float jumpSpeed = 10;
    public Vector3 LeftDirection;
    public Vector3 RightDirection;
    public Vector3 currentSpeed;
    public Climb clim;
    bool leaving;
    public HealthBarControl hbc;

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Ladder"&&player.transform.position.y<collision.transform.position.y)
    //    {
    //        onLadder = false;
    //        if (anim != null)
    //            anim.SetBool("Climb", false);
    //        GetComponentInParent<PlayerController>().enabled = true;

    //    }
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            if (Input.GetAxisRaw("Vertical") <-0.5f&&hbc.JumpTimer>0.3f)
            {
                //onLadder = true;
                //anim.speed = 1;
                player.position = new Vector3(collision.gameObject.transform.position.x, player.position.y, 0);
                anim.SetBool("Climb", true);
                //Debug.Log("ladder");
                GetComponent<ClimbDown>().enabled = true;
            }
            //else if (onLadder)
            //{
            //    anim.speed = 0;

            //}
        }
    }

    void MovementX()
    {
        LeftDirection = new Vector3(0, -180, 0);
        RightDirection = new Vector3(0, 0, 0);
        horizontalDirection = Input.GetAxisRaw("Horizontal");
    }
    public bool JumpKey
    {
        get
        {
            return Input.GetButtonDown("Jump");
        }
    }
    void TryJump()
    {
        if (Time.timeScale != 0 && controllable)
        {
            if (JumpKey && horizontalDirection != 0)
            {
                hbc.JumpTimer = 0;
                leaving = true;

                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x + 7, jumpSpeed);

                //onLadder = false;
                //playerRigidbody2D.gravityScale = 1;
                anim.SetBool("Climb", false);
                anim.SetTrigger("Jump");
                GetComponentInParent<PlayerController>().enabled = true;

            }
        }
    }
    private void OnEnable()

    {
        leaving = false;
        anim = GetComponentInParent<Animator>();

        GetComponentInParent<PlayerController>().enabled = false;
        playerRigidbody2D = GetComponentInParent<Rigidbody2D>();

        playerRigidbody2D.velocity = new Vector2(0, 0);


    }
    void Start()
    {
        //playerRigidbody2D.gravityScale = 0;
        controllable = true;
    }

    // Update is called once per frame
    void Update()
    {
        TryJump();
        if (!leaving)
        {
            verticalDirection = Input.GetAxisRaw("Vertical");
            if (verticalDirection < 0)
            {
                clim.enabled = true;
                GetComponent<ClimbDown>().enabled = false;
            }
            if (verticalDirection == 0)
            {
                playerRigidbody2D.gravityScale = 0;
                if (horizontalDirection == 0&&anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Climb"))
                    anim.speed = 0;
                else anim.speed = 1;
            }
            else
            {
                anim.speed = 1;
                playerRigidbody2D.gravityScale = 1;
            }
            if (!leaving)
                playerRigidbody2D.velocity = new Vector2(0, ClimbSpeed * verticalDirection);
            MovementX();
       }

    }
}
