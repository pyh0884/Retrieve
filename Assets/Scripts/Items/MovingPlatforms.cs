using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	public float waitTime;
	public float moveSpeed;
	public bool Moving;
	public Transform startPos;
	public Transform endPos;
    public GameObject player;
    private bool OnPlatform;
    // Start is called before the first frame update
    void Start()
    {
		Moving = true;
		transform.position = startPos.position;
		StartCoroutine(Move(endPos.position));
    }

    //private void Update()
    //{
    //        if (!player) player = GameObject.FindWithTag("Player");
    //    if (player)
    //    {
    //        if (OnPlatform)
    //            player.transform.SetParent(gameObject.transform);
    //        else
    //            player.transform.SetParent(null);
    //    }

    //}
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        player.transform.SetParent(gameObject.transform);
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        player.transform.SetParent(null);
    //    }
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        player.transform.SetParent(gameObject.transform);
    //    }
    //}
    


    IEnumerator Move(Vector3 target) {
		while (Moving)
		{
			yield return new WaitForSeconds(waitTime);
			Vector3 current = transform.position;
			while (transform.position != target)
			{
				transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
				yield return 0;
			}
			yield return new WaitForSeconds(waitTime);
			while (transform.position != current)
			{
				transform.position = Vector3.MoveTowards(transform.position, current, moveSpeed * Time.deltaTime);
				yield return 0;
			}
		}
	}
}
