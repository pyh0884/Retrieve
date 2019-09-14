using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChoose : MonoBehaviour
{
	public List<GameObject> Background_Green;
	public List<GameObject> Background_Green_G;
	public List<GameObject> Background_Yellow;
	public List<GameObject> Background_Yellow_G;
	public List<GameObject> Background_Blue;
	public List<GameObject> Background_Blue_G;
    // Start is called before the first frame update
    private void Awake()
    {
		if (PlayerPrefs.HasKey("YellowBossBeaten"))
		{
			if (PlayerPrefs.GetInt("YellowBossBeaten",0) > 0)
			{
				for (int i = 0; i < Background_Yellow.Count; i++)
				{
					Background_Yellow[i].SetActive(true);
				}
				for (int i = 0; i < Background_Yellow_G.Count; i++)
				{
					Background_Yellow_G[i].SetActive(false);
				}
			}
		}
		if (PlayerPrefs.HasKey("GreenBossBeaten"))
		{
			if (PlayerPrefs.GetInt("GreenBossBeaten",0) > 0)
			{
				for (int i = 0; i < Background_Green.Count; i++)
				{
					Background_Green[i].SetActive(true);
				}
				for (int i = 0; i < Background_Green_G.Count; i++)
				{
					Background_Green_G[i].SetActive(false);
				}
			}
		}
		if (PlayerPrefs.HasKey("BlueBossBeaten"))
		{
			if (PlayerPrefs.GetInt("BlueBossBeaten",0) > 0)
			{
				for (int i = 0; i < Background_Blue.Count; i++)
				{
					Background_Blue[i].SetActive(true);
				}
				for (int i = 0; i < Background_Blue_G.Count; i++)
				{
					Background_Blue_G[i].SetActive(false);
				}
			}
		}
	}
}
