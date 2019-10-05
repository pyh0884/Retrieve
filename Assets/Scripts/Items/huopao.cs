using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huopao : MonoBehaviour
{
    public GameObject leftBullet;
    public GameObject rightBullet;
    public Transform trans;
    public bool TowardsRight;
public void shoot()
    {if (TowardsRight)
            Instantiate(rightBullet, trans.position, Quaternion.identity);
    else
            Instantiate(leftBullet, trans.position, Quaternion.identity);

    }
}
