using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//传送门
public class LoadCheck : Trans
{
	public Vector3 loadPos;
	public int sceneIndex;
	GameManager manager;
    public bool BossPortol;
	public int bossIndex;
	public Animator anim;
	private bool[] BossBeaten=new bool[3];
	private void Start()
	{
		if (PlayerPrefs.GetInt("GreenBossBeaten", 0) > 0)
		{
			BossBeaten[1] = true;
		}
		if (PlayerPrefs.GetInt("YellowBossBeaten", 0) > 0)
		{
			BossBeaten[0] = true;
		}
		if (PlayerPrefs.GetInt("BlueBossBeaten", 0) > 0)
		{
			BossBeaten[2] = true;
		}

	}
	void Update()
    {
        if(!manager) manager = FindObjectOfType<GameManager>();
		if (!anim) anim = GameObject.Find("转场动画").GetComponentInChildren<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player"&&(((!BossBeaten[bossIndex])&&BossPortol)||(!BossPortol)))
        {
			manager.spawnPos = loadPos;
            anim.SetTrigger("FadeOut");
            LoadScene(sceneIndex);
		}
    }
}
