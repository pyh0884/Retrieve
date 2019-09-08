using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill5 : MonoBehaviour
{
    public int HealAmount=0;
    public void Heal()
    {
        gameObject.GetComponentInParent<HealthBarControl>().Damage(HealAmount);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
