using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCamera : MonoBehaviour {

	public float speed; 



	// Use this for initialization
	void Start () {
		speed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (Vector3.left * speed);
		} if (Input.GetKey (KeyCode.S)) {
			transform.Translate (Vector3.back * speed);
			transform.Translate (Vector3.down * speed);
		} if (Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector3.right * speed);
		} if (Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector3.forward * speed);
			transform.Translate (Vector3.up * speed);
		} if (Input.GetKey (KeyCode.Q)) {
			transform.Translate (Vector3.back * speed);
		} if (Input.GetKey (KeyCode.E)) {
			transform.Translate (Vector3.forward * speed);
		} 
	}
}
