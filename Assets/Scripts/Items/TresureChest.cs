using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureChest : MonoBehaviour
{
	public int chestIndex;

	public void Start()
	{
		if (PlayerPrefs.HasKey("TresuareChestOpened"+chestIndex))
		{
			if(PlayerPrefs.GetInt("TresuareChestOpened" + chestIndex,0)>0)
			Destroy(gameObject);
		}
	}

	public void SaveOpenState() {
		PlayerPrefs.SetInt("TresuareChestOpened" + chestIndex, 1);
	}
}
