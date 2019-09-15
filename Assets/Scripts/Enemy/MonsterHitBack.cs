using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBack : MonoBehaviour
{
	public float hitBackDist = 1;
	public float hitBackSpeed = 5;
	public int maxBackFrame = 10;
	public GameObject GroundCheck1;
	public GameObject GroundCheck2;
	public GameObject WallCheck;
	public bool Grounded;
	public bool Walled;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void gotHitCheck() {
		if (isGround && !isWall) StartCoroutine(GotHit());
	}

	IEnumerator GotHit() {
		Vector3 target;
		int frameCount = 0;
		target = transform.position + transform.right * hitBackDist;
		//if (right) target = transform.position + new Vector3(hitBackDist, 0, 0);
		//else target = transform.position + new Vector3(-hitBackDist, 0, 0);
		while (frameCount<maxBackFrame) {
			transform.position = Vector3.MoveTowards(transform.position, target, hitBackSpeed * Time.deltaTime);
			//Debug.Log("Backing");
			yield return null;
			frameCount++;
		} 
	}

	public bool isGround
	{
		get
		{
			Vector2 start1 = GroundCheck1.transform.position;
			Vector2 start2 = GroundCheck2.transform.position;
			Vector2 end1 = new Vector2(GroundCheck1.transform.position.x, GroundCheck1.transform.position.y - 2);
			Vector2 end2 = new Vector2(GroundCheck2.transform.position.x, GroundCheck2.transform.position.y - 2);
			Debug.DrawLine(start1, end1, Color.blue);
			Debug.DrawLine(start2, end2, Color.yellow);
			Grounded = Physics2D.Linecast(start1, end1, groundLayer) && Physics2D.Linecast(start2, end2, groundLayer);
			return Grounded;
		}
	}

	public bool isWall
	{
		get
		{
			Vector2 start = WallCheck.transform.position;
			Vector2 end1 = new Vector2(WallCheck.transform.position.x + 2, WallCheck.transform.position.y);
			Vector2 end2 = new Vector2(WallCheck.transform.position.x - 2, WallCheck.transform.position.y);
			Debug.DrawLine(start, end1, Color.red);
			Debug.DrawLine(start, end2, Color.green);
			Walled = Physics2D.Linecast(start, end1, wallLayer) || Physics2D.Linecast(start, end2, wallLayer);
			return Walled;
		}
	}
}
