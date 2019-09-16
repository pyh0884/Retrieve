 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill8Balls : MonoBehaviour
{
    Rigidbody2D rb;
    public GameManager gm;
    Vector2 speed;
    public float IniSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        SetRandomVelocity();
        Destroy(gameObject, 2.5f);
    }
    void SetRandomVelocity()
    {
        float tempAngle = Random.Range(Mathf.Deg2Rad * 40f, Mathf.Deg2Rad * 60f);
        int temp = (int)Random.Range(0, 2);
        Vector2 dir = new Vector2((temp == 0 ? 1 : -1) * Mathf.Cos(tempAngle), Mathf.Sin(tempAngle));
        rb.velocity = dir* IniSpeed;
    }
    private void ControlDirection()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, (rb.velocity.x >= 0) ? -Vector2.Angle(Vector2.up, rb.velocity) : Vector2.Angle(Vector2.up, rb.velocity));

    }
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
        speed *= IniSpeed;
        rb.velocity = speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10)) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10))));
            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10)) * 1.5f), 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 10))));
            }
        }
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
        }
    }
        void Update()
    {
        ControlDirection();
        setAngle();
    }
}
