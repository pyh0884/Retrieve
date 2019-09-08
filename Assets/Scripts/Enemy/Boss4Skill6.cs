using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4Skill6 : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 speed;
    //public Animator CameraAnim;
    public float BallVelocity=5;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-3,-3);
    }

    private void ControlDirection()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, (rb.velocity.x >= 0) ? -Vector2.Angle(Vector2.up, rb.velocity) : Vector2.Angle(Vector2.up, rb.velocity));

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //        Debug.Log(collision);
    //    CameraAnim.SetTrigger("Shake");
    //    GetComponent<Animator>().SetTrigger("Hit");
    //}

    void setAngle()
    {
        speed = rb.velocity;
        float angle = Vector2.Angle(speed.x > 0 ? Vector2.right : Vector2.left, speed);
        if (angle > 0)
            angle = Mathf.Clamp(angle, 30, 70);
        else
            angle = Mathf.Clamp(angle, -70, -30);
        //Debug.Log(Mathf.Cos(angle*Mathf.Deg2Rad) + " " + Mathf.Sin(angle * Mathf.Deg2Rad));
        speed.x = Mathf.Sign(speed.x) * Mathf.Cos(angle * Mathf.Deg2Rad);
        speed.y = Mathf.Sign(speed.y) * Mathf.Sin(angle * Mathf.Deg2Rad);
        speed *= BallVelocity;
        rb.velocity = speed;


    }
    private void Update()
    {
        ControlDirection();
        setAngle();

    }
}
