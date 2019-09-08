using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Skill1 : MonoBehaviour
{
    public GameObject Echo;
	public float speed = 8f;
    // Start is called before the first frame update

    void Update()
    {
		transform.position = Vector3.MoveTowards(transform.position, transform.position+(transform.rotation.y != 0 ? Vector3.right : Vector3.left), speed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        GameObject instance = Instantiate(Echo, transform.position, Quaternion.Euler(0, transform.rotation.y * 180, 0));
        Destroy(instance, 0.3f);
    }
}
