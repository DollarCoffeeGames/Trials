using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;
using gridMaster;

public abstract class Unit : Buildable 
{
    // Contains the current position (In Unity coordinates) of this unit
    protected Vector3 currentPos;
    // Contains the current Board Node this unit is occupying
    protected Node currentNode;
    // Contains the next Board Node this unit will move to
    protected Node nextNode;
    // Contains the path this unit will take when moving
    protected Stack<Node> path = null;
    // Contains the short path using path as base
    public List<Node> shortPath = new List<Node>();
    // List of all walkable nodes
    public List<Node> walkableNodes = new List<Node>();
    // Contains the basic attack value of this unit
    protected int Attack;
    // Returns the attack range value of this unit
    // 1    Melee
    // >1   Ranged
    protected int AttackRange;
    // Contains the area of effect of this unit's basic attack
    // THIS NEEDS REFINEMENT. Use an Array or 2d array?
    // 1    1 Node
    // 2    2x2 Nodes
    protected int AttackAOE;
    // Contains the distance (measured in Board Nodes) this unit can move in one turn
    // 0    No movement
    // >0   Radius of possible movement around self
    [SerializeField]
    [Range(0f,100f)]
    [Tooltip("Contains the distance (measured in Board Nodes) this unit can move in one turn")]
    protected int Movement;

    [SerializeField]
    [Range(0f,100f)]
    protected float Speed;

    // Contains the distance (measured in Board Nodes) this unit reveals around self (Dissipates Fog of War)
    // 0    Self visible only
    // >0   Radius of vision around self
    protected int SightRange;

    public bool startMovement = false; 
    public bool unitMoved     = false;
    protected bool updateNode = false; 

    Node targetNode;

    int indexPath = 0;

    float distanceNode;
    float startTime;

    // Applies this unit's basic attack to a target.
    public virtual void ApplyAttack(/*Board Node target??*/)
    {
        Debug.Log("Applying Unit Attack method.");
    }

    public virtual void StartMove()
    {
        this.updateNode = false;
        this.indexPath = 0;
        this.currentNode = this.shortPath[0];
        this.startMovement = true;
        this.unitMoved = true;
    }

    // Method which *executes* the determined movement of a unit.
    // 1) Unit is selected.
    // 2) Target location is selected.
    // 3) Action is queued.
    // 4) After Player ends turn, this action is executed.
    public virtual void Move()
    {
        // This method does not need to be virtual.
        // Take the Stack of Nodes, iterate through and mvoe to each Node in turn.
        // Execute similar to NavMeshAgent pathfinding.

        if (!this.updateNode)
        {
            if (this.indexPath < this.shortPath.Count - 1)
            {
                this.indexPath++;
            }
            else
            {
                this.startMovement = false;
            }

            if(this.targetNode != null)
            {
                this.currentNode = this.targetNode;
            }

            this.targetNode = this.shortPath[this.indexPath];

            this.distanceNode = Vector3.Distance(this.currentNode.worldPosition, this.targetNode.worldPosition);
            this.startTime = Time.time;

            this.updateNode = true;
        }

        float distCover = (Time.time - this.startTime) * this.Speed;

        if (this.distanceNode == 0)
        {
            this.distanceNode = 0.1f;
        }

        float pathJorney = distCover / this.distanceNode;

        if (pathJorney > 0.9f)
        {
            this.updateNode = false;
        }

        Vector3 targetPosition = Vector3.Lerp(this.currentNode.worldPosition, this.targetNode.worldPosition, pathJorney);

        Vector3 dir = this.targetNode.worldPosition - this.currentNode.worldPosition;

        if (!Vector3.Equals(dir, Vector3.zero))
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * this.Speed);
        }

        transform.position = targetPosition;

    }

    // Get/Set methods
    public void SetPath(Stack<Node> path)
    {
        this.path = path;
        this.CreateShortPath();
        GridUIMaster.instance.drawLine(this.shortPath);
    }

    public void SetWalkablePath(List<Node> nodes)
    {
        this.walkableNodes = nodes;
        GridUIMaster.instance.clearGrid();
        GridUIMaster.instance.highLightTile(this.walkableNodes);
    }

    public void CreateShortPath()
    {
        this.shortPath.Clear();

        if (this.path.Count <= 0)
        {
            return;
        }

        Vector3 curDirection = Vector3.zero;

        Node[] path = this.path.ToArray();

        for (int count = 1; count < path.Length; count++)
        {
            Vector3 nextDirection = new Vector3(path[count - 1].gridPositionX - path[count].gridPositionX,
                                                0,
                                                path[count - 1].gridPositionZ - path[count].gridPositionZ);

            if (nextDirection != curDirection)
            {
                this.shortPath.Add(path[count - 1]);
                this.shortPath.Add(path[count]);
            }

            curDirection = nextDirection;
        }

        this.shortPath.Add(path[path.Length - 1]);
    }

    override public void SelectUnit()
    {
        if (this.Movement != 0)
        {
            Node currNode = GridMaster.instance.GetNodeByPosition(transform.position);
            PathfindingMaster.instance.RequestWalkableNodes(currNode, this.Movement, this.SetWalkablePath); 
        }
    }
}
