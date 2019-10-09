using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exinrende : MonoBehaviour
{
	bool start;
	bool input;
	public int num;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (start) {
			switch (num) {
				case 0: input=Input.GetButtonDown("Jump");break;
				case 1: input = Input.GetButtonDown("Fire2");break;
				case 2: input = Input.GetButtonDown("Fire1")||Input.GetAxisRaw("Fire1")>0.5f;break;
			}
			if (input)
			{
				Time.timeScale = 1;
				Destroy(this);
			}
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Time.timeScale = 0;
			start = true;
		}
	}
}
