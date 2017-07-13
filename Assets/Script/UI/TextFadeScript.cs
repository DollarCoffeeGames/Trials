using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeScript : MonoBehaviour {

    Text text;
    float timer = 10.0f;
    bool increaseTimer = false;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void Update () {
       

       
		
	}

    void FadeIn()
    {
        timer += Time.deltaTime;
    }

    void FadeOut()
    {
        timer -= Time.deltaTime;
    }


}
