using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilesSpawn : MonoBehaviour
{
	public GameObject misslePrefab;
	public int numOfMissle;
	public float r;
	public float timeSpace;
	public float Speed;
	public float liveTime;
	private float angle;
	public bool isOn = false;
	IEnumerator MissleSpawn(int num)
	{
		for (int i = 0; i < num; i++)
		{
			yield return new WaitForSeconds(timeSpace);
			angle = i * 2 * Mathf.PI / (float)num;
			Instantiate(misslePrefab, new Vector3(r * Mathf.Sin(angle),r * Mathf.Cos(angle), 0)+ transform.position,new Quaternion(0,0,0,0),transform);

		}
		yield return Rotate();
	}
	IEnumerator Rotate() {
		for (int i = 0;i<Speed/10; i++) {
			transform.Rotate(Vector3.back, Speed * Time.deltaTime);
			yield return 0;
		}
		yield return new WaitForSeconds(liveTime);
		Destroy(gameObject);
	}
	// Start is called before the first frame update
	void Awake()
    {
		
    }
    // Update is called once per frame
    void Update()
    {
		if (isOn)
		{
			StartCoroutine(MissleSpawn(numOfMissle));
			isOn = false;
		}
	}
}
