using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;
using gridMaster;

public class MonsterOrc : MonoBehaviour 
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
            Node startNode = GridMaster.instance.GetNodeByPosition(transform.position);

            PathfindingMaster.instance.RequestWalkableNodes(startNode, 5, setWalkableNodes);

            searchWalkNodes = false;
        }

        if(this.walkNodes != null)
        {
            for(int count = 0; count < this.walkNodes.Count; count++)
            {
                if(this.walkNodes[count].Tile != null && this.walkNodes[count].Tile.transform.childCount > 0)
                {
                    Transform child = this.walkNodes[count].Tile.transform.GetChild(0);

                    if (child.GetComponent<Renderer>())
                    {
                        Material testMat = new Material(child.GetComponent<Renderer>().material);
                        testMat.color = Color.blue;
                        child.GetComponent<Renderer>().material = testMat;
                    }
                }
            }
        }
    }

    void Start()
    {
        curNode = new Stack<Node>();
    }

    public void Path(Stack<Node> nodeList)
    {
        Debug.Log("Pt198 - "+nodeList.Count);
        curNode = nodeList;
        Debug.Log("Pt198 - "+curNode.Count);

        Stack<Node> tempNode = new Stack<Node>(nodeList);

        Node previousNode = null;

        List<Vector3> renderPos = new List<Vector3>();

        while (tempNode.Count > 0)
        {
            Node curNode = tempNode.Pop();

            if (previousNode != null)
            {
                Vector3 posNode = curNode.worldPosition;

                posNode.y += 1;

                renderPos.Add(posNode);
            }

            previousNode = curNode;
        }

        ControlMaster.instance.pathRender.positionCount = 0;
        ControlMaster.instance.pathRender.SetPositions(renderPos.ToArray());
            
    }

    public void setWalkableNodes(List<Node> path)
    {
        this.walkNodes = path;
    }
}
