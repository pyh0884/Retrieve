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
            ins1.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0, 10) > 5 ? Random.Range(2,4) : -Random.Range(2, 4), Random.Range(5, 7));
            value -= ins1.GetComponentInChildren<CoinAI>().MoneyValue;

        }
        }
    public void GenCoinsInChest()
    {
        while (value > 0)
        {
            var ins1 = Instantiate(coins[Mathf.FloorToInt(Random.Range(0, coins.Length - 0.5f))], transform.position, Quaternion.identity);
            ins1.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0, 10) > 5 ? Random.Range(1, 7) : -Random.Range(1, 7), Random.Range(3, 10));
            value -= ins1.GetComponentInChildren<CoinAI>().MoneyValue;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isChest && collision.tag == "PlayerAttack")
        {

               GetComponent<Animator>().SetTrigger("Open");

        }
    }
    public void des()
    {
        Destroy(gameObject);
    }
}
