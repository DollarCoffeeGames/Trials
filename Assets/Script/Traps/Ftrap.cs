using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ftrap : Buildable 
{
	public ParticleSystem Fire;

	[SerializeField]
	Renderer[] trapRenders;

	[SerializeField]
	Material allowed;

	[SerializeField]
	Material notAllowed;


	bool curStatus = true;

	int currentTurn;

	public int resoureSpent;

	void Start () {
        turnMaster.instance.setPlayerResource(this.playerId, 25);
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
		FClear ();
		Fire.Stop ();
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
			Debug.Log ("Monster is taking damage from fire trap");
			Fire.Play ();
			Invoke ("FStop", 1.0f);
			Invoke ("FClear" , 2.9f);
			Invoke ("Destroy", 3.0f);
		}
	}

	void FStop () {
		Fire.Stop ();
	}

	public void FClear ()
    {
		Fire.Clear ();
    }

	void Destroy () {
		Destroy (gameObject);
	}
    override public void SelectUnit()
    {
    }
}
