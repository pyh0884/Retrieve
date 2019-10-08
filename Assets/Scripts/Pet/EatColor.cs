﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatColor : MonoBehaviour
{
    public Image[] ElementsImages;
    public Sprite[] ElementsSprites;
    public Sprite[] SkillSprites;
    public int[] elements= {0,0,0};
    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Yellow")
        {
            if (elements[0] == 0)
            {
                Debug.Log("Y1");
                elements[0] = 1;
            }
            else if (elements[2] == 0)
            {
                Debug.Log("Y2");

                elements[2] = 1;
            }
            else
            {
                Debug.Log("Y3");
                elements[0] = elements[2];
                elements[2] = 1;
            }
        }
        if (collision.tag == "Green")
        {
            Debug.Log("G");

            if (elements[0] == 0)
                elements[0] = 2;
            else if (elements[2] == 0)
                elements[2] = 2;
            else
            {
                elements[0] = elements[2];
                elements[2] = 2;
            }

        }
        if (collision.tag == "Blue")
        {
            Debug.Log("B");

            if (elements[0] == 0)
                elements[0] = 3;
            else if (elements[2] == 0)
                elements[2] = 3;
            else
            {
                elements[0] = elements[2];
                elements[2] = 3;
            }

        }
        if (collision.tag == "Red")
        {
            Debug.Log("R");

            if (elements[0] == 0)
                elements[0] = 4;
            else if (elements[2] == 0)
                elements[2] = 4;
            else
            {
                elements[0] = elements[2];
                elements[2] = 4;
            }

        }
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                collision.gameObject.GetComponent<BossHp>().Damage2(1, 2);
            }
            else
            {
                collision.gameObject.GetComponent<MonsterHp>().Damage2(1, 2);
            }
            chooseSkill();
        }
    }
    public void EatYellow()
    {
        if (elements[0] == 0)
        {
            Debug.Log("Y1");
            elements[0] = 1;
        }
        else if (elements[2] == 0)
        {
            Debug.Log("Y2");

            elements[2] = 1;
        }
        else
        {
            Debug.Log("Y3");
            elements[0] = elements[2];
            elements[2] = 1;
        }

    }
    public void EatGreen()
    {
                    Debug.Log("G");

            if (elements[0] == 0)
                elements[0] = 2;
            else if (elements[2] == 0)
                elements[2] = 2;
            else
            {
                elements[0] = elements[2];
                elements[2] = 2;
            }


    }
    public void EatBlue()
    {
        Debug.Log("B");

        if (elements[0] == 0)
            elements[0] = 3;
        else if (elements[2] == 0)
            elements[2] = 3;
        else
        {
            elements[0] = elements[2];
            elements[2] = 3;
        }

    }
    public void EatRed()
    {
        Debug.Log("R");

        if (elements[0] == 0)
            elements[0] = 4;
        else if (elements[2] == 0)
            elements[2] = 4;
        else
        {
            elements[0] = elements[2];
            elements[2] = 4;
        }

    }

    void chooseSkill()
    {
        switch (elements[0])
        {
            case 1:
                switch (elements[2])
                {
                    case 1:
                        elements[1] = 1;
                        break;
                    case 2:
                        elements[1] = 2;
                        break;
                    case 3:
                        elements[1] = 3;
                        break;
                    case 4:
                        elements[1] = 4;
                        break;
                    case 0:
                        elements[1] = 0;
                        break;
                }
                break;
            case 2:
                switch (elements[2])
                {
                    case 1:
                        elements[1] = 2;
                        break;
                    case 2:
                        elements[1] = 5;
                        break;
                    case 3:
                        elements[1] = 6;
                        break;
                    case 4:
                        elements[1] = 7;
                        break;
                    case 0:
                        elements[1] = 0;
                        break;
                }
                break;
            case 3:
                switch (elements[2])
                {
                    case 1:
                        elements[1] = 3;
                        break;
                    case 2:
                        elements[1] = 6;
                        break;
                    case 3:
                        elements[1] = 8;
                        break;
                    case 4:
                        elements[1] = 9;
                        break;
                    case 0:
                        elements[1] = 0;
                        break;
                }
                break;
            case 4:
                switch (elements[2])
                {
                    case 1:
                        elements[1] = 4;
                        break;
                    case 2:
                        elements[1] = 7;
                        break;
                    case 3:
                        elements[1] = 9;
                        break;
                    case 4:
                        elements[1] = 10;
                        break;
                    case 0:
                        elements[1] = 0;
                        break;
                }
                break;
            case 0:
                elements[1] = 0;
                break;
        }
    }
    void Awake()
    {
        elements = FindObjectOfType<GameManager>().ELEMENTS;
    }
    void Update()
    {
        switch (elements[0])
        {
            case 0:
                ElementsImages[0].sprite = ElementsSprites[0];
                anim.SetInteger("First", 0);
                break;
            case 1:
                anim.SetInteger("First", 1);
                ElementsImages[0].sprite = ElementsSprites[2];
                break;
            case 2:
                anim.SetInteger("First", 2);
                ElementsImages[0].sprite = ElementsSprites[4];
                break;
            case 3:
                anim.SetInteger("First", 3);
                ElementsImages[0].sprite = ElementsSprites[6];
                break;
            case 4:
                anim.SetInteger("First", 4);
                ElementsImages[0].sprite = ElementsSprites[8];
                break;
        }
        switch (elements[1])
        {
            case 0:
                ElementsImages[1].sprite = SkillSprites[10];
                break;
            case 1:
                ElementsImages[1].sprite = SkillSprites[0];
                break;
            case 2:
                ElementsImages[1].sprite = SkillSprites[1];
                break;
            case 3:
                ElementsImages[1].sprite = SkillSprites[2];
                break;
            case 4:
                ElementsImages[1].sprite = SkillSprites[3];
                break;
            case 5:
                ElementsImages[1].sprite = SkillSprites[4];
                break;
            case 6:
                ElementsImages[1].sprite = SkillSprites[5];
                break;
            case 7:
                ElementsImages[1].sprite = SkillSprites[6];
                break;
            case 8:
                ElementsImages[1].sprite = SkillSprites[7];
                break;
            case 9:
                ElementsImages[1].sprite = SkillSprites[8];
                break;
            case 10:
                ElementsImages[1].sprite = SkillSprites[9];
                break;


        }
        switch (elements[2])
        {
            case 0:
                anim.SetInteger("Second", 0);
                ElementsImages[2].sprite = ElementsSprites[1];
                break;
            case 1:
                anim.SetInteger("Second", 1);
                ElementsImages[2].sprite = ElementsSprites[3];
                break;
            case 2:
                anim.SetInteger("Second", 2);
                ElementsImages[2].sprite = ElementsSprites[5];
                break;
            case 3:
                anim.SetInteger("Second", 3);
                ElementsImages[2].sprite = ElementsSprites[7];
                break;
            case 4:
                anim.SetInteger("Second", 4);
                ElementsImages[2].sprite = ElementsSprites[9];
                break;
        }
        chooseSkill();
    }
}
