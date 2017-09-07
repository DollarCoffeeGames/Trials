using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;

public class GridUIMaster : MonoBehaviour 
{
    public static GridUIMaster instance
    {
        get { return _instance; }//can also use just get;
        set { _instance = value; }//can also use just set;
    }

    //Creates a class variable to keep track of GameManger
    static GridUIMaster _instance = null;

    public enum tileColorStyle
    {
        Blue
    }
    [Header("Units")]
    [SerializeField]
    public GameObject unitSelectedUI;

    [SerializeField]
    public GameObject desNodeUI;

    [Header("Particles")]
    public ParticleSystem spawnParticle;

    [Header("PathLine")]
    [SerializeField]
    public LineRenderer pathRender;

    [Header("WalkTiles")]
    [SerializeField]
    Material tileBlueMaterial;

    List<Node> lastPath;

	// Use this for initialization
	void Start () 
    {
        //check if GameManager instance already exists in Scene
        if(instance)
        {
            //GameManager exists,delete copy
            DestroyImmediate(gameObject);
        }
        else
        {
            //assign GameManager to variable "_instance"
            instance = this;
        }
	}

    public void clearGrid()
    {
        if (lastPath != null && lastPath.Count > 0)
        {
            for (int count = 0; count < lastPath.Count; count++)
            {
                lastPath[count].tileHighLigh.gameObject.SetActive(false);
            }
        }

        if (this.pathRender.positionCount != 0)
        {
            this.pathRender.positionCount = 0;
        }

        Debug.Log("Test");

        if(this.unitSelectedUI.activeSelf)
        {
            this.unitSelectedUI.SetActive(false);
        }

        if(this.desNodeUI.activeSelf)
        {
            this.desNodeUI.SetActive(false);
        }
    }
	
    public void highLightTile(List<Node> node)
    {
        this.clearGrid();

        for (int count = 0; count < node.Count; count++)
        {
            node[count].tileHighLigh.gameObject.SetActive(true);
            node[count].tileHighLigh.material = tileBlueMaterial;
        }

        lastPath = node;
    }

    public void drawLine(List<Node> path)
    {
        List<Vector3> pathTrans = new List<Vector3>();

        for(int count = 0; count < path.Count; count++)
        {
            Vector3 posNode = path[count].worldPosition;
            posNode.y += 1;
            pathTrans.Add(posNode);
        }

        this.pathRender.positionCount = 0;
        this.pathRender.positionCount = pathTrans.Count;
        this.pathRender.SetPositions(pathTrans.ToArray());
    }

    public void selectUnitUI(Vector3 position)
    {
        this.unitSelectedUI.SetActive(true);
        this.unitSelectedUI.transform.position = position;
    }

    public void destineNodeUI(Vector3 position)
    {
        this.desNodeUI.SetActive(true);
        this.desNodeUI.transform.position = position;
    }
}
