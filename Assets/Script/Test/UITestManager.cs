using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITestManager : MonoBehaviour 
{
    public static UITestManager instance
    {
        get { return _instance; }//can also use just get;
        set { _instance = value; }//can also use just set;
    }

    //Creates a class variable to keep track of GameManger
    static UITestManager _instance = null;

    [SerializeField]
    Text turnLbl;

	[SerializeField]
	Text resourceLbl;

    [SerializeField]
    Transform actionUIGroup;

    [SerializeField]
    GameObject actionUIPrefab;

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
	}

    public void updateTurn()
    {
        this.turnLbl.text = turnMaster.instance.turnCount.ToString();
    }

    public GameObject AddAction(PlayerActions.PlayerAction action, int actionId)
    {
        GameObject tempUI = Instantiate(actionUIPrefab, this.actionUIGroup);

        ActionButton actionBtn = tempUI.GetComponent<ActionButton>();

        Text textUI = tempUI.transform.Find("Text").GetComponent<Text>();
        Image imgUI  = tempUI.transform.Find("Image").GetComponent<Image>();

        actionBtn.SetActionId(actionId);

        switch (action)
        {
            case PlayerActions.PlayerAction.Trap:
                textUI .text = "New Trap";
                break;
            case PlayerActions.PlayerAction.CharMovement:
                textUI .text = "Movement";
                break;
        }

        return tempUI;
    }

	void Update ()
	{
		//this.resourceLbl.text = turnMaster.instance.resource.ToString();
	}
}
