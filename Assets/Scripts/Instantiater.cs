using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiater : MonoBehaviour
{
    public List<GameObject> monsters;
    // Start is called before the first frame update
    void Start()
    {
		for (int i = 0; i < monsters.Count; i++) Instantiate(monsters[i], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
