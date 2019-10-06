using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPeddleVice : MonoBehaviour
{
	GameObject main;
	[Header("击退")]
	public float hitBackDist = -6;
	public float hitBackSpeed = 40;
	public int maxBackFrame = 20;
    public GameObject deadBody;

	Animator anim;

    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
		main = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		if (main == null) Destroy(gameObject);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PlayerAttack"))
		{
			StartCoroutine(GotHit(transform));
			anim.SetTrigger("Die");
		}
        if (collision.tag == "Player")
        {
                Destroy(gameObject);
                if (deadBody)
                {
                    Instantiate(deadBody, transform.position, Quaternion.identity);
                }
            
        }
        if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 12))
        {
            if(deadBody)
            Instantiate(deadBody, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

	IEnumerator GotHit(Transform curr)
	{
		Vector3 target;
		int frameCount = 0;
		target = curr.position + curr.right * hitBackDist;
		//if (right) target = transform.position + new Vector3(hitBackDist, 0, 0);
		//else target = transform.position + new Vector3(-hitBackDist, 0, 0);
		while (frameCount < maxBackFrame)
		{
			curr.position = Vector3.MoveTowards(curr.position, target, hitBackSpeed * Time.deltaTime);
			yield return null;
			frameCount++;
		}
	}
}
