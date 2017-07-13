using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content : MonoBehaviour {

    [SerializeField]
    public bool isWalkable;

    [SerializeField]
    public float movementWeight;

    [Header("Block walk direction")]
    [SerializeField]
    public bool North;

    [SerializeField]
    public bool South;

    [SerializeField]
    public bool East;

    [SerializeField]
    public bool West;

}
