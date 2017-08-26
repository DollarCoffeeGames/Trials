using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerActions  : IEquatable<PlayerActions>
{

    public enum PlayerAction
    {
        Trap,
        CharMovement
    }

    public int actionId;
    public PlayerAction action;
    public int resource;
    public GameObject parent;
    public Buildable parentControl;
    public Unit      parentUnitControl;
    public GameObject uiAction;

    public bool Equals (PlayerActions other)
    {
        if (this.actionId == other.actionId)
            return true;
        else
            return false;
    }
}
