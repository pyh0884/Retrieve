﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour
{
    public bool yes;
    public int DestroyTime=2;
    public void DestroyMeeeeeeeeeeeeeeeeeeeee()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        if (!yes)
        Destroy(gameObject, DestroyTime);
    }
}
