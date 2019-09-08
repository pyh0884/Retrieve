using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyManagers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            FindObjectOfType<AudioManager>().Mute("BGM1");
            FindObjectOfType<AudioManager>().Mute("BGM2");
            FindObjectOfType<AudioManager>().Mute("BGM3");
            FindObjectOfType<AudioManager>().Mute("BGM4");
        if (FindObjectOfType<GameManager>() != null)
        {
            Destroy(FindObjectOfType<GameManager>().gameObject);
        }
        if (GameObject.FindWithTag("Player"))
            Destroy(GameObject.FindWithTag("Player"));
    }
}
