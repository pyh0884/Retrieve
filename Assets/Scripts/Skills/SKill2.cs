using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coroutines;

public class SKill2 : MonoBehaviour
{
    Collider2D nearest;

    public int damageAmt = 5;
    public float maxRadius = 8.0f;
    public float rushSpeed = 25.0f;
    public float liveTime = 0.5f;
    public LayerMask enemyLayer;
    private bool hit = false;
    Coroutines.Coroutine _Main;
    public int HitTimes;
    GameManager gm;
	Collider2D coll;
    void Start()
    {
        Destroy(gameObject, 3);
        FindEnemy();
        _Main = new Coroutines.Coroutine(Main());
        gm = FindObjectOfType<GameManager>();
		coll = GetComponent<Collider2D>();
        HitTimes = 4 + gm.levels[1];
    }

    // Update is called once per frame
    void Update()
    {
        // Just tick our root coroutine
        _Main.Update();
    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position, 9, enemyLayer);
        if (list.Length == 0)
        {
            nearest = null;
        }
        else
        {
            nearest = list[0];
            foreach (Collider2D col in list)
            {
                if (Vector2.Distance(new Vector2(col.transform.position.x, col.transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)) <= Vector2.Distance(new Vector2(nearest.transform.position.x, nearest.transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)))
                    nearest = col;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Enemy")
        //{
        //    FindObjectOfType<AudioManager>().Play("Hit");
        //    if (collision.gameObject.GetComponent<BossHp>() != null)
        //    {
        //        collision.gameObject.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 20)));
        //    }
        //    else
        //    {
        //        collision.gameObject.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 20)));
        //    }
        //    hit = true;
        //    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //}
        if (collision.gameObject.layer == 11)
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            Destroy(collision.gameObject);
        }
    }

    IEnumerable<Instruction> Main()
    {
        bool gotFirst = false;
        bool finished = false;
        yield return ControlFlow.ExecuteWhile(
            () => !finished,
            Throw(transform.rotation.eulerAngles.y == 0),
            CheckFirst(success => { gotFirst = success; finished = success; }),
            TimeOut(liveTime, finish => finished = finish)
            );
        if (gotFirst)
        {
            List<GameObject> aims = new List<GameObject>();
            bool gotOthers = false;
            yield return ControlFlow.Call(Search(success => gotOthers = success, result => aims = result));
            if (gotOthers)
            {
				for (int i = 0; i < HitTimes; i++)
                {
                    yield return ControlFlow.Call(Attack(aims[i%aims.Count]));
                }
            }
        }
        Destroy(gameObject);
    }

    IEnumerable<Instruction> Throw(bool right)
    {
        Vector3 delta = new Vector3((right ? rushSpeed : -rushSpeed) * Time.deltaTime, 0);
        while (true)
        {
            if (nearest)
            {
                GetComponent<Rigidbody2D>().velocity = (nearest.transform.position - transform.position).normalized * rushSpeed;
            }
            else
            {
                transform.position += delta;
            }
            yield return null;
        }
    }

    IEnumerable<Instruction> TimeOut(float waitTime, System.Action<bool> finish)
    {
        yield return Utils.WaitForSeconds(waitTime);
        finish(true);
    }

    IEnumerable<Instruction> CheckFirst(System.Action<bool> success)
    {
        //try
        //{
        while (true)
        {
            if (hit)
            {
                //Debug.Log("yeah");
                //collidedWith.GetComponent<MonsterHp>().Damage(damageAmt);
                success(true);
                yield break;
            }
            else yield return null;
        }
        //}
    }

    IEnumerable<Instruction> Search(System.Action<bool> success, System.Action<List<GameObject>> result)
    {
        Collider2D[] candidates = Physics2D.OverlapCircleAll(transform.position, maxRadius, enemyLayer);
        List<GameObject> targets = new List<GameObject>();
        if (candidates.Length == 0)
        {
            success(false);
            yield break;
        }
        else
        {
            foreach (Collider2D candidate in candidates)
                targets.Add(candidate.gameObject.transform.parent.gameObject);
            success(true);
            result(targets);
        }
    }

    IEnumerable<Instruction> Attack(GameObject target)
    {
        Debug.Log(target.name);
        while (transform.position != target.transform.position)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                rushSpeed * Time.deltaTime
                );
            yield return null;
        }
		coll.enabled = true;
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (target.GetComponent<BossHp>() != null)
            {
                target.GetComponent<BossHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 20)));
            }
            else
            {
                target.GetComponent<MonsterHp>().Damage(Mathf.RoundToInt((Random.Range(5, 13) + 20)));
            }
            hit = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        yield return null;
		coll.enabled = false;
		Vector2 rand = Random.insideUnitCircle.normalized;
		Vector3 newTarget = transform.position + (Vector3)rand;
		while (transform.position != newTarget) {
			transform.position = Vector3.MoveTowards(transform.position, newTarget, rushSpeed * Time.deltaTime);
			yield return null;
		}
    }
}