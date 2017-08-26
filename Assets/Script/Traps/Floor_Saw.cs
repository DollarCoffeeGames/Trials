using System.Collections;
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

	public bool startspin; 
	public bool sparkSwitch;
	public GameObject sparks;
	public Transform sparkSpawn;
	public Transform sparkSpawn1;

	void Start () 
    {
		speed = 300f;
		startspin = false;
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
        turnMaster.instance.setPlayerResource(this.playerId, resoureSpent);
	}

	void Update ()
	{
		if (startspin) 
        {
			saw.transform.Rotate (Vector3.forward, speed * Time.deltaTime);
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
		}
		}
		public void OnTriggerExit (Collider c)
		{
			if (c.gameObject.CompareTag ("Unit")) {
			Sparks ();	
			Invoke ("Destroy", 3.0f);
		}
		}

	public void Sparks () {
		if (sparkSwitch == true) {
			GameObject Sparks = Instantiate (sparks,
				                    sparkSpawn.position,
				                    sparkSpawn.rotation) as GameObject;
			sparkSwitch = false;
		} else if (sparkSwitch == false) {
			GameObject Sparks = Instantiate (sparks,
				sparkSpawn1.position,
				sparkSpawn1.rotation) as GameObject;
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
