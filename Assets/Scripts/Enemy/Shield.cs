using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
	public GameObject buttonPrefab;
	public List<GameObject> spawnPos;
    public BossHp bh;
    //public int count;
	void Update()
    {

    }
	private void OnEnable()
	{
        //count = spawnPos.Count;
        bh.Shield = true;
		for (int i = 0; i < spawnPos.Count; i++) {
			spawnPos[i].GetComponent<Animator>().SetTrigger("Respawn");
		}
	}
    public void hide()
    {
        bh.Shield = false;
        gameObject.SetActive(false);
    }
}
