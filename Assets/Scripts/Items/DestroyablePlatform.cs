using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyablePlatform : MonoBehaviour
{
    public float timer;
    private float Timer;
    bool des;

    // Start is called before the first frame update
    void Start()
    {
        des = false;
        Timer = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("DestroyPlatform");
        }
    }
    IEnumerator DestroyPlatform()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Timer = 0;
        des = true;
    }
    // Update is called once per frame
    void Update()
    {if (des)
        {
            Timer += Time.deltaTime;
            if (Timer > timer)
            {
                GetComponentInChildren<SpriteRenderer>().enabled = true;
                GetComponent<Collider2D>().enabled = true;
                des = false;
            }
        }
    }
}
