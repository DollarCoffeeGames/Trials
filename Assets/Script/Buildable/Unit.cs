using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;

public abstract class Unit : Buildable {

    // Contains the current position (In Unity coordinates) of this unit
    protected Vector3 currentPos;
    // Contains the current Board Node this unit is occupying
    protected Node currentNode;
    // Contains the next Board Node this unit will move to
    protected Node nextNode;
    // Contains the path this unit will take when moving
    protected Stack<Node> path = null;
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
    protected int Movement;
    // Contains the distance (measured in Board Nodes) this unit reveals around self (Dissipates Fog of War)
    // 0    Self visible only
    // >0   Radius of vision around self
    protected int SightRange;

    // Applies this unit's basic attack to a target.
    public virtual void ApplyAttack(/*Board Node target??*/)
    {
        Debug.Log("Applying Unit Attack method.");
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
        Debug.Log("Applying Unit Move method. Doing nothing.");
    }

    // Get/Set methods
    public void SetPath(Stack<Node> path)
    {
        this.path = path;
    }
}
