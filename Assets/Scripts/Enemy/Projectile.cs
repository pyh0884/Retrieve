using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Coroutines;

public class Projectile
	: MonoBehaviour
{
	public Vector3 InitialForce;
	public int damageAmt = 1;
	public bool selfDestroy = true;

	Coroutines.Coroutine _Main;

	// Use this for initialization
	void Start ()
	{
		_Main = new Coroutines.Coroutine(Main());
	}
	
	// Update is called once per frame
	void Update ()
	{
		_Main.Update();
	}

	System.Action<Collider2D> _OnTriggerEnter;
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (_OnTriggerEnter != null)
		{
			_OnTriggerEnter(collision);
		}
	}

	IEnumerable<Instruction> Main()
	{
		// Apply an initial force
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.velocity = InitialForce;

		while (true)
		{
			Collider2D collidedWith = null;
			_OnTriggerEnter = col => collidedWith = col;
			try
			{
				while (collidedWith == null)
				{
					yield return null;
				}
			}
			finally
			{
				_OnTriggerEnter = null;
			}

			if (collidedWith.CompareTag("Player"))
			{
				collidedWith.GetComponent<HealthBarControl>().Damage(damageAmt);
				if (selfDestroy) Destroy(gameObject);
			}
            else if(collidedWith.gameObject.layer==8|| collidedWith.gameObject.layer == 12)
            {
                Destroy(gameObject);
            }
			// Else just bounce/roll/etc...
		}
	}
}
