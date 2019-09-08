using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	private GameObject player;
	private bool found;
	// Start is called before the first frame update
	private void Awake()
	{
		found = false;
		player = null;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (!found)
		{
			if (!player) { player = GameObject.FindWithTag("Player"); }
			else { transform.SetParent(player.transform); transform.localPosition = Vector3.zero; found = true; }
		}
    }
}
