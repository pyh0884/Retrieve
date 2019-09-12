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
    void Update()
    {
        if(!manager) manager = FindObjectOfType<GameManager>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player"&&(((!manager.BossBeaten[bossIndex])&&BossPortol)||(!BossPortol)))
        {
			manager.spawnPos = loadPos;
            anim.SetTrigger("FadeOut");
            LoadScene(sceneIndex);
		}
    }
}
