using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuck : MonoBehaviour
{
    public PlayerController pc;
    public Climb cb;
    public ClimbDown cbd;
    private float timer;
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pc.enabled && !cb.enabled && !cbd.enabled)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 0.25f)
        {
            pc.enabled = true;
        }
    }
}
