using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextControler : MonoBehaviour
{
    private static GameObject canv;
    private static DamageText damageText;
    public static void CreatDamageText(string str, Transform position,int DMGtype)
    {
        DamageText exp = Instantiate(damageText);
        if (exp!=null) {
            exp.transform.SetParent(canv.transform, false);
            exp.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(position.position);
            exp.SetText(str);
            switch (DMGtype)
            {
                case 1:
                    exp.GetComponentInChildren<Animator>().SetTrigger(Random.Range(0, 10) > 5 ? "Cri1": "Cri2") ;
                    break;
                case 2:
                    exp.GetComponentInChildren<Animator>().SetTrigger(Random.Range(0, 10) > 5 ? "DMG1":"DMG2");
                    break;
                case 3:
                    exp.GetComponentInChildren<Animator>().SetTrigger(Random.Range(0, 10) > 5 ? "Dot1":"Dot2");
                    break;
            }
            
        }
    }

    public static void Initialize()
    {
        canv = GameObject.Find("BossCanvas");
        if (damageText==null)
        {
            damageText = Resources.Load<DamageText>("DamageTextParent");
        }
    }
}
