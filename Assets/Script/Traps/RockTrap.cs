using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrap : Buildable {

	public Animator anim;

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

	public int resoureSpent;

	GameObject TempPart;
	Vector3 oldPos;
	Vector3 trapPos;
	public GameObject block;
	public GameObject rock;

	public bool startroll; 

	void Start () 
	{
		TempPart = GameObject.Find ("TempTrapSpawnPart");
		oldPos = TempPart.transform.position;
		trapPos = gameObject.transform.position;
		TempPart.transform.position = trapPos;
		Invoke ("MovePartBack", 0.5f);
		startroll = false;
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
		turnMaster.instance.setPlayerResource(this.playerId, resoureSpent);
	}

	void Update ()
	{
		if (rock) {					
		anim.SetBool ("StartRoll", startroll);
		}
		if (startroll && block.transform.localPosition.y >= 1f) 
		{
			block.transform.Translate (Vector3.back * 0.5f * Time.fixedUnscaledDeltaTime);
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
			Debug.Log ("Monster is taking damage from rock trap");
			startroll = true;
			Invoke ("Destroy", 1.5f);
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
