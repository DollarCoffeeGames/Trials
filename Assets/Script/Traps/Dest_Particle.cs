using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dest_Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("Destroy", 1.7f);
	}
	
	void Destroy () {
		Destroy (gameObject);
	}


}
