using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public bool selfDestroy=true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthBarControl>().Damage(1);
            //动画，动画事件加入不同的表现+粒子
            if (selfDestroy)
            Destroy(gameObject);
        }
    }
	public void SelfDestroy()
	{
		Destroy(gameObject);
	}
    public void Dici()
    {
        FindObjectOfType<AudioManager>().Play("Dici");
    }
}