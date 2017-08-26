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
    List<PlayerActions> turnAction;

    int m_turnCount = 0;
    int actionCount = 0;

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

    bool startTurnAction = false;
    int currAction = 0;

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

        this.turnAction = new List<PlayerActions>();
        charList = new List<PlayerTemplate>();
        turnEventFunc = new List<turnEvent>();
	}

    void Update()
    {
        if (this.startTurnAction)
        {
            if (this.turnAction.Count > 0)
            {
                switch (this.turnAction[currAction].action)
                {
                    case PlayerActions.PlayerAction.Trap:
                        this.turnAction[currAction].parentControl.actionDone = true;
                        Destroy(this.turnAction[currAction].uiAction);
                        this.currAction++;
                        break;
                    case PlayerActions.PlayerAction.CharMovement:
                        if (!this.turnAction[currAction].parentUnitControl.startMovement && !this.turnAction[currAction].parentUnitControl.unitMoved)
                        {
                            this.turnAction[currAction].parentUnitControl.StartMove();
                        }
                        else if (!this.turnAction[currAction].parentUnitControl.startMovement && this.turnAction[currAction].parentUnitControl.unitMoved)
                        {
                            this.turnAction[currAction].parentUnitControl.unitMoved = false;
                            Destroy(this.turnAction[currAction].uiAction);

                            this.currAction++;
                        }
                        break;
                }
            }

            if (this.currAction >= this.turnAction.Count)
            {
                this.moveTurn();
            }
        }
    }

    public void StartTurnAction()
    {
        this.startTurnAction = true;
        this.currAction = 0;
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

        this.actionCount = 0;
        this.startTurnAction = false;

        this.turnAction.Clear();
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

    PlayerTemplate getCurrentPlayer()
    {
        return this.charList[this.currentPlayerId()];
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

    public int addAction(PlayerActions.PlayerAction type, int resource, GameObject parent)
    {
        PlayerTemplate player = getCurrentPlayer();

        if (player.resourceAmount > 0 && player.resourceAmount > resource)
        {
            PlayerActions action = new PlayerActions();

            action.action = type;
            action.resource = resource;
            action.parent = parent;
            action.parentControl = parent.GetComponent<Buildable>();
            action.parentUnitControl = parent.GetComponent<Unit>();
            action.actionId = this.actionCount++;

            action.uiAction = UITestManager.instance.AddAction(type, action.actionId);

            player.resourceAmount -= resource;

            this.turnAction.Add(action);

            return action.actionId;
        }

        return -1;
    }

    public void removeAction(int id)
    {
        PlayerActions action = this.turnAction.Find(a => a.actionId == id);


        if (action.action == PlayerActions.PlayerAction.Trap)
        {
            Destroy(action.parent);
        }

        PlayerTemplate player = getCurrentPlayer();

        player.resourceAmount += action.resource;

        Destroy(action.uiAction);
        this.turnAction.Remove(action);
    }
}
