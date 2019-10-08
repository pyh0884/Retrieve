using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangAn : MonoBehaviour
{
	public int targetSceneIndex;
	public float Ping;
	private bool IsStart = false;
	private bool go = false;
	private float LastTime = 0;
	public Image image;
	public RawImage raw;
	void Update()
    {		
		if (IsStart && Ping > 0 && LastTime > 0 )
		{
			image.fillAmount = (Time.time - LastTime) / Ping;
			if (Time.time - LastTime > Ping)
			{
				Debug.Log("长按触发");
				GetComponent<ComicPlayer>().FadeOut();
				GetComponent<Trans>().LoadScene(targetSceneIndex);
				go = true;
				IsStart = false;
				LastTime = 0;
			}
			
		}
		if(!IsStart&&!go) image.fillAmount = Mathf.Lerp(image.fillAmount, 0, Time.deltaTime * 5);
		checkHold();
	}

	void checkHold() {
		if (Input.GetButtonDown("Submit")) LongPress(true);
		if (Input.GetButtonUp("Submit")) LongPress(false);
	}

	public void LongPress(bool bStart)
	{
		IsStart = bStart;
		if (IsStart)
		{
			LastTime = Time.time;
			Debug.Log("长按开始");
		}
		else if (LastTime != 0)
		{
			LastTime = 0;			
			Debug.Log("长按取消");
		}
	}

	private void Awake()
	{
		if (raw != null)
		{
			bool y = PlayerPrefs.GetInt("YellowBossBeaten", 0) != 0;
			bool g = PlayerPrefs.GetInt("GreenBossBeaten", 0) != 0;
			bool b = PlayerPrefs.GetInt("BlueBossBeaten", 0) != 0;
			raw.texture = Resources.Load("Jitan/" + (y ? "Y" : "") + (g ? "G" : "") + (b ? "B" : ""), typeof(Texture)) as Texture;
		}
	}
}
