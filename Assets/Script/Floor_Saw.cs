﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Saw : Buildable 
{
	[SerializeField]
	Renderer[] trapRenders;

	[SerializeField]
	Material allowed;

	[SerializeField]
	Material notAllowed;

//	[SerializeField]
//	public Vector2 size = Vector2.one;

	bool curStatus = true;

	int currentTurn;
	bool playAnim;

	public int resoureSpent;

	public float speed = 300f;
		
	public GameObject saw;
	GameObject TempPart;
	Vector3 oldPos;
	Vector3 trapPos;

	public bool startspin; 
	public bool sparkSwitch;
	public GameObject sparks;
	public Transform sparkSpawn;
	public Transform sparkSpawn1;

	void Start () 
    {
		TempPart = GameObject.Find ("TempTrapSpawnPart");
		oldPos = TempPart.transform.position;
		trapPos = gameObject.transform.position;
		TempPart.transform.position = trapPos;
		Invoke ("MovePartBack", 0.5f);


		speed = 300f;
		startspin = false;
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
        turnMaster.instance.setPlayerResource(this.playerId, resoureSpent);
	}

	void Update ()
	{
		if (startspin) 
        {
			saw.transform.Rotate (Vector3.forward, speed);
		}
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
		if (c.gameObject.CompareTag ("Unit")) {
			//apply damage to monster 
			//should be doing things here
			Debug.Log ("Monster is taking damage from saw trap");
			startspin = true;
			Sparks ();
			Invoke ("Destroy", 1.5f);
		}
		}
		public void OnTriggerExit (Collider c)
		{
			if (c.gameObject.CompareTag ("Unit")) {
			Sparks ();	
		}
		}

	public void Sparks () {
		if (sparkSwitch == true) {
			Instantiate (sparks, sparkSpawn.position, sparkSpawn.rotation);
			sparkSwitch = false;
		} else if (sparkSwitch == false) {
			Instantiate (sparks, sparkSpawn1.position, sparkSpawn1.rotation);
			sparkSwitch = true;
		}
	}

	void Destroy () {
		Destroy (gameObject);
	}

	void MovePartBack () {
		TempPart.transform.position = oldPos;
	}

    override public void SelectUnit()
    {
    }
}