// Fades out the Text of the HPParticle and Destroys it 

using UnityEngine;
using System.Collections;

public class HPParticleScript : MonoBehaviour {

	public float Alpha =1f;
	public float FadeSpeed = 1f;
	public float Gravity;

	private GameObject HPLabel;

	// Set a Variable
	void Start () 
	{
		Gravity = 0.1f;
		HPLabel = gameObject.transform.Find("HPLabel").gameObject;
	}

	void Update () 
	{
		Alpha = Mathf.Lerp(Alpha,0f,FadeSpeed * Time.unscaledDeltaTime);
		transform.Translate (Vector3.down * Gravity);

		Color CurrentColor = HPLabel.GetComponent<TextMesh>().color;
		HPLabel.GetComponent<TextMesh>().color = new Color(CurrentColor.r,CurrentColor.g,CurrentColor.b,Alpha);

		if (Alpha < 0.01f)
		{
			Destroy(gameObject);
		}
	}
}
