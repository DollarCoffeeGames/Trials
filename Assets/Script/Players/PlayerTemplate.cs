using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayerTemplate : MonoBehaviour {

    protected int playerId;

    [SerializeField]
    public int resourceAmount = 0;


    protected virtual void Start()
    {
        this.resourceAmount = turnMaster.instance.playerResourceAmount;

        this.playerId = turnMaster.instance.registerPlayer(this);
    }

    abstract public void startTurn();

    public virtual void turnEvent(int curTurn)
    {
        this.resourceAmount = turnMaster.instance.playerResourceAmount;
    }
}
