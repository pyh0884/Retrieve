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
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PlayerAttack"))
		{
			anim.SetTrigger("Die");
		}
        if (collision.CompareTag("Player"))
        {
            if (deadBody)
            {
				Instantiate(deadBody, transform.position, Quaternion.identity);
            }
			anim.SetTrigger("Die");
		}
		if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 12))
		{
			if (deadBody)Instantiate(deadBody, transform.position, Quaternion.identity);
			anim.SetTrigger("Die");
		}
	}
}
