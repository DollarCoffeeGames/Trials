using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    static GameManager _instance = null;
    AudioManager aM;
    UIManager UI;

    int highScore;
    int enemyDistanceToGoal;





    // Use this for initialization
    void Start () {
        aM = GetComponent<AudioManager>();
        UI = GetComponent<UIManager>();

        if(instance)
        {
            DestroyImmediate(gameObject);
        }

        else
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                SceneManager.LoadScene(1);
            }
        } 
        
        if(Input.GetKeyDown(KeyCode.KeypadEnter)||Input.GetKeyDown(KeyCode.Return))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                SceneManager.LoadScene(1);
            }
        }       



		
	}

    public void OnValueChanged (float newValue)
    {
        AudioListener.volume = newValue;
       
    }

    void StartGame()
    {
        SceneManager.LoadScene("Title");
    }

    void QuitGame()
    {
        Application.Quit();
    }

    public static GameManager instance
    {
        get { return _instance; }   // or get;
        set { _instance = value; }  // or set;
    }
}
