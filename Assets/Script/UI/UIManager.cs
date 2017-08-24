using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    int utilityPoints;
    

    GameManager gm;
    public GameObject trapPanel;
    public GameObject menuButton;
    public GameObject pauseMenu;
    int resourcePoints;

    // Use this for initialization
    void Start () {
        trapPanel.SetActive(false);
        pauseMenu.SetActive(false);
        menuButton.SetActive(true);
        gm = GetComponent<GameManager>();
        resourcePoints = 100;
		
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}

    void DisplayStats(int hp, int up, int atk, int def, int rng)
    {
        //mouse over stat display for characters
    }

    void TrapStats(int dmg, int rng, int dur, int spd, int cost)
    {
        //mouse over stat display for traps
        //opens panels contained in buttons
    }

    public void ToggleMenu()
    {
        if (trapPanel.activeSelf == true)
        {
            trapPanel.SetActive(false);
        }
        else
        {
            trapPanel.SetActive(true);
        }
    }

    void ToggleMenuButton()
    {
        if(menuButton.activeSelf == true)
        {
            menuButton.SetActive(false);
        }
        else
        {
            menuButton.SetActive(true);
        }
    }

    private void TogglePause()
    {
        if (Time.timeScale>0)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);

        }
        else if (Time.timeScale==0)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

}
