using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion5AI : MonoBehaviour
{
    public Vector2 speed;
    public Vector2 direction;
    Rigidbody2D rd;
    public GameObject GroundCheck;
    public bool Grounded;
    public LayerMask groundLayer;
    public Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        movement = new Vector2(speed.x * direction.x, 0);
    }

    void WanderEnemyMovement()
    {
        
        if (!isGround)
        {
            if (direction.x ==-1)
            {
               rd.transform.eulerAngles = new Vector3(0,0,0);
            }
            else if (direction.x==1)
            {
                rd.transform.eulerAngles = new Vector3(0, -180, 0);
            }
            direction.x = -direction.x;
            movement = new Vector2(speed.x * direction.x, 0);
        }
        rd.velocity = movement;
    }

    bool isGround
    {
        get
        {
            Vector2 start = GroundCheck.transform.position;
            //Vector2 end = new Vector2(start.x + direction.x > 0 ? distance:-distance,start.y);
            Vector2 end = new Vector2(GroundCheck.transform.position.x, GroundCheck.transform.position.y-2);
            // = new Vector2(start.x + direction.x > 0 ? distance * 15: -distance * 15,start.y);
            Debug.DrawLine(start, end, Color.blue);
            Grounded = Physics2D.Linecast(start, end, groundLayer);
            return Grounded;
        }
    }
    void Update()
    {
        WanderEnemyMovement();
    }
}
