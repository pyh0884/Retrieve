using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameManager gm;
	public GameObject attackEffectPrefab;
	private GameObject attackEffect;
    public float dmgMultiplier;
	private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public IEnumerator Vibration(float left, float right, float time = 0.5f)
    {
        try
        {
            XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, left, right);
            yield return new WaitForSeconds(time);
            XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0, 0);
        }
        finally
        {
            XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (collision.gameObject.GetComponent<BossHp>() != null)
            {
                if (Random.Range(0,100)<gm.CRIT)
                collision.gameObject.GetComponent<BossHp>().Damage2(Mathf.RoundToInt((Random.Range(4, 6)+ gm.DAMAGE * 8)* dmgMultiplier*1.4f),1);
                else
                collision.gameObject.GetComponent<BossHp>().Damage2(Mathf.RoundToInt((Random.Range(4, 6) + gm.DAMAGE * 8) * dmgMultiplier));
                //if(gm.levels[3]==3)
                //collision.gameObject.GetComponent<BossHp>().Burn=3;

            }
            else
            {
                if (Random.Range(0, 100) < gm.CRIT)
                    collision.gameObject.GetComponent<MonsterHp>().Damage2(Mathf.RoundToInt((Random.Range(4, 6) + gm.DAMAGE * 8) * dmgMultiplier * 1.4f), 1);
                else
                    collision.gameObject.GetComponent<MonsterHp>().Damage2(Mathf.RoundToInt((Random.Range(4, 6) + gm.DAMAGE * 8) * dmgMultiplier));
                //if (gm.levels[3] == 3)
                //    collision.gameObject.GetComponent<MonsterHp>().Burn = 3;

            }
            StartCoroutine(Vibration(0.03f, 0.03f, 0.1f));
            attackEffect = Instantiate(attackEffectPrefab, (transform.position + collision.transform.position) / 2, new Quaternion());
			attackEffect.transform.right = GetAttackAngle(transform.position, collision.gameObject.transform.position);
		}
        if (collision.gameObject.layer==11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
			attackEffect = Instantiate(attackEffectPrefab, (transform.position + collision.transform.position) / 2, new Quaternion());
			attackEffect.transform.right = GetAttackAngle(transform.position, collision.gameObject.transform.position);
		}
    }


	public Vector3 GetAttackAngle(Vector3 attack, Vector3 target)
	{
		Vector3 vector = (target - attack).normalized * 4;
		Vector2 random = Random.insideUnitCircle.normalized;
		vector += new Vector3(random.x, random.y);
		return vector;
		//调用此方法使特效与攻击方向契合并有+-15°内的随机偏差，生成特效之后使之.transform.right/up=GetAttackAngle
	}

}
