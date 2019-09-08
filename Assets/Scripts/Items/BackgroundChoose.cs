using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChoose : MonoBehaviour
{
    public List<GameObject> Background;
    private GameManager manager;
    // Start is called before the first frame update
    private void Awake()
    {
        manager = GameObject.FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (manager.BossIndex > 0)
        {
            Background[0].SetActive(false);
            Background[manager.BossIndex].SetActive(true);
        }
    }
}
