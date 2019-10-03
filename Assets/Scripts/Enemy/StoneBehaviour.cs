using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBehaviour : MonoBehaviour
{
    //public GameObject brickPrefab;
    public GameObject target;
    public float waitTime;
    public float speed;
    public Vector3 SpawnPos;
    private bool ready;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindWithTag("Player");
        ready = false;
        StartCoroutine("Main");
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready) transform.right = target.transform.position - transform.position;
    }

    void Shoot()
    {
        GetComponent<Rigidbody2D>().velocity = speed * transform.right;
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 8||collision.gameObject.layer == 12)
    //    {
    //        Destroy(gameObject);
    //        //FindObjectOfType<AudioManager>().Play("Stone");
    //        //Instantiate(brickPrefab, transform.position + SpawnPos, new Quaternion(0, 0, 0, 0));
    //        //此处加动画
    //        //Destroy(gameObject, 0.01f);//自毁等待时长要长于动画时长
    //    }
    //    //private void OnTriggerEnter2D(Collider2D collision)
    //    //{
    //    //	if (collision.tag == "Player")
    //    //	{
    //    //		//加动画
    //    //		Destroy(this, 0.01f);//自毁等待时长要长于动画时长
    //    //	}
    //    }

    IEnumerator Main()
	{
		yield return new WaitForSeconds(waitTime);
		ready = true;
		Shoot();
		yield return null;
	} 
}