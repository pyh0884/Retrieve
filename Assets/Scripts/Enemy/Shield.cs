using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
	public GameObject buttonPrefab;
	public List<Transform> spawnPos;
	// Start is called before the first frame update
	// Update is called once per frame
	void Update()
    {
		int temp = 0;
		for (int i = 0; i < spawnPos.Count; i++) temp += spawnPos[i].childCount;
		if (temp==0) gameObject.SetActive(false);
    }
	private void OnEnable()
	{
		for (int i = 0; i < spawnPos.Count; i++) {
			Instantiate(buttonPrefab, spawnPos[i],false);
		}
	}
}
