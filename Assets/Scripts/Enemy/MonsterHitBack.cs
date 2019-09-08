using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBack : MonoBehaviour
{
	public float hitBackDist = 1;
	public float hitBackSpeed = 5;
	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator GotHit(bool right) {
		Vector3 target;
		if (right) target = transform.position + new Vector3(hitBackDist, 0, 0);
		else target = transform.position + new Vector3(-hitBackDist, 0, 0);
		while (transform.position != target) {
			transform.position = Vector3.MoveTowards(transform.position, target, hitBackSpeed * Time.deltaTime);
			yield return 0;
		} 
	}
}
