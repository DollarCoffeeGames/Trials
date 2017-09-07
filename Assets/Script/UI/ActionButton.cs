using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour {
    [SerializeField]
    Button actionBtn;

    int actionId;

	// Use this for initialization
	void Start () 
    {
        this.actionBtn.onClick.AddListener(OnClickRemove);
	}
	
    public void SetActionId(int id)
    {
        this.actionId = id;
    }

    public void OnClickRemove()
    {
        turnMaster.instance.removeAction(this.actionId);
    }
}
