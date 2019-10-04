using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBack : MonoBehaviour
{
	public float hitBackDist = 1;
	public float hitBackSpeed = 5;
	public int maxBackFrame = 10;
	//public GameObject GroundCheck1;
	//public GameObject GroundCheck2;
	public GameObject WallCheck;
	//public bool Grounded;
	public bool Walled;
	//public LayerMask groundLayer;
	public LayerMask wallLayer;    
	public void gotHitCheck() {
		if (/*isGround && */!isWall) StartCoroutine(GotHit());
	}
    public void Des()
    {
        Destroy(this);
    }
    IEnumerator GotHit() {
		Vector3 target;
		int frameCount = 0;
		target = transform.position + transform.right * hitBackDist;
		//if (right) target = transform.position + new Vector3(hitBackDist, 0, 0);
		//else target = transform.position + new Vector3(-hitBackDist, 0, 0);
		while (frameCount<maxBackFrame) {
			transform.position = Vector3.MoveTowards(transform.position, target, hitBackSpeed * Time.fixedDeltaTime);
			//Debug.Log("Backing");
			yield return null;
			frameCount++;
		} 
	}

    //public bool isground
    //{
    //	get
    //	{
    //		vector2 start1 = groundcheck1.transform.position;
    //		vector2 start2 = groundcheck2.transform.position;
    //		vector2 end1 = new vector2(groundcheck1.transform.position.x, groundcheck1.transform.position.y - 2);
    //		vector2 end2 = new vector2(groundcheck2.transform.position.x, groundcheck2.transform.position.y - 2);
    //		debug.drawline(start1, end1, color.blue);
    //		debug.drawline(start2, end2, color.yellow);
    //		grounded = physics2d.linecast(start1, end1, groundlayer) && physics2d.linecast(start2, end2, groundlayer);
    //		return grounded;
    //	}
    //}

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
