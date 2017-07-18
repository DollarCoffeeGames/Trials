using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace gridMaster
{
    namespace Pathfinding
    {
        public class Pathfinding
        {
            public volatile bool jobDone = false;
            PathfindingMaster.PathfindingJobComplete completeCallbackFunc;
            Stack<Node> lastPath;

            Node startPosition;
            Node finalPosition;

            GridMaster gridMaster;

            public Pathfinding(Node startNode, Node finalNode, PathfindingMaster.PathfindingJobComplete callback)
            {
                startPosition = startNode;
                finalPosition = finalNode;
                completeCallbackFunc = callback;

                gridMaster = GridMaster.instance;
            }

            public Stack<Node> findPath(Node startNode, Node finalNode)
            {
                return findPathInternal(startNode, finalNode);
            }

            public void findPath()
            {
                jobDone = false;
                lastPath = findPathInternal(startPosition, finalPosition);
                jobDone = true;
            }

            public void NotifyComplete()
            {
                if (this.completeCallbackFunc != null)
                {
                    this.completeCallbackFunc(lastPath);
                }
            }

            private Stack<Node> findPathInternal(Node startNode, Node finalNode)
            {

                Stopwatch SW = new Stopwatch();
                SW.Start();
                //List of nodes to the destination
                Stack<Node> finalPath = new Stack<Node>();

                //List of nodes to check and nodes already checkded
                Heap<Node> openNodes = new Heap<Node>(gridMaster.maxSize);
                List<Node> closedNodes = new List<Node>();

                //Fist node is the start Node;
                openNodes.Add(startNode);

                int countTest = 0;

                while (openNodes.Count > 0)
                {
                    Node currentNode = openNodes.RemoveFirst();
                    closedNodes.Add(currentNode);

                    if (currentNode.Equals(finalNode))
                    {
                        SW.Stop();
                        UnityEngine.Debug.Log("Path Found: "+ SW.ElapsedMilliseconds +" ms");
                        finalPath = retracePath(startNode, currentNode);
                        break;
                    }

                    foreach (Node neighbourNode in GetNeighbours(currentNode, false))
                    {
                        if (!closedNodes.Contains(neighbourNode))
                        {
                            float newMovementCostToNeighbour = currentNode.hCost + GetDistance(currentNode, neighbourNode);

                            if (newMovementCostToNeighbour < neighbourNode.hCost || !openNodes.Contains(neighbourNode))
                            {
                                neighbourNode.gCost = newMovementCostToNeighbour;
                                neighbourNode.hCost = GetDistance(neighbourNode, finalNode);

                                neighbourNode.parentNode = currentNode;

                                if (!openNodes.Contains(neighbourNode))
                                {
                                    openNodes.Add(neighbourNode);
                                    openNodes.UpdateItem(neighbourNode);
                                }
                            }
                        }
                    }

                    /*if (countTest > 10)
                    {
                        break;
                    }*/

                    countTest++;
                }

                return finalPath;

            }

            private Stack<Node> retracePath(Node startNode, Node finalNode)
            {
                Stack<Node> path = new Stack<Node>();

                Node currentNode = finalNode;

                while (currentNode != startNode)
                {
                    path.Push(currentNode);

                    currentNode = currentNode.parentNode;
                }

                return path;
            }

            private List<Node> GetNeighbours(Node currentNode, bool checkY)
            {

                List <Node> retList = new List<Node>();

                for (int countX = -1; countX <= 1; countX++)
                {
                    for (int indexY = -1; indexY <= 1; indexY++)
                    {
                        for (int countZ = -1; countZ <= 1; countZ++)
                        {
                            int countY = indexY;

                            if (!checkY && countY != 0)
                            {
                                continue;
                            }

                            if (countX != 0 || countY != 0 || countZ != 0)
                            {
                                Node newNode = GetNeighbourNode(currentNode, new Vector3(currentNode.gridPositionX + countX, countY, currentNode.gridPositionZ + countZ), true);

                                if (newNode != null)
                                {
                                    retList.Add(newNode);
                                }
                            }
                        }
                    }
                }

                return retList;
            }

            private Node GetNeighbourNode(Node currentNode, Vector3 nodePos, bool checkVertical)
            {

                Node retNode = null;

                Node node = GetNode(nodePos.x, nodePos.y, nodePos.z);

                if (node != null && node.isWalkable)
                {
                    retNode = node;
                }
                else if (checkVertical)
                {
                    nodePos.y -= 1;
                    Node bottomNode = GetNode(nodePos.x, nodePos.y, nodePos.z);

                    if (bottomNode != null && bottomNode.isWalkable)
                    {
                        retNode = bottomNode;
                    }
                    else
                    {
                        nodePos.y += 1;

                        Node topNode = GetNode(nodePos.x, nodePos.y, nodePos.z);

                        if (topNode != null && topNode.isWalkable)
                        {
                            retNode = topNode;
                        }
                    }
                }

                //For Diagonal moviment
                int newX = (int)(nodePos.x - currentNode.gridPositionX);
                int newZ = (int)(nodePos.z - currentNode.gridPositionZ);

                if (Mathf.Abs(newX) == 1 && Mathf.Abs(newZ) == 1)
                {
                    Node neightbour1 = GetNode(nodePos.x + newX, nodePos.y, nodePos.z);

                    if (neightbour1 == null)
                    {
                        retNode = null;
                    }
                    else if (!neightbour1.isWalkable)
                    {
                        retNode = null;
                    }

                    Node neightbour2 = GetNode(nodePos.x, nodePos.y, nodePos.z + newZ + newZ);

                    if (neightbour2 == null)
                    {
                        retNode = null;
                    }
                    else if (!neightbour2.isWalkable)
                    {
                        retNode = null;
                    }
                }

                //Other conditions to check like walls
                if (retNode != null)
                {
                    if (!retNode.isConnected(currentNode))
                    {

                        if (retNode.gridPositionX == 8 && retNode.gridPositionZ == 6)
                        {
                            UnityEngine.Debug.Log("("+currentNode.gridPositionZ+", "+currentNode.gridPositionX+") not connected");
                        }

                        retNode = null;
                    }
                }

                return retNode;
            }

            private Node GetNode(int positionX, int positionY, int positionZ)
            {
                Node nNode = null;

                lock (gridMaster)
                {
                    nNode = gridMaster.GetNode(positionX, positionY, positionZ);
                }

                return nNode;
            }

            private Node GetNode(float positionX, float positionY, float positionZ)
            {
                return GetNode((int)positionX, (int)positionY, (int)positionZ);
            }

            private float GetDistance(Node nodeA, Node nodeB)
            {
                Vector3 distance = nodeA.position - nodeB.position;

                distance.x = Mathf.Abs(distance.x);
                distance.y = Mathf.Abs(distance.y);
                distance.z = Mathf.Abs(distance.z);

                if (distance.x > distance.z)
                {
                    return 14 * distance.z + 10 * (distance.x - distance.z) + 10 * distance.y;
                }

                return 14 * distance.x + 10 * (distance.z - distance.x) + 10 * distance.y;
            }
        }
    }
}
