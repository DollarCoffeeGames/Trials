﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : Buildable 
{

    [SerializeField]
    Renderer[] trapRenders;

    [SerializeField]
    Material allowed;

    [SerializeField]
    Material notAllowed;

    /*[SerializeField]
    public Vector2 size = Vector2.one;*/

    bool curStatus = true;

	int currentTurn;

	public int resoureSpent;

	void Start () {
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
		//turnMaster.instance.resource -= resoureSpent;
	}


	void Update ()
	{

	}



	public void testTurnCount (int turn)
	{
		if (turn - currentTurn > 3) {
			this.Alive = false;
			Debug.Log ("Object is dead");
		
		//	turnMaster.instance.removeTurnEvent (testTurnCount);
		//	Destroy (gameObject);
			//commented out until bug fix with turnmaster
			//destroys attached script
		}

	}

    public void setStatus(bool status)
    {

        if (curStatus == status)
        {
            return;
        }

        foreach (Renderer trapRender in this.trapRenders)
        {
            trapRender.material = (status) ? this.allowed : notAllowed;
        }

        curStatus = status;
    }


	public void OnTriggerEnter (Collider c)
	{
		if (c.gameObject.CompareTag ("Monster")) {
		//apply damage to monster 
			//should be doing things here
			Debug.Log ("Monster is taking damage from trap");
		}
	}
	public void OnTriggerExit (Collider c)
	{
		if (c.gameObject.CompareTag ("Monster")) {
		}
    }

    override public void SelectUnit()
    {
    }
}