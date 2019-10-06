using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAI : MonoBehaviour
{
    public bool selfDestroy = true;
    public int DMG=1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthBarControl>().Damage(DMG);
            //动画，动画事件加入不同的表现+粒子
            if (selfDestroy)
                Destroy(gameObject);
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
