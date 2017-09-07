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

	GameObject TempPart;
	Vector3 oldPos;
	Vector3 trapPos;

	bool curStatus = true;

	int currentTurn;

	public int resoureSpent;

	void Start () {
		TempPart = GameObject.Find ("TempTrapSpawnPart");
		oldPos = TempPart.transform.position;
		trapPos = gameObject.transform.position;
		TempPart.transform.position = trapPos;
		Invoke ("MovePartBack", 0.5f);
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
			Invoke ("FStop", 0.4f);
			Invoke ("FClear" , 1.8f);
			Invoke ("Destroy", 1.5f);
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

	void MovePartBack () {
		TempPart.transform.position = oldPos;
	}

    override public void SelectUnit()
    {
    }
}
