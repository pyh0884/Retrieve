using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicPlayer : MonoBehaviour
{
	public Animator anim;
	public void FadeOut()
	{
		anim.SetTrigger("FadeOut");
	}
}
