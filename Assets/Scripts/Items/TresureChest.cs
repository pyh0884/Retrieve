using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureChest : MonoBehaviour
{
	public int chestIndex;
	GameManager gm;

	public void Start()
	{
		gm = FindObjectOfType<GameManager>();
		if (PlayerPrefs.HasKey("TresuareChestOpened"+chestIndex))
		{
			if(PlayerPrefs.GetInt("TresuareChestOpened" + chestIndex,0)>0)
			Destroy(gameObject);
		}
	}

	public void SaveOpenState() {
		PlayerPrefs.SetInt("TresuareChestOpened" + chestIndex, 1);
		PlayerPrefs.SetFloat("CRIT", gm.CRIT);
		PlayerPrefs.SetInt("MAXHP", gm.MAXHP);
		PlayerPrefs.SetInt("DAMAGE", gm.DAMAGE);
	}
}
