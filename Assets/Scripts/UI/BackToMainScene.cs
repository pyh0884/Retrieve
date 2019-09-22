using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainScene : Trans
{
    GameManager manager;

    public Animator anim;
    public Vector3 loadPos;
    public GameObject Boss;
    public int num=4;


    // Start is called before the first frame update
    void Update()
    {
        if (!manager) manager = FindObjectOfType<GameManager>();
    }
    IEnumerator load(int numb)
    {
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("FadeOut");
        LoadScene(numb);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" &&Boss==null)
        {
            manager.spawnPos = new Vector3(-4,-2,0);
            //anim.SetTrigger("FadeOut");
            StartCoroutine("load", num);
        }
    }
}
