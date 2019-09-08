using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleReveal : MonoBehaviour
{
	public float fadespeed = 5f;
	private SpriteRenderer sr;
	private Coroutine c;
    // Start is called before the first frame update
    void Start()
    {
		sr = GetComponent<SpriteRenderer>();
		c = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "PlayerMask") {
			if(c!=null)StopCoroutine(c);
			c = StartCoroutine(FadeInOut(true, fadespeed, sr));
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "PlayerMask")
		{
			if(c!=null)StopCoroutine(c);
			c = StartCoroutine(FadeInOut(false, fadespeed, sr));
		}
	}

	public IEnumerator FadeInOut(bool isFadeOut,float fadeSpeed,SpriteRenderer renderer) {
		if (isFadeOut)
			while (renderer.color.a != 0)
			{
				renderer.color = Vector4.MoveTowards(renderer.color, new Vector4(1, 1, 1, 0), fadeSpeed * Time.deltaTime);
				yield return 0;
			}
		else
			while (renderer.color.a != 1) {
				renderer.color = Vector4.MoveTowards(renderer.color, Vector4.one, fadeSpeed * Time.deltaTime);
				yield return 0;
			}
	}
}
