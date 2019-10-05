using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huopaoBullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed=10;
    public bool TowardsRight;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
        rb.velocity = new Vector2(TowardsRight ? speed : -speed, 0);
    }
}
