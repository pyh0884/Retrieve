using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour
{
    public HealthBarControl hbc;
    GameManager gm;
    public int HealPerLevel = 1;
    public bool enter;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        hbc = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBarControl>();
        StartCoroutine("heal");
    }

    IEnumerator heal()
    {
        while (true)
        {
            if (enter)
            {
                hbc.Damage(-HealPerLevel * (gm.levels[2] + 1));
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enter = false;
        }
    }
}
