using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster;
using gridMaster.Pathfinding;

public class mousePosition : MonoBehaviour
{
    [SerializeField]
    GameObject[] trapList;

    [SerializeField]
    GameObject[] trapPrefab;

    [SerializeField]
    GameObject currentTrap;

    int currentTrapNum;
    Vector3 originalTrapPos;

    [SerializeField]
    GameObject lastMonster;

    int lastTrapNumber = 0;
    Vector2 lastTrapSize = Vector2.one;

    void FixedUpdate () 
    {    
        //create a ray cast and set it to the mouses cursor position in game
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit)) 
        {
            //draw invisible ray cast/vector
            Debug.DrawLine (ray.origin, hit.point);
            //log hit area to the console
            //Debug.Log(hit.point+" = Grid - "+GridMaster.instance.gridPosition(hit.point));

            //Debug.Log(hit.transform.name, hit.transform.gameObject);

            if (currentTrap)
            {
                currentTrap.transform.position = GridMaster.instance.gridPosition(hit.point);

                if (!GridMaster.instance.hasTrap(currentTrap.transform.position, this.lastTrapSize))
                {
                    currentTrap.SendMessage("setStatus", true);
                }
                else
                {
                    currentTrap.SendMessage("setStatus", false);
                }

                if (Input.GetButtonUp("Fire1"))
                {

                    if (!GridMaster.instance.hasTrap(currentTrap.transform.position, this.lastTrapSize))
                    {
                        GameObject newTrap = Instantiate(trapPrefab[currentTrapNum], currentTrap.transform.position, currentTrap.transform.rotation);
                        GridMaster.instance.setTrap(currentTrap.transform.position, newTrap, this.lastTrapSize);

                        currentTrap.SendMessage("setStatus", true);
                        currentTrap.transform.position = originalTrapPos;
                        currentTrap = null;
                        this.lastTrapSize = Vector2.one;

                        if (lastTrapNumber == 3)
                        {
                            lastMonster = newTrap;
                        }
                    }
                }
            }
            else
            {
                if (Input.GetButtonUp("Fire1"))
                {
                    Node endNode = GridMaster.instance.GetNodeByPosition(hit.point);
                    Node startNode = GridMaster.instance.GetNodeByPosition(Vector3.zero);

                    if (lastMonster)
                    {
                        startNode = GridMaster.instance.GetNodeByPosition(lastMonster.transform.position);
                    }

                    PathfindingMaster.instance.RequestPath(startNode, endNode, setMonsterPath);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (currentTrap)
            {
                currentTrap.SendMessage("setStatus", true);
                currentTrap.transform.position = originalTrapPos;
                currentTrap = null;
            }
        }
    }

    public void setMonsterPath(Stack<Node> path)
    {
        Stack<Node> nodeList = new Stack<Node>(path);
        if (lastMonster)
        {
            lastMonster.GetComponent<MonsterOrc>().curNode = new Stack<Node>(path);

            Debug.Log("pt999 = " + lastMonster.GetComponent<MonsterOrc>().curNode.Count, lastMonster);
        }

        while (nodeList.Count > 0)
        {
            Node curNode = nodeList.Pop();
            Debug.Log("Node - x:" + curNode.gridPositionX + " z: " + curNode.gridPositionZ+" Walkable: "+curNode.isWalkable, curNode.Tile);
        }
    }

    public void useTrap(int trapNumber)
    {
        originalTrapPos = trapList[trapNumber].transform.position;
        currentTrapNum = trapNumber;
        currentTrap = trapList[trapNumber];

        lastTrapSize = trapList[trapNumber].GetComponent<trap>().size;

        lastTrapNumber = trapNumber;
    }
}
