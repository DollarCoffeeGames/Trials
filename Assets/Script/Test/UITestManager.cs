using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITestManager : MonoBehaviour 
{
    [SerializeField]
    Text turnLbl;
	
    // Use this for initialization
	void Start () 
    {
	}

    public void updateTurn()
    {
        this.turnLbl.text = turnMaster.instance.turnCount.ToString();
    }
}
