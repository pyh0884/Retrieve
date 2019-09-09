using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour
{
    public bool yes;
    public void DestroyMeeeeeeeeeeeeeeeeeeeee()
    {
        Destroy(gameObject);
    }
    void Update()
    {if (!yes)
        Destroy(gameObject, 2);      
    }
}
