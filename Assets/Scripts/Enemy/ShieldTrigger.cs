using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrigger : MonoBehaviour
{
    //public Shield sd;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("PlayerAttack"))
        {
            //sd.count -= 1;
            GetComponent<Animator>().SetTrigger("Die");
            anim.SetTrigger("Hit");
        }
	}
}
