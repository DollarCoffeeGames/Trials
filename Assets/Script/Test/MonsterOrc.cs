using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;

public class MonsterOrc : Unit 
{

    void OnDrawGizmos()
    {
        if (this.shortPath.Count > 0)
        {

            Node previousNode = null;

            for(int count = 0; count < this.shortPath.Count; count++)
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
    }

    void Update()
    {
        if (this.startMovement)
        {
            this.Move();
        }
    }
}
