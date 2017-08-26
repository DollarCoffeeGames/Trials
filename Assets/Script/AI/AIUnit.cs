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

        bool searchWalkNodes = true;

        void OnDrawGizmosSelected()
        {
            if (curNode != null)
            {
                Stack<Node> tempNode = new Stack<Node>(curNode);

                Node previousNode = null;

                while (tempNode.Count > 0)
                {
                    Node curNode = tempNode.Pop();

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

            if (searchWalkNodes)
            {

                searchWalkNodes = false;
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

        /*override public void SetPath(Stack<Node> path)
        {
            base.SetPath(path);
            GridUIMaster.instance.drawLine(this.shortPath);
        }*/

        override public void Move()
        {
            base.Move();
            //this.walkNodes.Clear();
        }
    }
}