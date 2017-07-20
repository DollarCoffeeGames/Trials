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

    List<PlayerTemplate> charList;

    int m_turnCount = 0;

    public int turnCount
    {
        get 
        {
            return m_turnCount;
        }
        private set
        {
            m_turnCount = value;
        }
    }

    public delegate void turnEvent(int curTurn);

    List<turnEvent> turnEventFunc;

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

    public void turnEventTest(int curTurn)
    {
    }
	
    public int registerPlayer(PlayerTemplate charControl)
    {
        charList.Add(charControl);

        if (charList.Count == 1)
        {
            charControl.startTurn();
        }

        return charList.Count - 1;
    }

    public void moveTurn()
    {
        turnCount++;

        foreach(turnEvent func in turnEventFunc)
        {
            func(this.turnCount);
        }
    }

    public bool isPlayerTurn(int playerNum)
    {
        if ((turnCount % this.charList.Count) == playerNum)
        {
            return true;
        }

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
