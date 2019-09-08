using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointCheckPoint : MonoBehaviour
{
	public int sceneIndex;
    // Start is called before the first frame update
    void Start()
    {
		GetScene();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player") {
			UpdateSaveData(transform.position,sceneIndex);
		}
	}

	void GetScene() {
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
	}

	public void UpdateSaveData(Vector3 position, int index) {
		PlayerPrefs.SetFloat("RespwanX", position.x);
		PlayerPrefs.SetFloat("RespwanY", position.y);
		PlayerPrefs.SetInt("RespwanSceneIndex", index);
	}
}
