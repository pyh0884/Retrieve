using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiater : MonoBehaviour
{
    public List<GameObject> monsters;
    GameManager gm;
    public float Pos;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Pos = gm.ElitePos;
            if (Random.Range(0, 100) >= Pos)
            {
                Instantiate(monsters[0], transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(monsters[1], transform.position, Quaternion.identity);
                Destroy(gameObject);

            }
    }

}
