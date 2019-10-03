using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill9 : MonoBehaviour
{
    public GameManager gm;
    public LayerMask enemyLayer;
    Collider2D nearest;
    public float LastTime;
    public GameObject MainObj;
    public Transform paokou;
    private Vector3 direction;
    public GameObject Bullet;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Destroy(MainObj,6);
    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, 20, enemyLayer);
        if (list.Length == 0)
        {
            nearest = null;
        }
        else
        {
            nearest = list[0];
            foreach (Collider2D col in list)
            {
                if (Vector2.Distance(new Vector2(col.transform.position.x, col.transform.position.y), new Vector2(gameObject.transform.position.x, col.transform.position.y)) <= Vector2.Distance(new Vector2(nearest.transform.position.x, nearest.transform.position.y), new Vector2(gameObject.transform.position.x, col.transform.position.y)))
                    nearest = col;
            }
        }
    }
    public void shoot()
    {
        GameObject obj = Instantiate(Bullet,paokou.position,Quaternion.FromToRotation(transform.position,direction));
        obj.transform.up = direction;
        obj.GetComponent<Rigidbody2D>().velocity = obj.transform.up * 10;
    }


    private void Update()
    {
        FindEnemy();
        if (nearest)
        {
            GetComponent<Animator>().speed = 1;
            direction = nearest.transform.position - transform.position;
            transform.right = direction;
        }
        else
        {
            GetComponent<Animator>().speed = 0;
        }
    }
}
