using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeImage : MonoBehaviour
{
    public GameManager gm;
    public Sprite[] spr;
    public Image img;
    public int level;
    public int skill;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {if (gm.levels[skill] < level)
        img.sprite = spr[0];
    else
        img.sprite = spr[1];
    }
}
