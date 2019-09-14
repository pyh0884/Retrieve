using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Trans : MonoBehaviour
{
    public GameObject PauseMenu;
    public void LoadScene(int number) 
    {
        StartCoroutine(LoadSceneFunction(number));      
    }

    IEnumerator LoadSceneFunction(int number) 
    {

        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }

        Time.timeScale = 1;
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(number);
        yield return new WaitForSeconds (0.7f);
    }

    public void QuickLoad(int number)
    {
        SceneManager.LoadScene(number);
    }

	public void ClearSaveData() {
		if (PlayerPrefs.HasKey("YellowBossBeaten"))PlayerPrefs.DeleteKey("YellowBossBeaten");
		if (PlayerPrefs.HasKey("BlueBossBeaten"))PlayerPrefs.DeleteKey("BlueBossBeaten");
		if (PlayerPrefs.HasKey("GreenBossBeaten"))PlayerPrefs.DeleteKey("GreenBossBeaten");
		if (PlayerPrefs.HasKey("RespwanX"))PlayerPrefs.DeleteKey("RespwanX");
		if (PlayerPrefs.HasKey("RespwanY"))PlayerPrefs.DeleteKey("RespwanY");
		if (PlayerPrefs.HasKey("RespwanSceneIndex"))PlayerPrefs.DeleteKey("RespwanSceneIndex");
		for (int i = 0; i <= 30; i++) {
			if (PlayerPrefs.HasKey("TresuareChestOpened" + i)) {
				PlayerPrefs.DeleteKey("TresuareChestOpened" + i);
			}
		}
	}
}
