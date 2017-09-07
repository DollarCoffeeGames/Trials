using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;
using gridMaster.environment;

namespace AI
{
    [System.Serializable]
    public class AILeader 
    {

        public AIUnits leader = null;
        public int totalUnits;
        public List<AIUnits> units;
        List<AIUnits> unitsWithoutAction;

        public Hotpoint conquer;

    	// Use this for initialization
        public AILeader() 
        {
            this.units = new List<AIUnits>();
            this.conquer = new Hotpoint();
    	}

        public void updateUnits()
        {
            this.unitsWithoutAction = new List<AIUnits>(this.units);
        }
    	
    	// Update is called once per frame
    	public void Update () 
        {
            if(this.conquer.conquerRemaining.Count > 0)
            {
                this.checkUnitAction(this.leader);

                for (int count = 0; count < this.units.Count; count++)
                {
                    this.checkUnitAction(this.units[count]);
                }
            }
    	}

        public void Move()
        {
            
        }

        public bool isFull()
        {
            if (this.units.Count >= (this.totalUnits - 1))
            {
                return true;
            }

            return false;
        }

        void checkUnitAction(AIUnits unit)
        {
            this.checkUnitMovement(unit);
        }

        void checkUnitMovement(AIUnits unit)
        {
            if (unit.destNode == null)
            {
                int randomNode = Random.Range(0, this.conquer.conquerRemaining.Count - 1);
                Node conquerNode = this.conquer.conquerRemaining[randomNode];

                unit.destNode = conquerNode;
                unit.waitingNodes = true;

                PathfindingMaster.instance.RequestPath(unit.currentNode, conquerNode, this.leader.SetPath);

                this.conquer.conquerRemaining.RemoveAt(randomNode);

                this.unitsWithoutAction.Remove(unit);
            }
            else
            {
                /*if (!this.leader.waitingNodes && )
                {
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
                }*/
            }
        }
    }
}
