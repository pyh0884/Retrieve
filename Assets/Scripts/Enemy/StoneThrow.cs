using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrow : MonoBehaviour
{
	public GameObject stonePrefab;
	public float shootSpeed;
	public float CDTime;
	public bool CD;
	public Transform shootPos;
	private GameObject player;
	public Vector3 dist;
	private GameObject stone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindWithTag("Player");
        else
        {
            dist = player.transform.position - transform.position;
        }
        if (player&&!CD) {
			if (dist.magnitude < 5) {
				stone = Instantiate(stonePrefab, shootPos.position, Quaternion.Euler(0, transform.rotation.y * 180, 0));
				stone.transform.right = dist.normalized;
				stone.GetComponent<Rigidbody2D>().velocity = shootSpeed * dist.normalized;
				StartCoroutine(CDCount());
			}
		}
    }
	IEnumerator CDCount() {
		CD = true;
		yield return new WaitForSeconds(CDTime);
		CD = false;
	}
}
