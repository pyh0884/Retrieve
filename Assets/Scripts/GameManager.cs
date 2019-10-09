using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float CurrentHp;
    public float HpCapacity;
    public int CurrentDMG;
    public float CritPos;
    public GameObject playerPrefab;
	private GameObject player;
    private GameObject boss;
	public Vector3 spawnPos;
    public int BossIndex;
    public bool cheat;
    public int[] elements= { 0, 0, 0 };
    public int[] levels = { 0, 0, 0, 0 };
    public float HorizontalSpeed;
    public float JumpSpeed;
    public int BurnDamage=2;
    public float SlowMultiplier = 1.5f;
    public float money = 0;
    public float targetMoney = 0;
    public float MoneySpeed;
    [Header("产生精英怪比例")]
    [Range(0, 100)]
    public float ElitePos; //每关精英怪数量
    public int YellowPos = 5;
    public int GreenPos = 5;
    public int BluePos = 5;
    public int RedPos = 5;
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
		HpCapacity = PlayerPrefs.GetInt("MAXHP", 110);
		CurrentDMG = PlayerPrefs.GetInt("DAMAGE", 0);
        levels[0]= PlayerPrefs.GetInt("YELLOW", 0);
        levels[1] = PlayerPrefs.GetInt("GREEN", 0);
        levels[2] = PlayerPrefs.GetInt("BLUE", 0);
        levels[3] = PlayerPrefs.GetInt("RED", 0);
        money= PlayerPrefs.GetFloat("MONEY", 0);
        targetMoney = money;
        YellowPos = PlayerPrefs.GetInt("YELLOWPOS", 5);
        GreenPos = PlayerPrefs.GetInt("GREENPOS", 5);
        BluePos = PlayerPrefs.GetInt("BLUEPOS", 5);
        RedPos = PlayerPrefs.GetInt("REDPOS", 5);
        SlowMultiplier = 1.5f + 0.25f * levels[0];
        BurnDamage = 2 + 2 * levels[3];
        HorizontalSpeed = 7.75f + levels[1] * 0.5f;
        JumpSpeed = 11.5f + levels[1] * 0.4f;
    }
    public float HP
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
    public float MAXHP
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

    public void increaseCrit(int CRIT)
    {
        CritPos += CRIT;
    }
    public void LevelUpYellow() {
        if (levels[0] != 3)
        {   levels[0]++;
            PlayerPrefs.SetInt("YELLOW", levels[0]);
            YellowPos += 5;
            PlayerPrefs.SetInt("YELLOWPOS", YellowPos);
            SlowMultiplier = 1.5f+ 0.25f*levels[0];
            CurrentDMG += 1;
            PlayerPrefs.SetInt("DAMAGE", CurrentDMG);
        }
    }
    public void LevelUpGreen()
    {
        if (levels[1] != 3)
        {
            levels[1]++;
            PlayerPrefs.SetInt("GREEN", levels[1]);
            GreenPos += 5;
            PlayerPrefs.SetInt("GREENPOS", GreenPos);
            JumpSpeed += 0.4f;
            HorizontalSpeed += 0.5f;
            player.GetComponent<PlayerController>().HorizontalSpeed = HorizontalSpeed;
            player.GetComponent<PlayerController>().jumpSpeed = JumpSpeed;
        }
    }
    public void LevelUpBlue()
    {
        if (levels[2] != 3)
        {
            levels[2]++;
            PlayerPrefs.SetInt("BLUE", levels[2]);
            BluePos += 5;
            PlayerPrefs.SetInt("BLUEPOS", BluePos);
            HpCapacity += 30;
            PlayerPrefs.SetFloat("MAXHP", HpCapacity);

        }
    }
    public void LevelUpRed()
    {
        if (levels[3] != 3)
        {
            levels[3]++;
            PlayerPrefs.SetInt("RED", levels[3]);
            RedPos += 5;
            PlayerPrefs.SetInt("REDPOS", RedPos);
            BurnDamage = 2+2*levels[3];
            CritPos += 5;
            PlayerPrefs.SetFloat("CRIT", CritPos);

        }
    }
    public void GetMoney(int n)
    {
        //lerp数字
        targetMoney += n;
        targetMoney = Mathf.Clamp(targetMoney,0,10000);
        PlayerPrefs.SetFloat("MONEY", targetMoney);

    }
    void Start()
    {
        CurrentHp = HpCapacity;
        BossIndex = 0;
        HorizontalSpeed = 7.75f;
        JumpSpeed = 11.5f;
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
        Cursor.visible = false;
        if (Input.GetKeyDown(KeyCode.F12) && !cheat)
        {
            cheat = true;
        }
        else if (Input.GetKeyDown(KeyCode.F12) && cheat)
        {
            cheat = false;
        }
        int sceneNum = SceneManager.GetActiveScene().buildIndex;
        if (player == null&&sceneNum!=0&& sceneNum !=1 && sceneNum != 2 && sceneNum != 7 && sceneNum != 10 && sceneNum != 13 && sceneNum != 15 && sceneNum != 16 && sceneNum != 17)
		{
			player = Instantiate(playerPrefab, spawnPos, new Quaternion());			
		}
        money = Mathf.Lerp(money,targetMoney,Time.deltaTime*MoneySpeed);
        CurrentHp = player.GetComponent<HealthBarControl>().Hp;
        if(player.GetComponentInChildren<EatColor>())
        elements = player.GetComponentInChildren<EatColor>().elements;
        //HpCapacity = player.GetComponent<HealthBarControl>().HpMax;
    }
}

