using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing_Axe : Buildable {

	[SerializeField]
	Renderer[] trapRenders;

	[SerializeField]
	Material allowed;

	[SerializeField]
	Material notAllowed;


	bool curStatus = true;

	int currentTurn;

	public int resoureSpent;

	bool swingOn;

	float speed; 

	public float m_parameter;

	bool swingLeft;

	public Transform m_moveAxe;

	// Use this for initialization
	void Start () {
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
		turnMaster.instance.setPlayerResource(this.playerId, resoureSpent);
		swingOn = false;
		speed = 100f;
	}
	
	// Update is called once per frame
	void Update () {
		m_parameter = m_moveAxe.transform.localEulerAngles.z;

		if (m_parameter >= 60 && m_parameter <= 295) {
			swingLeft = false;
		} else if (m_parameter <= 305 && m_parameter >= 295) {
			swingLeft = true;
		}

		if (swingOn) {
			if (swingLeft) {
				m_moveAxe.transform.Rotate (Vector3.forward * speed * Time.deltaTime);
			}
			if (!swingLeft) {
				m_moveAxe.transform.Rotate (Vector3.forward * -speed * Time.deltaTime);
			}
		} 
	}

	public void testTurnCount (int turn) {
		if (turn - currentTurn > 3) {
			this.Alive = false;
			Debug.Log ("Object is dead");

			//	turnMaster.instance.removeTurnEvent (testTurnCount);
			//	Destroy (gameObject);
			//commented out until bug fix with turnmaster
			//destroys attached script
		}

	}

	public void setStatus(bool status) {

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
		
	public void OnTriggerEnter (Collider c) {
		if (c.gameObject.CompareTag ("Unit")) {
			//apply damage to monster 
			//should be doing things here
			Debug.Log ("Monster is taking damage from swinging axe trap");
			swingOn = true;
		}
	}
	public void OnTriggerExit (Collider c) {
		if (c.gameObject.CompareTag ("Unit")) {
			Invoke ("Destroy", 3.0f);
		}
	}

	void Destroy () {
		Destroy (gameObject);
	}



	override public void SelectUnit()
	{
	}
}
