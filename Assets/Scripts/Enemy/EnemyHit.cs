using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public bool selfDestroy=true;
    public GameObject deadBody;
    public int DMG=1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthBarControl>().Damage(DMG);
            //动画，动画事件加入不同的表现+粒子
            if (selfDestroy)
            {
                Destroy(gameObject);
                if (deadBody)
                {
                    Instantiate(deadBody, transform.position, Quaternion.identity);
                }
            }
        }
        if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 12)&&deadBody)
        {
            Instantiate(deadBody, transform.position, Quaternion.identity);
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