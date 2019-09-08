using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 生命值跟伤害处理
/// </summary>
public class HealthPoint : MonoBehaviour
{
    public int hp=1;
    public int hpMax = 100;
    public GameObject mainObject;
    public Animator anim;
    public Text hpText;
    public Slider hpSlider;
    private bool dead=false;
   // private Animator animator;

    public void Damage(int damageCount)
    {
        //Debug.Log("执行了哟");
        if (this.name=="HitBox")
        FindObjectOfType<AudioManager>().Play("Enemy_Hit");
        else
            FindObjectOfType<AudioManager>().Play("Boss_Hit");

        DamageTextControler.CreatDamageText(damageCount.ToString(),mainObject.transform);
        hp -= damageCount;
        
    }

    public void DieStart(GameObject obj)
    {
        anim.SetTrigger("Die");
    }

    public void Dying()
    {

    }

    public void DieEnd()
    {

    }

    void Awake()
    {
        hp = hpMax;
        anim = mainObject.GetComponent<Animator>();
        DamageTextControler.Initialize();
    }

    void Update()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);
        if (hp <= 0&&dead==false)
        {
            // TODO 帧动画事件淡出+destroy 
            // destroy the object or play the dead animation
            
            DieStart(mainObject);
            dead = true;
        }
        hpText.text = hp+" / "+hpMax;
        hpSlider.value = (float)hp / hpMax;
    }
}
