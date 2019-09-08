using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureChest : MonoBehaviour
{
	public GameManager manager;
	public int chestIndex;
	public GameObject content;
	public GameObject opened;

	private void Awake()
	{
		if (!manager) manager = FindObjectOfType<GameManager>();
	}

	private void Start()
	{
		if (manager.TreasureChestOpened[chestIndex])
		{
			Instantiate(opened, transform.position, new Quaternion());
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "PlayerAttack") {
			Instantiate(opened, transform.position, new Quaternion());
			Instantiate(content, transform.position, new Quaternion());
			manager.TreasureChestOpened[chestIndex] = true;
			Destroy(gameObject);
		}
	}
}
