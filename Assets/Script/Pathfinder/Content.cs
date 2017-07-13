using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content : MonoBehaviour {

    [SerializeField]
    public bool isWalkable = true;

    [SerializeField]
    public float movementWeight = 0;

    [Header("Block walk direction")]
    [SerializeField]
    public bool North = false;

    [SerializeField]
    public bool South = false;

    [SerializeField]
    public bool East = false;

    [SerializeField]
    public bool West = false;

}
