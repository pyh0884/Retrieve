using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAvatar : MonoBehaviour
{
	public float biteSpeed = 3.0f;
	Animator anim;
	SpriteRenderer sr;
    SpriteRenderer Main;
    private GameObject xise;
    EatColor ea;
    PetAI pa;
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
        sr = FindObjectOfType<PetAI>().GetComponent<SpriteRenderer>();
        Main = GetComponent<SpriteRenderer>();
        ea = FindObjectOfType<EatColor>();
        pa = FindObjectOfType<PetAI>();
        sr.enabled = false;
        Destroy(gameObject, 1.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.up.y < 0) Main.flipY = true;
        else Main.flipY = false;
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
    public void show()
    {
        sr.enabled = true;
        pa.CanAttack = true;
        Destroy(gameObject);
    }
    public void Eaten() {
        if (xise)
        {
            switch (xise.tag)
            {
                case "Red":
                    Destroy(xise);
                    ea.EatRed();
                    break;
                case "Green":
                    Destroy(xise);
                    ea.EatGreen();
                    break;
                case "Blue":
                    Destroy(xise);
                    ea.EatBlue();
                    break;
                case "Yellow":
                    Destroy(xise);
                    ea.EatYellow();
                    break;
                default:
                    break;
            }
        }
	}
}
