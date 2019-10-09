using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clickable : MonoBehaviour
{
    public GameManager gm;
    public int skill = 0;
    public Button but;
    int level;
    void Start()
    {

        but = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gm.levels[skill])
        {
            case 0:
                level = 50;
                break;
            case 1:
                level = 150;

                break;
            case 2:
                level = 300;
                break;
            default:
                level = 50000;
                break;
        }
        if (gm.money < level)
        {
            but.interactable = false;
        }
        else { but.interactable = true; }
    }
}
