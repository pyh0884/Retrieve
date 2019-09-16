using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int CurrentHp;
    public int HpCapacity;
    public int CurrentDMG;
    public float CritPos;
    public GameObject playerPrefab;
	private GameObject player;
    private GameObject boss;
	public Vector3 spawnPos;
    public int BossIndex;
    public bool cheat;
    public int[] elements= { 0, 0, 0 };
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "__Main Menu")
            Destroy(gameObject);
		
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
		CritPos = PlayerPrefs.GetFloat("CRIT", 8);
		HpCapacity = PlayerPrefs.GetInt("MAXHP", 5);
		CurrentDMG = PlayerPrefs.GetInt("DAMAGE", 0);
    }
    public int HP
    {
        get
        {
            return CurrentHp;
        }
    }
    public float CRIT
    {
        get
        {
            return CritPos;
        }
    }

    public void SetBossIndex(int num)
    {
            BossIndex = num;
    }
    public int MAXHP
    {
        get
        {
            return HpCapacity;
        }
    }
    public int DAMAGE
    {
        get
        {
            return CurrentDMG;
        }
    }
    public bool CHEAT
    {
        get
        {
            return cheat;
        }
    }

    public int[] ELEMENTS
    {
        get
        {
            return elements;
        }
    }

    public void increaseATK(int ATK)
    {
        CurrentDMG += ATK;
    }
    void Start()
    {
        CurrentHp = HpCapacity;
        BossIndex = 0;
	}
    public void Respawn()
    {
		float x = PlayerPrefs.GetFloat("RespwanX", 1.5f);
		float y = PlayerPrefs.GetFloat("RespwanY", -1);
		int index = PlayerPrefs.GetInt("RespwanSceneIndex", 3);
        spawnPos = new Vector3(x, y);
        /*		if(SceneManager.GetActiveScene().buildIndex!=index)*/
        SceneManager.LoadScene(index);
        CurrentHp = HpCapacity;	
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12) && !cheat)
        {
            cheat = true;
        }
        else if (Input.GetKeyDown(KeyCode.F12) && cheat)
        {
            cheat = false;
        }
        if (player == null)
		{
			player = Instantiate(playerPrefab, spawnPos, new Quaternion());			
		}
        CurrentHp = player.GetComponent<HealthBarControl>().Hp;
        elements = player.GetComponentInChildren<EatColor>().elements;
        //HpCapacity = player.GetComponent<HealthBarControl>().HpMax;
    }
}

