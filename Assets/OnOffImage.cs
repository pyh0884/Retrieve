using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnOffImage : MonoBehaviour
{
    public Sprite[] spr;
    public Image img; 
    public bool Ischecked;
    // Start is called before the first frame update
    public void ische()
    {
        if (Ischecked)
            Ischecked = false;
        else
            Ischecked = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Ischecked)
        {
            img.sprite = spr[0];
        }
        else
        {
            img.sprite = spr[1];
        }
    }
}
