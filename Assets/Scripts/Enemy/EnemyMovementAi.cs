using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 敌方单位移动逻辑
/// </summary>
public class EnemyMovementAi : MonoBehaviour
{
    #region variables
    /// <summary>
    /// 常规变量
    /// </summary>
    public GameObject Attack2Object;
    public GameObject Attack4Object;
    private Rigidbody2D EnemyRigidbody2D;
    Animator anim;
    public Vector2 speed = new Vector2(6f, 6f);
    public Vector2 direction = new Vector2(-1, 0);//需要改成面对玩家方向
    private Vector2 movement;
    [Header("who is the player")]
    public GameObject Player;
    public Text hpText;
    public Slider hpSlider;
    //public int hp;
    //[Range(0,100)]
    //public int hpMax = 100;
    [Header("Stop Walking Distance")]
    public float StopDistance = 9.5f;
    private bool FindPlayer=false;
    /// <summary>
    /// 墙壁检测变量
    /// </summary>
    [Header("distance from the wall")]
    [Range(0, 10)]
    public float distance;
    [Header("start point of the ray")]
    public Transform wallCheck;
    [Header("wall mask")]
    public LayerMask groundLayer;
    public bool HitWall;
    /// <summary>
    /// 攻击移动判定变量
    /// </summary>
    [Header("The attack range of the enemy")]
    public float attackRange = 0.5f;
    [Header("distance from enemy to player")]
    public float DistanceToMe;
    public Vector3 LeftDirection = new Vector3(0, 0, 0);
    public Vector3 RightDirection = new Vector3(0, -180, 0);
    public bool playAttack2 = false;
    #endregion
    // 方法
    /// <summary>
    /// 判定是否为墙壁层
    /// </summary>
    bool isWall
    {
        get
        {
            Vector2 start = wallCheck.position;
            //Vector2 end = new Vector2(start.x + direction.x > 0 ? distance:-distance,start.y);
            Vector2 end;
            // = new Vector2(start.x + direction.x > 0 ? distance * 15: -distance * 15,start.y);
            switch (direction.x)
            {
                case 1:
                    end = new Vector2(start.x + distance, start.y);
                    break;
                case -1:
                    end = new Vector2(start.x - distance, start.y);
                    break;
                default:
                    end = new Vector2(start.x - distance, start.y);
                    break;
            }
            Debug.DrawLine(start, end, Color.blue);
            HitWall = Physics2D.Linecast(start, end, groundLayer);
            return HitWall;

        }
    }


    /// <summary>
    /// 巡逻型敌人，遇到墙转向
    /// </summary>
    void WanderEnemyMovement()
    {
        movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
        if (isWall) {
            switch (direction.x)
            {
                case 1:
                    EnemyRigidbody2D.transform.eulerAngles = LeftDirection;
                    break;
                case -1:
                    EnemyRigidbody2D.transform.eulerAngles = RightDirection;
                    break;
            }
            direction.x = -direction.x;
        }
        EnemyRigidbody2D.velocity = movement;
    }


    /// <summary>
    /// 追踪主角，并停在主角前方
    /// </summary>
    void Chasing()
    {
        if (Player != null)
        {
            if (playAttack2)
            {
                movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
                EnemyRigidbody2D.velocity = movement;
            }
            else if (!playAttack2&&DistanceToMe < 40)
                {
                    if (Player.transform.position.x > EnemyRigidbody2D.transform.position.x)
                    {
                        direction.x = 1;
                        EnemyRigidbody2D.transform.eulerAngles = LeftDirection;
                    }
                    else
                    {
                        direction.x = -1;
                        EnemyRigidbody2D.transform.eulerAngles = RightDirection;
                    }
                    if (DistanceToMe > StopDistance && !isWall)
                    {
                        movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
                        EnemyRigidbody2D.velocity = movement;
                    }
                    else
                    {
                        EnemyRigidbody2D.velocity = Vector2.zero;
                    }

                }
                else
            {
                EnemyRigidbody2D.velocity = Vector2.zero;

                //movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
                //EnemyRigidbody2D.velocity = movement;
            }

        }
        else
        {
            EnemyRigidbody2D.velocity = Vector2.zero;
        }
    }

    void MovableOn()
    {
        speed.x=6;
    }
    
    void MovableOff()
    {
        speed.x=0;
    }

    void muteSound()
    {
        FindObjectOfType<AudioManager>().Mute("BossCharge");
        FindObjectOfType<AudioManager>().Mute("BossRun");
    }
    void die()
    {
        FindObjectOfType<AudioManager>().Mute("BossCharge");
        FindObjectOfType<AudioManager>().Mute("BossRun");
        FindObjectOfType<AudioManager>().Play("Boss_Dead");

        Destroy(gameObject);
    }

