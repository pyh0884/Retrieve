using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    public HealthBarControl hbc;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Invincible(float time)
    {
        hbc.invincibleCD = 0.7f-time;
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
