using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill5 : MonoBehaviour
{
    public int HealAmount=0;
    public GameObject HealArea;
    GameManager gm;
    public int HealPerLevel = 10;
    public void Heal()
    {
        gameObject.GetComponentInParent<HealthBarControl>().Damage(HealAmount);
        Instantiate(HealArea,new Vector3(transform.position.x, transform.position.y-2),Quaternion.identity);
    }
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        HealAmount = -30 - (HealPerLevel * gm.levels[2]);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
