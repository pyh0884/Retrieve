using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiater : MonoBehaviour
{
    public GameObject[] monsters;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(monsters[0], transform.position, Quaternion.identity);
        Instantiate(monsters[1], transform.position, Quaternion.identity);
        Instantiate(monsters[2], transform.position, Quaternion.identity);
        Instantiate(monsters[3], transform.position, Quaternion.identity);
        Instantiate(monsters[4], transform.position, Quaternion.identity);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
