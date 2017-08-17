using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITestManager : MonoBehaviour 
{
    [SerializeField]
    Text turnLbl;
	[SerializeField]
	Text resourceLbl;

    // Use this for initialization
	void Start () 
    {
	}

    public void updateTurn()
    {
        this.turnLbl.text = turnMaster.instance.turnCount.ToString();
    }
	void Update ()
	{
		//this.resourceLbl.text = turnMaster.instance.resource.ToString();
	}
}
