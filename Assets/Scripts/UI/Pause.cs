using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool paused = false;
    public GameObject PauseMenu;
    public GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu") && !paused)
        {
            Cursor.visible = false;

            Time.timeScale = 0;            
            PauseMenu.SetActive(true);
            cursor.SetActive(true);
            paused = true;
        }
        else if ((Input.GetButtonDown("Menu")||Input.GetButtonDown("Cancel")) && paused)
        {
            Cursor.visible = false;

            PauseMenu.SetActive(false);
            cursor.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }
    public void resume()
    {
        Cursor.visible = false;

        PauseMenu.SetActive(false);
        cursor.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }
}
