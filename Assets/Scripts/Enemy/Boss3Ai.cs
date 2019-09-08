using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss3Ai : MonoBehaviour
{
	public bool pubCD;
	public float pubCDTime;
	public bool isSkill;
	public bool skyAttack;
	public bool isFlying;
	public float flySpeed;
	public float flyStayTime = 6;
	public float refreshTime = 10;
	public int colorState;
	public int colorNum = 3;
	public List<bool> SkillCD;
	public List<float> SkillCDTime;
	public float nearDist;
	public float midDist;
	public float flyHeight;
	public GameObject player;
	public GameObject boomerangPrefab;
	public GameObject sprintPrefab;
	public GameObject stabPrefab;
	public GameObject laserPrefab;
	public int numOfCall = 5;
	public float stabGap;
	public float laserGap;
	public float stabFloor;
	public GameObject missileSpawner;
	public GameObject stonePrefab;
	public List<Transform> handTrans;
	public List<Transform> footTrans;
	public List<Transform> birthPoints;
	public GameObject eyes;
	public List<GameObject> seTu;
	public float seTuFadeSpeed = 5f;

	public bool HitWall;
	public Transform wallCheck;
	public float wallCheckDistance;
	public LayerMask groundLayer;

	public float speed;
	private Rigidbody2D enemyRigidBody;
	private int skillNum;
	private Vector3 dist;
	private List<Vector3> stabPosList;
	private GameObject spawn;
	private GameObject bull;
	private GameObject ball;
	private SpriteRenderer[] sr = new SpriteRenderer[4];
    private Animator anim;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
		enemyRigidBody = GetComponent<Rigidbody2D>();
		for (int i = 0; i < seTu.Count; i++) sr[i] = seTu[i].GetComponent<SpriteRenderer>();
		colorState = 0;
		isSkill = false;
		StartCoroutine("PubCD_Counter");
		StartCoroutine(MoveStateCounter());
		stabPosList = new List<Vector3>();
		for(int i=0;i<SkillCD.Count;i++)SkillCD[i]= false;
		Refresh();
        ps = GetComponentInChildren<ParticleSystem>();
        
	}

    public void Invincible()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    public void InvincibleOff()
    {
        GetComponent<Collider2D>().enabled = true;

    }


    void Update()
	{
        if (!player) player = GameObject.FindWithTag("Player");
        else
        {
            dist = player.transform.position - transform.position;
        }
        if (!isSkill)
		{
			if (!isFlying) Move();
			else enemyRigidBody.velocity = Vector2.zero;
			if (!pubCD)
			{
				StartCoroutine(seTuFadeIn(seTu[colorState].GetComponent<SpriteRenderer>(), seTuFadeSpeed));
				colorState = ColorSelect();
				if (!skyAttack) skillNum = colorState * 2 - 1;
				else skillNum = colorState * 2;
				isSkill = true;
			}
		}
		else
		{
			if (!pubCD)
			{
				enemyRigidBody.velocity = Vector2.zero;
				SkillUse(skillNum);
			}
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
			if (transform.eulerAngles.y < 0)
				end = new Vector2(start.x - wallCheckDistance, start.y);
			else
				end = new Vector2(start.x + wallCheckDistance, start.y);
			Debug.DrawLine(start, end, Color.blue);
			HitWall = Physics2D.Linecast(start, end, groundLayer);
			return HitWall;
		}
	}

	void Move() {
		if (!isWall)
		{
			if (Mathf.Abs(dist.x) < midDist)
			{
				if (dist.x > 0)
				{
					transform.eulerAngles = -180 * Vector3.up;
					enemyRigidBody.velocity = new Vector2(-speed, 0);
				}
				else
				{
					transform.eulerAngles = Vector3.zero;
					enemyRigidBody.velocity = new Vector2(speed, 0);
				}
			}
			else enemyRigidBody.velocity = Vector2.zero;
		}
		else
		{
			enemyRigidBody.velocity = Vector2.zero;
			Refresh();
		}
	}

	int ColorSelect()
	{
		int temp = Random1ToN(colorNum);
		for (int i = 0; temp == colorState; i++)
		{
			temp = Random1ToN(colorNum);
		}
		//更改外观主题色
		switch (temp)
		{
			case 1:
				{
                    ps.startColor = new Vector4(0.8078f,0.7411f,0.5607f,0.65f);
                    eyes.GetComponent<SpriteRenderer>().color = Color.yellow;
					break;
				}
			case 2:
				{

                    ps.startColor = new Vector4(0.3803f, 0.5568f, 0.2156f, 0.65f);
                    eyes.GetComponent<SpriteRenderer>().color = new Vector4(0.572549f, 0.8235295f, 0.3294118f, 1);
					break;
				}
			case 3:
				{
                    ps.startColor = new Vector4(0.5411f, 0.8352f, 1, 0.65f);
                    eyes.GetComponent<SpriteRenderer>().color = Color.cyan;
					break;
				}
		}
		StartCoroutine(seTuFadeOut(sr[temp], seTuFadeSpeed));
		return temp;
	}

	void SkillUse(int num)
	{
		if (SkillCD[num]==true)
		{
			isSkill = false; return;
		}
		else {
            anim.SetTrigger("Skill");
			switch (num)
			{

				case 1://黄色地面技能（Prefab冲锋）
					{
						if (dist.x < 0) bull = Instantiate(sprintPrefab, footTrans[0].position, new Quaternion());
						else bull = Instantiate(sprintPrefab, footTrans[1].position, Quaternion.Euler(0, transform.rotation.y * 180, 0));
						Destroy(bull, 2.5f);
						isSkill = false;
						break;

					}
				case 2://黄色空中技能（回旋镖）
					{
                        //播放动画
                        if (Random.Range(0,1)>0.5f)
                        FindObjectOfType<AudioManager>().Play("Throw1");
                        else
                        FindObjectOfType<AudioManager>().Play("Throw2");
                        Instantiate(boomerangPrefab, handTrans[0].position, new Quaternion());
						isSkill = false;
						break;
					}
				case 3://绿色地面技能（荆棘地刺）
					{
						//播放动画
						stabPosList.Clear();
						float tempPos = transform.position.x;
						for (int j = 1; j <= numOfCall; j++)
						{
							stabPosList.Add(new Vector3(tempPos + stabGap * j, stabFloor));
							stabPosList.Add(new Vector3(tempPos - stabGap * j, stabFloor));
						}
						for (int i = 0; i < 2*numOfCall; i++)
						{
							Instantiate(stabPrefab, stabPosList[i],new Quaternion());
						}
						isSkill = false;
						break;
					}
				case 4://绿色空中技能（羽毛/弹幕射击）
					{
						Instantiate(missileSpawner,handTrans[0].position,new Quaternion());
						//播放动画
						isSkill = false;
						break;
					}
				case 5://蓝色地面技能（激光）
					{
                        FindObjectOfType<AudioManager>().Play("Laser");
						Instantiate(laserPrefab, new Vector3(0, 0), Quaternion.Euler(0, 0, transform.rotation.y == 0 ? -18.75f : 18.75f));
						Instantiate(laserPrefab, new Vector3(laserGap, 0), Quaternion.Euler(0, 0, transform.rotation.y == 0 ? -18.75f : 18.75f));
						Instantiate(laserPrefab, new Vector3(-laserGap, 0), Quaternion.Euler(0, 0, transform.rotation.y == 0 ? -18.75f : 18.75f));
						isSkill = false;
						break;
					}
				case 6://蓝色空中技能（丢碰撞球）
					{
						ball=Instantiate(stonePrefab, handTrans[0].position, new Quaternion());
						Destroy(ball, 5f);
						isSkill = false;
						break;
					}
				default:break;
			}
			StartCoroutine("PubCD_Counter");
			StartCoroutine(SkillCD_Counter(num));
		}
	}

	IEnumerator SkillCD_Counter(int i) {
		SkillCD[i] = true;
		yield return new WaitForSeconds(SkillCDTime[i]);
		SkillCD[i] = false;
	}

	IEnumerator MoveStateCounter() {
		float temp = transform.position.y;
		while (true)
		{
			skyAttack = false;
			yield return new WaitForSeconds(flyStayTime);
			isFlying = true;
			while (transform.position.y != flyHeight)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, flyHeight), flySpeed * Time.deltaTime);
				yield return 0;
			}
			isFlying = false;
			skyAttack = true;
			yield return new WaitForSeconds(flyStayTime);
			isFlying = true;
			while (transform.position.y != temp)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, temp), flySpeed * Time.deltaTime);
				yield return 0;
			}
			isFlying = false;
		}
	}

	public int Random1ToN(int n)
	{
		int temp = Mathf.CeilToInt(Random.value * n);
		while(temp==0) temp = Mathf.CeilToInt(Random.value * n);
		return temp;
	}

	IEnumerator RefreshCount() {
		float temp = Time.time;
		while (Time.time - temp <= 10f)
		{
			if (!isWall) yield return 0;
			else yield break;
		}
        anim.SetTrigger("TP");
        yield return new WaitForSeconds(0.5f); 
		Refresh();
	}

	void Refresh() {
		if (transform.position.x < 0) transform.position = new Vector3(birthPoints[0].position.x, transform.position.y);
		else transform.position = new Vector3(birthPoints[1].position.x, transform.position.y);
		StartCoroutine(RefreshCount());
	}

	IEnumerator PubCD_Counter()
	{
		pubCD = true;
		yield return new WaitForSeconds(pubCDTime);
		pubCD = false;
	}

	IEnumerator seTuFadeOut(SpriteRenderer renderer, float fadeSpeed) {
		while (renderer.color != Color.clear)
		{
			renderer.color = Vector4.MoveTowards(renderer.color, Color.clear, fadeSpeed * Time.deltaTime);
			yield return 0;
		}
	}

	IEnumerator seTuFadeIn(SpriteRenderer renderer, float fadeSpeed) {
		while (renderer.color != Color.white)
		{
			renderer.color = Vector4.MoveTowards(renderer.color, Color.white, fadeSpeed * Time.deltaTime);
			yield return 0;
		}
	}
}
