using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gridMaster
{
    namespace Pathfinding
    {
        public class Node: IHeapItem<Node>
        {
            List<GameObject> TileContent = new List<GameObject>();

            public GameObject m_Tile = null;

            public GameObject Tile
            {
                get { return this.m_Tile; }
                set
                {
                    this.m_Tile = value;
                    this.worldPosition = value.transform.position;
                }
            }

            GameObject m_Trap = null;

            public GameObject Trap
            {
                get { return this.m_Trap; }
                set
                {
                    this.m_Trap = value;
                }
            }

            public Vector3 position
            {
                get
                {
                    return new Vector3(gridPositionX, 0, gridPositionZ);
                }
            }

            public Vector3 worldPosition = Vector3.zero;

            int m_heapIndex;

            public int heapIndex
            {
                get
                {
                    return this.m_heapIndex;
                }
                set
                {
                    this.m_heapIndex = value;
                }
            }

            public int CompareTo(Node nodeToComp)
            {
                int compare = fullCost.CompareTo(nodeToComp.fullCost);

                if (compare == 0)
                {
                    compare = hCost.CompareTo(nodeToComp.hCost);
                }

                return -compare;
            }

            public float gridPositionX;
            public float gridPositionZ;

            public float gCost = 0;
            public float hCost = 0;
            public float cCost = 0;


            public bool North;
            public bool South;
            public bool East;
            public bool West;

            public float fullCost
            {
                get
                {
                    return this.gCost + this.hCost + this.cCost;
                }
            }

            public Node parentNode = null;
            public List<Node> Neighbours = new List<Node>();
            public bool m_isWalkable = true;

            public bool isWalkable
            {
                get
                {
                    if (!m_isWalkable)
                    {
                        return false;
                    }

                    return true;
                }
                set
                {
                    m_isWalkable = value;
                }
                    
            }

            public void addContent(GameObject content)
            {
                this.TileContent.Add(content);

                if (this.Tile != null)
                {   
                    content.transform.SetParent(this.Tile.transform);
                }

                Content objData = content.GetComponent<Content>();

                if (this.gridPositionX == 8 && this.gridPositionZ == 6)
                {
                    Debug.Log(this.South + " - " + this.North + " - " + this.East + " - " + this.West);
                }

                if(objData != null)
                {
                    this.isWalkable = (objData.isWalkable && this.isWalkable);
                    this.North      = (objData.North || this.North);
                    this.South      = (objData.South || this.South);
                    this.East       = (objData.East || this.East);
                    this.West       = (objData.West || this.West);
                }
            }

            public void setTrap(GameObject trapObj, bool isParent)
            {
                this.Trap = trapObj;

                if (isParent && this.Tile != null)
                {   
                    this.Trap.transform.SetParent(this.Tile.transform);
                }
            }

            public bool isConnected(Node parentNode)
            {
                if (this.gridPositionX == parentNode.gridPositionX)
                {
                    if (this.gridPositionZ > parentNode.gridPositionZ)
                    {
                        if (this.South || parentNode.North)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (this.North || parentNode.South)
                        {
                            return false;
                        } 
                    }
                }
                else if (this.gridPositionZ == parentNode.gridPositionZ)
                {
                    if (this.gridPositionX < parentNode.gridPositionX)
                    {
                        if (this.East || parentNode.West)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (this.West || parentNode.East)
                        {
                            return false;
                        } 
                    }
                }
                else
                {
                    float distX = parentNode.gridPositionX - this.gridPositionX;

                    if (this.gridPositionX < parentNode.gridPositionX)
                    {
                        if (this.East)
                        {
                            return false;
                        } 
                    }
                    else
                    {
                        if (this.West)
                        {
                            return false;
                        } 
                    }
                }
                    
                return true;
            }
        }
    }
}