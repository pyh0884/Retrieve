using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsGenerator : MonoBehaviour
{
    public bool isChest;
    public GameObject[] coins;
    public int value;
	public float minimumX = 1;
	public float maxmumX = 7;
	public float minimumY = 3;
	public float maxmumY = 10;

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
		float MinimumX = minimumX / maxmumX;
		float MinimumY = minimumY / maxmumY;
		while (value > 0)
		{
			float rand_VelocityX = -Mathf.Abs(Mathf.Sqrt(-2 * Mathf.Log(Random.value)) * Mathf.Sin(2 * Mathf.PI * Random.value));
			float rand_VelocityY = Mathf.Abs(Mathf.Sqrt(-2 * Mathf.Log(Random.value)) * Mathf.Sin(2 * Mathf.PI * Random.value));
			var ins1 = Instantiate(coins[Mathf.FloorToInt(Random.Range(0, coins.Length - 0.5f))], transform.position, Quaternion.identity);
			ins1.GetComponent<Rigidbody2D>().velocity =
				/*new Vector2(Random.Range(0, 10) > 5 ? Random.Range(1, 7) : -Random.Range(1, 7), Random.Range(3, 10));*/
				new Vector2((rand_VelocityX * (1 - MinimumX) + MinimumX) * (Random.value > 0.5f ? maxmumX : -maxmumX), (rand_VelocityY * (1 - MinimumY) + MinimumY) * maxmumY);
			value -= ins1.GetComponentInChildren<CoinAI>().MoneyValue;
			//hahaha
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