    public void TouchableOn()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void TouchableOff()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// 攻击判定
    /// </summary>
    private float roll;
    public float commonCd = 5;     //todo:update中加入cd+=deltatime；
    public float Attack2Cd = 10;    //20s
    public float Attack3Cd = 10;    //20s
    public float Attack4Cd = 2;    //3s
    public float Attack1Cd = 5;    //10s
    void AttackSelection()
    {
        if (Player != null)
        {
            if (commonCd > 5)
            {
                if (DistanceToMe <= 10)
                {
                    if (Attack4Cd >= 2)
                    {
                        anim.SetTrigger("Attack4");
                        Attack4Cd = 0;
                    }
                }
                else if (DistanceToMe <= 11)
                {
                    roll = (float)Random.Range(0.0f, 10.0f);
                    if (Attack3Cd >= 10 && roll >= 8)
                    {
                        anim.SetTrigger("Attack3");
                        Attack3Cd = 0;
                    }
                    else if (Attack4Cd >= 2)
                    {
                        anim.SetTrigger("Attack4");
                        Attack4Cd = 0;
                    }
                    roll = 0;
                }
                else if (DistanceToMe <= 60)
                {
                    roll = (float)Random.Range(0.0f, 10.0f);
                    if (Attack1Cd >= 5 && roll > 6)
                    {
                        anim.SetTrigger("Attack1");
                        Attack1Cd = 0;
                    }
                    else if (Attack2Cd >= 10 && roll > 3 && !isWall)
                    {
                        anim.SetTrigger("Attack2");
                        Attack2Cd = 0;
                    }
                    else if (Attack3Cd >= 10)
                    {
                        anim.SetTrigger("Attack3");
                        Attack3Cd = 0;
                    }
                    roll = 0;
                }
                else if (DistanceToMe <= 120)
                {
                    roll = (float)Random.Range(0.0f, 10.0f);
                    if (Attack1Cd >= 5 && roll > 5)
                    {
                        anim.SetTrigger("Attack1");
                        Attack1Cd = 0;
                    }
                    else if (Attack3Cd >= 10)
                    {
                        anim.SetTrigger("Attack3");
                        Attack3Cd = 0;
                    }
                    roll = 0;
                }
                commonCd = 0;
            }
        }
    }

    void Attack1()
        {
        if (DistanceToMe > 9)
        {
            anim.SetTrigger("Attack1");
        }
        ///
        }

  
    void activateAttack2()
    {
        FindObjectOfType<AudioManager>().UnMute("BossCharge");
        playAttack2 = true;
        Attack2Object.SetActive(true);
        speed = speed * 12;
    }

    void Attack2Start()
    {
        if (playAttack2)
        {
            if (isWall)
            {
                GetComponent<Animator>().SetTrigger("HitPlayer");
                Attack2End();
            }
        }
    }

    void Attack2End()
    {
        FindObjectOfType<AudioManager>().Mute("BossCharge");
        playAttack2 = false;
        Attack2Object.SetActive(false);
        speed = new Vector2(6,6);
        
    }

    void Attack4Start()
    {
        Attack4Object.SetActive(true);
        anim.SetTrigger("Attack4");
    }

    void Attack4End()
    {
        Attack4Object.SetActive(false);
    }


    /// <summary>
    /// start.update.fixedupdate
    /// </summary>
    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        EnemyRigidbody2D = GetComponent<Rigidbody2D>();
        //hp = hpMax;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Player != null)
            DistanceToMe = Vector2.Distance(Player.transform.position, EnemyRigidbody2D.transform.position);
        if (FindPlayer) AttackSelection();
        else if (DistanceToMe < 40) FindPlayer = true;

        commonCd += Time.deltaTime;
            Attack1Cd += Time.deltaTime;
            Attack2Cd += Time.deltaTime;
            Attack3Cd += Time.deltaTime;
            Attack4Cd += Time.deltaTime;
            Attack2Start();
            if (DistanceToMe < 90 && direction.x != 0 && !playAttack2 && !isWall && speed.x == 6 && Time.timeScale!=0)
            {
                FindObjectOfType<AudioManager>().UnMute("BossRun");
            }
            else
                FindObjectOfType<AudioManager>().Mute("BossRun");
        
    }
    void FixedUpdate()
    {
        //WanderEnemyMovement();
        Chasing();
    }
}
