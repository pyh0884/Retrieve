using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAvatar : MonoBehaviour
{
	public float biteSpeed = 3.0f;
	Animator anim;
	SpriteRenderer sr;
    GameObject xise;
    public EatColor ea;
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
        ea = FindObjectOfType<EatColor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.up.y < 0) sr.flipY = true;
        else sr.flipY = false;
    }

    public void bite(Transform target)
    {
        xise = target.gameObject;

        StartCoroutine(Bite(target));
    }
    public IEnumerator Bite(Transform target)
	{

        while (Vector3.Distance(transform.position, target.position) > 1)
		{
			
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y+0.3f), Time.deltaTime * biteSpeed);
			yield return null;
		}
		anim.SetTrigger("Eat");
	}
    public void chooseColor()
    {
        switch (xise.tag)
        {
            case "Red":
                ea.EatRed();
                break;
            case "Green":
                ea.EatGreen();
                break;
            case "Blue":
                ea.EatBlue();
                break;
            case "Yellow":
                ea.EatYellow();
                break;
            default:
                break;
        }
    }
    public void Eaten() {
        
		Destroy(gameObject);
	}
}
