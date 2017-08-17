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

    [SerializeField]
    [Range(0f, 1000f)]
    public int playerResourceAmount;

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

        charList = new List<PlayerTemplate>();
        turnEventFunc = new List<turnEvent>();
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

        charList[turnCount % this.charList.Count].startTurn();
    }

    public void setPlayerResource(int playerId, int amountResource)
    {
        this.charList[playerId].resourceAmount -= amountResource;
    }

    public bool isPlayerTurn(int playerNum)
    {
        if ((turnCount % this.charList.Count) == playerNum)
        {
            return true;
        }

        return false;
    }

    public int currentPlayerId()
    {
        return (turnCount % this.charList.Count);
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
