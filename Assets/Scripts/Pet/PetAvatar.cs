using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAvatar : MonoBehaviour
{
	public float biteSpeed = 3.0f;
	Animator anim;
	SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if (transform.up.y < 0) sr.flipY = true;
		else sr.flipY = false;
    }

    public void bite(Transform target)
    {
        StartCoroutine("Bite",target);
    }
    IEnumerator Bite(Transform target)
	{
		while (Vector3.Distance(transform.position, target.position) > 1)
		{
			
			transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * biteSpeed);
			yield return null;
		}
		anim.SetTrigger("Eat");
	}

	public void Eaten() {
		FindObjectOfType<EatColor>().gotcha = true;
		Destroy(gameObject);
	}
}
