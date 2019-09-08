using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehaviour : MonoBehaviour
{
	public float waitTime;
	public float runTime;
	public float stayTime;
	public GameObject target;
	public float speed;
	public int state;
	public float vel;
	private float timeStamp;
	// Start is called before the first frame update
	void Start()
	{
		state = 0;
		StartCoroutine("Main");
	}
	// Update is called once per frame
	void Update()
    {
        if (!target)target = GameObject.FindWithTag("Player");
        switch (state) {
			case 0: transform.right = target.transform.position - transform.position;break;
			case 1: Shoot(Mathf.Lerp(0, speed, (Time.time - timeStamp) / runTime));break;
			case 2: Shoot(Mathf.Lerp(speed, 0, (Time.time - timeStamp) / runTime)); break;
			case 3: Shoot(0);break;
			case 4: Shoot(Mathf.Lerp(0, -speed, (Time.time - timeStamp) / runTime)); break;
			case 5: Shoot(Mathf.Lerp(-speed, 0, (Time.time - timeStamp) / runTime)); break;
			default:Destroy(gameObject, 0.01f);break;
		}
	}

	void Shoot(float speed)
	{
		GetComponent<Rigidbody2D>().velocity = speed * transform.right;
		vel = GetComponent<Rigidbody2D>().velocity.magnitude;
	}

	void StateUpgrade() {
		state++;
		timeStamp = Time.time;
	}

	IEnumerator Main() {
		yield return new WaitForSeconds(waitTime);
		StateUpgrade();
		yield return new WaitForSeconds(runTime);
		StateUpgrade();
		yield return new WaitForSeconds(runTime);
		StateUpgrade();
		yield return new WaitForSeconds(stayTime);
		StateUpgrade();
		yield return new WaitForSeconds(runTime);
		StateUpgrade();
		yield return new WaitForSeconds(runTime);
		StateUpgrade();
		yield return null;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "PlayerHitBox") Destroy(gameObject,0.01f);
	}
}
