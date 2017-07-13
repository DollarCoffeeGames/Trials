using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour 
{
    int utilityPoints;

    GameManager gm;

    // Use this for initialization
    void Start () {

        gm = GetComponent<GameManager>();
		
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

}
