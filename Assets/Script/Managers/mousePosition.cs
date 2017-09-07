using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster;
using gridMaster.Pathfinding;

public class mousePosition : MonoBehaviour
{

    Unit currUnit;

    [Header("Traps")]
    [SerializeField]
    GameObject[] trapList;

    [SerializeField]
    GameObject[] trapPrefab;

    GameObject currentTrap;

    int currentTrapNum;
    Vector3 originalTrapPos;

    Vector2 lastTrapSize    = Vector2.one;
    Vector2 originalTrapSize = Vector2.one;

    bool canClick = true;
    bool canRotate = true;
    bool hasPath = false;

    Node currNode;

    void FixedUpdate () 
    {    
        //create a ray cast and set it to the mouses cursor position in game
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit)) 
        {
            //draw invisible ray cast/vector
            Debug.DrawLine (ray.origin, hit.point);

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

                if (Input.GetButtonUp("Fire1") && this.canClick)
                {

                    if (!GridMaster.instance.hasTrap(currentTrap.transform.position, this.lastTrapSize))
                    {
                        GameObject newTrap = Instantiate(trapPrefab[currentTrapNum], currentTrap.transform.position, currentTrap.transform.rotation);

						if (!newTrap.transform.CompareTag("Unit"))
						{
							GridMaster.instance.setTrap(currentTrap.transform.position, newTrap, this.lastTrapSize);
						}
						else
						{
							Unit tempUnit = newTrap.GetComponent<Unit>();
							GridMaster.instance.setUnit(currentTrap.transform.position, tempUnit);

						}

                        newTrap.GetComponent<Buildable>().playerId = turnMaster.instance.currentPlayerId();

                        currentTrap.SendMessage("setStatus", true);
                        currentTrap.transform.position = originalTrapPos;
                        currentTrap.transform.rotation = Quaternion.identity;
                        currentTrap = null;
                        this.lastTrapSize = Vector2.one;

                        turnMaster.instance.addAction(PlayerActions.PlayerAction.Trap, 25, newTrap);

                        this.canClick = false;
                        StartCoroutine(this.cooldownClick());
                    }
                }
                else if (Input.GetKeyUp(KeyCode.R) && this.canRotate)
                {
                    currentTrap.transform.Rotate(0, 90, 0);

                    Vector2 newSize = this.originalTrapSize;

                    Debug.Log((int)Mathf.Round(currentTrap.transform.eulerAngles.y));

                    switch ((int)Mathf.Round(currentTrap.transform.eulerAngles.y))
                    {
                        case 90:
                            newSize.y = -this.originalTrapSize.x;
                            newSize.x = this.originalTrapSize.y;

                            break;
                        case -180:
                            newSize.y = this.originalTrapSize.y;
                            newSize.x = -this.originalTrapSize.x;

                            break;
                        case -90:
                            newSize.y = this.originalTrapSize.x;
                            newSize.x = -this.originalTrapSize.y;

                            break;
                    }

                    this.lastTrapSize = newSize;
                    this.canRotate = false;
                    StartCoroutine(this.cooldownRotation());
                }
            }
            else
            {
                if (Input.GetButtonUp("Fire1") && this.canClick)
                {
                    if (hit.transform.CompareTag("Unit"))
                    {
                        Unit tempUnit = hit.transform.gameObject.GetComponent<Unit>();

                        if (tempUnit.actionDone)
                        {
                            this.currUnit = tempUnit;
                            this.currUnit.SelectUnit();
                            this.hasPath = false;
                        }
                    }
                    else if (hit.transform.CompareTag("Tile"))
                    {
                        this.hasPath = true;
                        turnMaster.instance.addAction(PlayerActions.PlayerAction.CharMovement, 10, this.currUnit.gameObject);
                    }

                    this.canClick = false;
                    StartCoroutine(this.cooldownClick());
                }
                else if(!this.hasPath && this.currUnit != null)
                {
                    Node endNode = GridMaster.instance.GetNodeByPosition(hit.point);

                    if (currNode != endNode && this.currUnit.walkableNodes.Contains(endNode))
                    {
                        if (this.currUnit != null)
                        {
                            Node startNode = GridMaster.instance.GetNodeByPosition(this.currUnit.transform.position);

                            PathfindingMaster.instance.RequestPath(startNode, endNode, this.currUnit.SetPath);
                        }

                        currNode = endNode;
                    }
                }

                if (this.currUnit != null)
                {
                    GridUIMaster.instance.destineNodeUI(GridMaster.instance.gridPosition(hit.point));
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (this.currentTrap)
            {
                this.currentTrap.SendMessage("setStatus", true);
                this.currentTrap.transform.position = originalTrapPos;
                this.currentTrap = null;
            }
            else if(this.currUnit)
            {
                this.currUnit = null;
                GridUIMaster.instance.clearGrid();
            }
        }
    }

    public void useTrap(int trapNumber)
    {
        this.originalTrapPos = trapList[trapNumber].transform.position;
        this.currentTrapNum = trapNumber;
        this.currentTrap = trapList[trapNumber];

        this.lastTrapSize     = trapList[trapNumber].GetComponent<Buildable>().size;
        this.originalTrapSize = lastTrapSize;

        if (this.currUnit)
        {
            this.currUnit = null;
            GridUIMaster.instance.clearGrid();
        }
    }

    public void moveUnit()
    {
        GridUIMaster.instance.clearGrid();
        this.currUnit.StartMove();
        this.currUnit = null;
    }

    IEnumerator cooldownClick()
    {
        yield return new WaitForSeconds(0.5f);
        this.canClick = true;
    }

    IEnumerator cooldownRotation()
    {
        yield return new WaitForSeconds(0.5f);
        this.canRotate = true;
    }
}
