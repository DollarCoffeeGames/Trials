using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;

public class MonsterOrc : MonoBehaviour 
{
    public Stack<Node> curNode;

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
    }

    void Start()
    {
        curNode = new Stack<Node>();
    }

    void Path(Stack<Node> nodeList)
    {
        Debug.Log("Pt198 - "+nodeList.Count);
        curNode = nodeList;
        Debug.Log("Pt198 - "+curNode.Count);
    }
}
