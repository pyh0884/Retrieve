using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolTest : MonoBehaviour
{
	public float time = 1.0f;
	public Transform Target;
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(JumpInParabola(Target,time));
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	IEnumerator JumpInParabola(Transform target,float time)
	{
		float tempX = transform.position.x;
		float tempY = transform.position.y;
		float prob = (tempY - target.position.y) / ((tempX - target.position.x) * (tempX - target.position.x));
		float speed = 2*(target.position.x - tempX) / time;
		while (target.position.x<tempX?transform.position.x>=(2*target.position.x-tempX):transform.position.x<= (2 * target.position.x - tempX))
		{
			transform.position = Vector3.MoveTowards(transform.position,new Vector3(target.position.x,transform.position.y),speed*Time.deltaTime);
			float temp = transform.position.x - target.position.x;
			transform.position = new Vector3(transform.position.x, target.position.y + prob * temp * temp);
			yield return null;
		} 
	}
}
