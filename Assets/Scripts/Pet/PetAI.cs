using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAI : MonoBehaviour
{
    public bool Boss;
    public Animator anim;
    public PlayerController pc;
    public Collider2D col;
    public EatColor ec;
    public LayerMask enemyLayer;
    public Collider2D nearest;
    public GameObject[] Skills;
    public GameObject player;
    public float xOffSet;
    public float yOffSet;
    GameObject CurrentSkill;
    GameManager gm;

	private bool isPressing = false;

    IEnumerator UseSkill()
    {
        while (true)
        {
            if (pc.controllable)
            {
                if (Input.GetButtonDown("Fire4") || Input.GetAxisRaw("Fire4") != 0)
                {
                    if (isPressing == false)
                    {
                        // Call your event function here.
                        switch (ec.elements[1])
                        {
                            case 1://防护罩            
                                anim.SetTrigger("Attack");
                                var skill = Instantiate(Skills[1], new Vector3(player.transform.position.x, player.transform.position.y + 2), transform.rotation);
                                skill.GetComponent<Rigidbody2D>().velocity = new Vector2(player.transform.rotation.y == 0 ? 5 : -5, 0);
                                ec.elements[0] = 0;
                                ec.elements[1] = 0;
                                ec.elements[2] = 0;
                                break;
                            case 2://连锁闪电
                                anim.SetTrigger("Attack");
                                Instantiate(Skills[2], player.transform.position, transform.rotation);
                                ec.elements[0] = 0;
                                ec.elements[1] = 0;
                                ec.elements[2] = 0;

                                break;
                            case 3://悬浮大剑
                                anim.SetTrigger("Attack");
                                Skills[3].SetActive(true);
                                ec.elements[0] = 0;
                                ec.elements[1] = 0;
                                ec.elements[2] = 0;

                                break;
                            case 4://陨石
                                {
                                    anim.SetTrigger("Attack");
                                    FindEnemy();
                                    if (nearest == null)
                                    {
                                        if (player.transform.rotation.y == 0)
                                            Instantiate(Skills[4], new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + yOffSet, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                        else
                                            Instantiate(Skills[4], new Vector3(player.transform.position.x - xOffSet, player.transform.position.y + yOffSet, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));

                                    }
                                    else
                                    {
                                        Instantiate(Skills[4], new Vector3(nearest.transform.position.x - 4, nearest.transform.position.y + 8, 0), Quaternion.identity);
                                    }
                                    ec.elements[0] = 0;
                                    ec.elements[1] = 0;
                                    ec.elements[2] = 0;
                                    break;
                                }
                            case 5://回血
                                anim.SetTrigger("Attack");
                                Skills[5].SetActive(true);
                                ec.elements[0] = 0;
                                ec.elements[1] = 0;
                                ec.elements[2] = 0;

                                break;
                            case 6://匕首
                                {
                                    anim.SetTrigger("Attack");
                                    switch (gm.levels[1])
                                    {
                                        case 0:
                                            if (player.transform.rotation.y == 0)
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            else
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            break;
                                        case 1:
                                            if (player.transform.rotation.y == 0)
                                            {
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y - 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            }
                                            else
                                            {
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y - 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            }
                                            break;
                                        case 2:
                                            if (player.transform.rotation.y == 0)
                                            {
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y - 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y + 3, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            }
                                            else
                                            {
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y - 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y + 3, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            }

                                            break;
                                        case 3:
                                            if (player.transform.rotation.y == 0)
                                            {
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y - 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y + 3, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y - 3, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            }
                                            else
                                            {
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y - 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                yield return new WaitForSeconds(1);
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y + 3, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                                Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y - 3, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                            }

                                            break;
                                    }
                                    ec.elements[0] = 0;
                                    ec.elements[1] = 0;
                                    ec.elements[2] = 0;
                                    break;
                                }
                            case 7://地火
                                {
                                    anim.SetTrigger("Attack");
                                    if (player.transform.rotation.y == 0)
                                        Instantiate(Skills[7], new Vector3(player.transform.position.x + 1, player.transform.position.y, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                    else
                                        Instantiate(Skills[7], new Vector3(player.transform.position.x - 1, player.transform.position.y, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                                    ec.elements[0] = 0;
                                    ec.elements[1] = 0;
                                    ec.elements[2] = 0;

                                    break;
                                }
                            case 8://弹射箭
                                anim.SetTrigger("Attack");
                                Instantiate(Skills[8], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.identity);
                                ec.elements[0] = 0;
                                ec.elements[1] = 0;
                                ec.elements[2] = 0;

                                break;
                            case 9://炮台
                                {
                                    anim.SetTrigger("Attack");
                                    Instantiate(Skills[9], new Vector3(player.transform.position.x - 0.5f, player.transform.position.y + 1.5f, 0), Quaternion.identity);
                                    ec.elements[0] = 0;
                                    ec.elements[1] = 0;
                                    ec.elements[2] = 0;

                                    break;
                                }
                            case 10://爆炸
                                anim.SetTrigger("Attack");
                                GameObject obj;
                                obj = Instantiate(Skills[10], player.transform.position, Quaternion.identity);
                                obj.transform.SetParent(player.transform);
                                ec.elements[0] = 0;
                                ec.elements[1] = 0;
                                ec.elements[2] = 0;

                                break;
                            default:
                                break;
                        }
                        isPressing = true;
                    }
                }
                if (Input.GetAxisRaw("Fire4") == 0)
                {
                    isPressing = false;
                }
            }
            yield return null;
        }
    }
    void TryEat()
    {
        if (Input.GetButtonDown("Fire3") && pc.controllable)
        {
            anim.SetTrigger("Eat");
        }
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(1);
        Debug.Log(1);
    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(player.transform.position, 9, enemyLayer);
        if (list.Length == 0)
        {
            nearest = null;
        }
        else
        {
            nearest=list[0];
            foreach (Collider2D col in list)
            {
                if (Vector2.Distance(new Vector2(col.transform.position.x, col.transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)) <= Vector2.Distance(new Vector2(nearest.transform.position.x, nearest.transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)))
                    nearest = col;
            }
        }
    }
    void ColOn()
    {
        col.enabled = true;
    }
    void ColOff()
    {
        col.enabled = false;
    }
    void Start()
    {
        if (Boss)
            Destroy(gameObject);
        gm = FindObjectOfType<GameManager>();
        StartCoroutine("UseSkill");

    }

    void Update()
    {
        TryEat();
    }
}
