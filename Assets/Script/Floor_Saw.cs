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

	[SerializeField]
	public Vector2 size = Vector2.one;

	bool curStatus = true;

	int currentTurn;
	bool playAnim;

	public int resoureSpent;

	public float m_speed = 300f;
		
	public GameObject m_saw;

	public bool m_startspin; 
	public bool m_sparkSwitch;
	public GameObject m_sparks;
	public Transform m_sparkSpawn;
	public Transform m_sparkSpawn1;

	void Start () 
    {
		m_speed = 300f;
		m_startspin = false;
		currentTurn = turnMaster.instance.registerTurnEvent (testTurnCount);
        turnMaster.instance.setPlayerResource(this.playerId, resoureSpent);
	}

	void Update ()
	{
		if (m_startspin) 
        {
			m_saw.transform.Rotate (Vector3.forward, m_speed * Time.deltaTime);
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
		if (c.gameObject.CompareTag ("Monster")) {
			//apply damage to monster 
			//should be doing things here
			Debug.Log ("Monster is taking damage from saw trap");
			m_startspin = true;
			Sparks ();
		}
		}
		public void OnTriggerExit (Collider c)
		{
			if (c.gameObject.CompareTag ("Monster")) {
			m_startspin = false;
			Sparks ();		
		}
		}

	public void Sparks () {
		if (m_sparkSwitch == true) {
			GameObject sparks = Instantiate (m_sparks,
				                    m_sparkSpawn.position,
				                    m_sparkSpawn.rotation) as GameObject;
			m_sparkSwitch = false;
		} else if (m_sparkSwitch == false) {
			GameObject sparks = Instantiate (m_sparks,
				m_sparkSpawn1.position,
				m_sparkSpawn1.rotation) as GameObject;
			m_sparkSwitch = true;
		}
	}

    override public void SelectUnit()
    {
    }
}
