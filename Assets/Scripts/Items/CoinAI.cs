using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAI : MonoBehaviour
{
    GameManager gm;
    public int MoneyValue = 1;
    private bool Enter;
    public float speed=3;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
    }
    void MoveToWards()
    {
        transform.parent.transform.position = Vector3.Lerp(transform.parent.transform.position, GameObject.Find("PlayerTransform").transform.position, Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Enter = true;
            gm.GetMoney(MoneyValue);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Enter)
        {
            MoveToWards();

            if (Mathf.Abs(transform.parent.transform.position.x-GameObject.Find("PlayerTransform").transform.position.x)<1)
            {
                GetComponentInParent<Rigidbody2D>().velocity = Vector3.zero;
                anim.SetTrigger("Hit");
                Destroy(gameObject.transform.parent.gameObject, 1);
            }
        }
    }
}
