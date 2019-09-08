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

    void TryEat()
    {
        if (Input.GetKeyDown(KeyCode.E)&& pc.controllable)
        {
            anim.SetTrigger("Eat");
        }
    }
    void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && pc.controllable)
        {
            anim.SetTrigger("Attack");
            switch (ec.elements[1])
            {
                case 1://防护罩
                    Skills[1].SetActive(true);
                    break;
                case 2://连锁闪电
                    //if (nearest == null)
                    //{

                    //}
                    //else
                    //{

                    //}
                    break;
                case 3://悬浮大剑
                    break;
                case 4://陨石
                    {
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
                        break;
                    }
                case 5://回血
                    Skills[5].SetActive(true);
                    break;
                case 6://匕首
                    {
                        if (player.transform.rotation.y == 0)
                            Instantiate(Skills[6], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                        else
                            Instantiate(Skills[6], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                        break;
                    }
                case 7://地火
                    {
                        if (player.transform.rotation.y == 0)
                            Instantiate(Skills[7], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                        else
                            Instantiate(Skills[7], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                        break;
                    }
                case 8://冰锥
                    break;
                case 9://TODO黑洞
                    {
                        if (player.transform.rotation.y == 0)
                            Instantiate(Skills[9], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                        else
                            Instantiate(Skills[9], new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, 0), Quaternion.Euler(0, player.transform.rotation.y * 180, 0));
                        break;
                    }
                case 10://爆炸
                    GameObject obj;
                    obj=Instantiate(Skills[10], player.transform.position, Quaternion.identity);
                    obj.transform.SetParent(player.transform);
                    break;
                default:
                    break;
            }
        }
    }
    void FindEnemy()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(player.transform.position, 6, enemyLayer);
        if (list.Length == 0)
        {
            nearest = null;
        }
        else
        {
            nearest=list[0];
            foreach (Collider2D col in list)
            {
                if (Vector2.Distance(new Vector2(col.transform.position.x, col.transform.position.y), new Vector2(gameObject.transform.position.x, col.transform.position.y)) <= Vector2.Distance(new Vector2(nearest.transform.position.x, nearest.transform.position.y), new Vector2(gameObject.transform.position.x, col.transform.position.y)))
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
    }

    void Update()
    {
        TryEat();
        UseSkill();

    }
}
