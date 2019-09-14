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
	public List<bool> TreasureChestOpened;
	public List<bool> BossBeaten;
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
		for (int i = 0; i < TreasureChestOpened.Count; i++) TreasureChestOpened[i] = false;
		for (int i = 0; i < BossBeaten.Count; i++) BossBeaten[i] = false;
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

    public void OpenTreasureChest(int index) {
		if (index <= TreasureChestOpened.Count) 
			if (!TreasureChestOpened[index])TreasureChestOpened[index] = true;
	}
	public void BeatBoss(int index) {
		if (index <= BossBeaten.Count)
			if (!BossBeaten[index]) BossBeaten[index] = true;
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

    public void increaseATK(int ATK)
    {
        CurrentDMG += ATK;
    }
    void Start()
    {
        HpCapacity = 6;
        CurrentHp = HpCapacity;
        CurrentDMG = 0;
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
        HpCapacity = player.GetComponent<HealthBarControl>().HpMax;
    }
}

