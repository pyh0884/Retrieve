using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicPlayer : MonoBehaviour
{
	public Animator anim;
    AudioManager am;
    public void FadeOut()
	{
		anim.SetTrigger("FadeOut");
	}
    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        am.Mute("PlayerRun");

    }
}
