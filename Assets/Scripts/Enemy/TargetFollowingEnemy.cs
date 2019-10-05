using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollowingEnemy : MonoBehaviour
{
	public float traceRouteStartTime=0.5f;
	public float lookAtEndTime=1.5f;
	public float mouseAttractionAmt = 0.01f;
	public float velocityLerpAmt = 0.25f;

	private GameObject target;
	private bool lookAtState;
	private bool traceRouteState;
	private Vector3 dist;
    public GameObject deadBody;

    public Vector3 velocity;          //当前帧速度
	public Vector3 newVelocity;       //下一帧速度
	public Vector3 newPosition;       //下一帧位置
	public bool isFire = true;



	// Start is called before the first frame update
	void Awake()
	{
        FindObjectOfType<AudioManager>().Play("Fire");
        target =GameObject.FindWithTag("Player");
		lookAtState = true;
		traceRouteState = false;
	}

	void Start()
    {
		StartCoroutine("Main");
    }

	public void LookAtTarget(Vector3 aim) {
        //		transform.LookAt(target.transform, -Vector3.forward);
        //      尝试用其他方法实现瞄脸
        //float zRotation = Mathf.Atan2(aim.x - transform.position.x, aim.y - transform.position.y);
        //zRotation -= 90;
        //float zRotationOrigin = transform.eulerAngles.z;
        //float zRotationAddition = zRotation - zRotationOrigin;
        //if (zRotationAddition > 180) zRotationAddition -= 360;
        //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + zRotationAddition);//垃圾
        //transform.right = aim - transform.position;
        if(isFire)transform.right=Vector3.right;
		else transform.right = aim - transform.position;

	}

    public void TraceRoute(Transform aim) {
		newVelocity = velocity;
		newPosition = transform.position;
		dist = new Vector3(aim.position.x, aim.position.y+1) - transform.position;
		newVelocity += dist * mouseAttractionAmt;
	}

	public void LinearMove() {
		newVelocity = velocity;
	}

    // Update is called once per frame
    void Update()
    {
		if (lookAtState)
		{
			LookAtTarget(new Vector3(target.transform.position.x, target.transform.position.y+1));
			if (traceRouteState)
			{
				TraceRoute(target.transform);
			}
		}
		else {
			LinearMove();
		}
		if (traceRouteState)
		{
			velocity = (1 - velocityLerpAmt) * velocity + velocityLerpAmt * newVelocity;
			newPosition = this.transform.position + velocity * Time.deltaTime;
			transform.position = newPosition;
		}
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {
            Instantiate(deadBody, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //FindObjectOfType<AudioManager>().Play("Stone");
            //此处加动画
            //Destroy(gameObject, 0.01f);//自毁等待时长要长于动画时长
        }
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        if (collision.tag == "Player")
        {
            //加动画
            Instantiate(deadBody, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator Main() {
		yield return new WaitForSeconds(traceRouteStartTime);
		traceRouteState = true;
		yield return new WaitForSeconds(lookAtEndTime - traceRouteStartTime);
		lookAtState = false;
		yield return null;
	}


}


