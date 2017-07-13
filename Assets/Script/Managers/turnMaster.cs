using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turnMaster : MonoBehaviour {


    public static turnMaster instance
    {
        get { return _instance; }//can also use just get;
        set { _instance = value; }//can also use just set;
    }

    //Creates a class variable to keep track of GameManger
    static turnMaster _instance = null;

    //List<charController1> charList;

    int turnCount = 0;

    float startTurnTime = 0;

    public delegate void turnEvent(int curTurn);

    List<turnEvent> turnEventFunc;

    AsyncOperation async;

	// Use this for initialization
	void Start () 
    {

        //check if GameManager instance already exists in Scene
        if(instance)
        {
            //GameManager exists,delete copy
            DestroyImmediate(gameObject);
        }
        else
        {
            //assign GameManager to variable "_instance"
            instance = this;
        }

        turnEventFunc = new List<turnEvent>();
	}

    void Update()
    {
        /*if (gameEnded)
        {
            return;
        }*/
    }
	
    /*public int registerPlayer(charController1 charControl)
    {
        charList.Add(charControl);

        Vector3 charPos = charControl.transform.position;

        charPos.x = Random.Range(-6f, 120f);
        charPos.z = 0;

        charControl.transform.position = charPos;

        if (charList.Count == 1)
        {
            this.focusCurrentPlayer();
            this.updateCharCameraPos();
            charControl.StartTurn();
        }

        return charList.Count - 1;
    }*/

    public void moveTurn()
    {
        turnCount++;
        //this.charList[turnCount % this.charList.Count].StartTurn();

        startTurnTime = Time.time;

        foreach(turnEvent func in turnEventFunc)
        {
            func(this.turnCount);
        }
    }

    public bool currentTurn(int playerNum)
    {
        /*if ((turnCount % this.charList.Count) == playerNum)
        {
            return true;
        }*/

        return false;
    }

    public int registerTurnEvent(turnEvent func)
    {
        turnEventFunc.Add(func);

        return this.turnCount;
    }

    public void removeTurnEvent(turnEvent func)
    {
        turnEventFunc.Remove(func);
    }
}
