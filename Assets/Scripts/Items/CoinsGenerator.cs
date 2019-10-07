using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsGenerator : MonoBehaviour
{
    public bool isChest;
    public GameObject[] coins;
    public int value;

    public void GenCoins()
    {
        while (value > 0)
        {
            var ins1 = Instantiate(coins[Mathf.FloorToInt(Random.Range(0,coins.Length-0.5f))],transform.position,Quaternion.identity);
            ins1.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0, 10) > 5 ? 3 : -3, 7);
            value -= ins1.GetComponentInChildren<CoinAI>().MoneyValue;

        }
        }
    // Update is called once per frame
    void Update()
    {
        
    }
}
