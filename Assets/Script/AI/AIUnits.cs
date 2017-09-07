using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;
using gridMaster;

namespace AI
{
    public class AIUnits : Unit 
    {
        public Stack<Node> curNode;

        public List<Node> walkNodes = null;

        public Node destNode = null;

        public bool waitingNodes = false;

        bool searchWalkNodes = true;

        void OnDrawGizmosSelected()
        {
            if (this.shortPath != null)
            {

                Node previousNode = null;

                for (int count = 0; count < this.shortPath.Count; count++)
                {
                    Node curNode = this.shortPath[count];

                    if (previousNode != null)
                    {
                        Vector3 startPos = previousNode.Tile.transform.position;
                        Vector3 endPos = curNode.Tile.transform.position;

                        startPos.y += 1;
                        endPos.y += 1;

                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(startPos, endPos);

                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(startPos, Vector3.one);

                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube(endPos, Vector3.one);
                    }

                    previousNode = curNode;
                }
            }
        }

        void Start()
        {
            curNode = new Stack<Node>();
        }

/*        override public void SelectUnit()
        {
            Debug.Log("AI Unit Selected");
        }*/

        public void setWalkableNodes(List<Node> path)
        {
            this.walkNodes = path;

            path.RemoveAt(0);

            GridUIMaster.instance.highLightTile(path);
            GridUIMaster.instance.selectUnitUI(transform.position);
        }

        override public void SetPath(Stack<Node> path)
        {
            this.path = path;
            this.CreateShortPath();

            this.waitingNodes = false;
        }

        override public void CreateShortPath()
        {
            this.shortPath.Clear();

            if (this.path.Count <= 0)
            {
                return;
            }

            Vector3 curDirection = Vector3.zero;

            this.shortPath = new List<Node>(this.path);
        }

        override public void Move()
        {
            base.Move();
            //this.walkNodes.Clear();
        }
    }
}