using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Spikes : Buildable {
	
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

	public float speed;

	public GameObject spike;
	public int i;
	public bool startSpike; 
	public bool sparkSwitch;
	public GameObject sparks;
	public Transform sparkSpawn;
	public Transform sparkSpawn1;

	void Start () 
	{
		speed = 0.1f;
		startSpike = false;
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
		turnMaster.instance.setPlayerResource(this.playerId, resoureSpent);
	}

	void Update ()
	{
		if (startSpike) {
			if (i >= 0 && i <= 10) {
				spike.transform.Translate (Vector3.forward * speed);
				i++;
			} else if (i >= 10) {
				spike.transform.Translate (Vector3.back * speed);
				i++;
			}
			if (i >= 22) {
				i = 0;
			}
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
			Debug.Log ("Monster is taking damage from spike trap");
			startSpike = true;
			Sparks ();
			Invoke ("Destroy", 0.5f);
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

	override public void SelectUnit()
	{
	}
	}
