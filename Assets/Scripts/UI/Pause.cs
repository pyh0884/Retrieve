using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool paused = false;
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu") && !paused)
        {
            Time.timeScale = 0;            
            PauseMenu.SetActive(true);
            paused = true;
        }
        else if ((Input.GetButtonDown("Menu")||Input.GetButtonDown("Cancel")) && paused)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }
    public void resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }
}
