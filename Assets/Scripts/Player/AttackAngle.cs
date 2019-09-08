using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAngle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public Vector3 GetAttackAngle(Vector3 attack, Vector3 target) {
		Vector3 vector = (target - attack).normalized*4;
		Vector2 random = Random.insideUnitCircle;
		vector += new Vector3(random.x, random.y);
		return vector;
		//调用此方法使特效与攻击方向契合并有+-15°内的随机偏差，生成特效之后使之.transform.right/up=GetAttackAngle；
	}
}
