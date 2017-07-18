using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayerTemplate : MonoBehaviour {

    protected int playerId;

    abstract public void startTurn();

    public virtual void turnEvent(int curTurn)
    {
    }
}
