using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class destroyPet : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    //GameObject[] pets;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 15)
        {
            Destroy(obj1);
            Destroy(obj2);
            Destroy(obj3);
        }
        //Debug.Log(pets[0].name);
        //Destroy(pets[0]);
        //Destroy(pets[1]);

        //Destroy(pets[2]);

    }


}
