using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using gridMaster.Pathfinding;

namespace gridMaster
{
    public enum contentType
    {
        WALLS,
        PROPS
    }

    public class GridMaster : MonoBehaviour 
    {
        public static GridMaster instance
        {
            get { return _instance; }//can also use just get;
            set { _instance = value; }//can also use just set;
        }

        //Creates a class variable to keep track of GameManger
        static GridMaster _instance = null;

        [SerializeField]
        [Range(1f, 100f)]
        float speed;

        [SerializeField]
        bool startAnimation;

        [SerializeField]
        bool directionAnimation;

        [SerializeField]
        Vector3 startPosition;

        [SerializeField]
        Vector3 endPosition;

        [SerializeField]
        Vector2 columnCount;

        [SerializeField]
        Vector2 columnSize;

        [SerializeField]
        GameObject Board;

        [SerializeField]
        GameObject BoardLevel;

        [SerializeField]
        TextAsset mapFile;

        [SerializeField]
        TextAsset timeFile;

        [SerializeField]
        TextAsset wallsFile;

        [SerializeField]
        TextAsset propFile;

        [SerializeField]
        GameObject[] propPrefabs;

        [SerializeField]
        GameObject[] wallsPrefabs;

        [SerializeField]
        GameObject[] squarePrefabs;

        int currentColumn;

        List<List<Node>> Grid;

        List<List<Node>> mapGrid;

		[HideInInspector]
        public List<Node> spawnList;

        public List<List<Node>> conquerList;

        public int maxSize
        {
            get
            {
                return (int)(columnCount.x * columnCount.y);
            }
        }

    	// Use this for initialization
    	void Awake () 
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

			this.mapGrid = new List<List<Node>>();
			this.spawnList = new List<Node> ();
            this.conquerList = new List<List<Node>>();

			Node tempNode = new Node ();

			for (int countY = 0; countY < columnCount.y; countY++) 
			{
				this.mapGrid.Add(new List<Node>());

				for (int countX = 0; countX < columnCount.x; countX++) 
				{
					this.mapGrid [countY].Add (tempNode);
				}
			}

            /*this.Grid = new List<List<Node>>();

            for (int countY = 0; countY < columnCount.y; countY++)
            {
                this.Grid.Add(new List<Node>());

                for (int countX = 0; countX < columnCount.x; countX++)
                {

                    GameObject tempTile = createTile(countX, countY, (countY + countX) % 2, false);

                    Node currentNode = tempTile.AddComponent<Node>();

                    currentNode.Tile = tempTile;
                    currentNode.Tile.transform.SetParent(this.Board.transform);

                    currentNode.Tile.name = "Tile_" + countY + "_" + countX;

                    currentNode.gridPositionX = countX;
                    currentNode.gridPositionZ = countY;

                    this.Grid[countY].Add(currentNode);
                }
            }

            loadFileMap();*/
    	}
    	
    	// Update is called once per frame
    	/*void Update () 
        {
            if (startAnimation)
            {
                if (!directionAnimation)
                {

                    if (currentColumn >= this.Grid.Count)
                    {
                        startAnimation = false;
                        return;
                    }
                    //Debug.Log(this.Grid[currentColumn][0].transform.position.y + " - " + this.endPosition.y);

                    for (int count = 0; count < this.Grid[currentColumn].Count; count++)
                    {
                        Vector3 tempPos = this.Grid[currentColumn][count].Tile.transform.position;
                        tempPos.y -= this.speed * Time.deltaTime;

                        if (tempPos.y <= this.endPosition.y)
                        {
                            tempPos.y = this.endPosition.y;
                        }

                        this.Grid[currentColumn][count].Tile.transform.position = tempPos;

                    }

                    if (this.Grid[currentColumn][0].Tile.transform.position.y <= this.endPosition.y)
                    {
                        currentColumn++;
                    }
                }
                else
                {

                    if (currentColumn >= this.mapGrid.Count)
                    {
                        startAnimation = false;
                        return;
                    }

                    for (int count = 0; count < this.mapGrid[currentColumn].Count; count++)
                    {
                        Vector3 tempPos = this.mapGrid[currentColumn][count].Tile.transform.position;
                        tempPos.y += this.speed * Time.deltaTime;

                        if (tempPos.y >= this.startPosition.y)
                        {
                            tempPos.y = this.startPosition.y;
                        }

                        this.mapGrid[currentColumn][count].Tile.transform.position = tempPos;
                    }

                    if (this.mapGrid[currentColumn][0].Tile.transform.position.y >= (this.startPosition.y - 0.01f))
                    {
                        Debug.Log("Pt1");
                        currentColumn++;
                    }
                }
            }
            else
            {
                currentColumn = 0;
            }
    	}*/

        void loadFileMap()
        {
            this.mapGrid = new List<List<Node>>();

            string[] entries = mapFile.text.Split('\n');

            int column = 0;
            int row = 0;

            foreach (string line in entries)
            {
                string[] tiles = line.Split(' ');

                this.mapGrid.Add(new List<Node>());

                foreach (string tile in tiles)
                {
                    GameObject tempTile;

                    if (tile != "-1")
                    {
                        tempTile = createTile(column, row, int.Parse(tile));
                    }
                    else
                    {
                        tempTile = createEmpty(column, row, int.Parse(tile));
                    }

                    Node currentNode = tempTile.AddComponent<Node>();

                    if (tile == "-1")
                    {
                        currentNode.isWalkable = false;
                    }

                    currentNode.Tile = tempTile;

                    currentNode.gridPositionX = column;
                    currentNode.gridPositionZ = row;

                    this.mapGrid[row].Add(currentNode);

                    currentNode.Tile.transform.SetParent(BoardLevel.transform);
                    currentNode.Tile.name = "Tile_" + row + "_" + column;

                    column++;
                }

                column = 0;
                row++;
                //Debug.Log("--------------End of Line-------------");
            }

            loadPropMap(this.wallsFile, contentType.WALLS);
            loadPropMap(this.propFile, contentType.PROPS);
        }

        void loadPropMap(TextAsset contentFile, contentType Type)
        {
            string[] entries = contentFile.text.Split('\n');

            int column = 0;
            int mapColumn = 0;
            int row = 0;

            foreach (string line in entries)
            {
                string[] tiles = line.Split(' ');

                foreach (string tile in tiles)
                {
                    if (tile != "-1")
                    {
                        GameObject contentObj = null;
                        switch (Type)
                        {
                            case contentType.PROPS:
                                contentObj = createProp(column, row, int.Parse(tile));
                                break;
                            case contentType.WALLS:
                                //Debug.Log(int.Parse(tile));
                                contentObj = createWall(column, row, int.Parse(tile));
                                break;
                        }

                        this.mapGrid[row][column].addContent(contentObj);

                        mapColumn++;
                    }

                    column++;
                }

                mapColumn = 0;
                column = 0;
                row++;
                //Debug.Log("--------------End of Line-------------");
            }
        }

        GameObject createEmpty(int column, int row, int tile, bool initialPos = true)
        {
            Vector3 tempPosition = (initialPos)? startPosition : endPosition;

            tempPosition.x += columnSize.x * column;
            tempPosition.z += columnSize.y * row;

            GameObject square = new GameObject();

            square.transform.position = tempPosition;
            square.transform.rotation = Quaternion.Euler(Vector3.zero);

            return square;
        }

        GameObject createTile(int column, int row, int tile, bool initialPos = true)
        {
            Vector3 tempPosition = (initialPos)? startPosition : endPosition;

            tempPosition.x += columnSize.x * column;
            tempPosition.z += columnSize.y * row;

            GameObject square = Instantiate(squarePrefabs[tile], 
                tempPosition,
                Quaternion.Euler(Vector3.zero));

            return square;
        }

        GameObject createProp(int column, int row, int prop, bool initialPos = true)
        {
            Vector3 tempPosition = (initialPos)? startPosition : endPosition;

            tempPosition.x += columnSize.x * column;
            tempPosition.z += columnSize.y * row;

            GameObject square = Instantiate(propPrefabs[prop], 
                tempPosition,
                Quaternion.Euler(Vector3.zero));

            return square;
        }

        GameObject createWall(int column, int row, int prop, bool initialPos = true)
        {
            Vector3 tempPosition = (initialPos)? startPosition : endPosition;

            tempPosition.x += columnSize.x * column;
            tempPosition.z += columnSize.y * row;

            GameObject square = Instantiate(wallsPrefabs[prop], 
                tempPosition,
                Quaternion.Euler(Vector3.zero));

            //Debug.Log(prop + " - " + wallsPrefabs[prop], square);

            return square;
        }

        public Vector3 gridPosition(Vector3 posOriginal)
        {
            if (posOriginal.x < startPosition.x)
            {
                posOriginal.x = startPosition.x;
            }

            if (posOriginal.z < startPosition.z)
            {
                posOriginal.z = startPosition.z;
            }

            if (posOriginal.x >= startPosition.x + columnSize.x * (columnCount.x - 1))
            {
                posOriginal.x = startPosition.x + columnSize.x * (columnCount.x - 1);
            }

            if (posOriginal.y >= startPosition.z + columnSize.y * (columnCount.y - 1))
            {
                posOriginal.y = startPosition.z + columnSize.y * (columnCount.y - 1);
            }

            return startPosition + new Vector3( Mathf.Floor( posOriginal.x / columnSize.x ) * columnSize.x,
                                                0,
                Mathf.Round( posOriginal.z / columnSize.y ) * columnSize.y );
        }

        public bool hasTrap(Vector3 posTrap)
        {
            Vector3 gridPos = new Vector3(Mathf.Floor(posTrap.x / columnSize.x),
                                  0,
                Mathf.Round(posTrap.z / columnSize.y));

            if (gridPos.x < 0)
            {
                gridPos.x = 0;
            }

            if (gridPos.z < 0)
            {
                gridPos.z = 0;
            }

            if (gridPos.x >= columnCount.x)
            {
                gridPos.x = (columnCount.x - 1);
            }

            if (gridPos.z >= columnCount.y)
            {
                gridPos.z = (columnCount.y - 1);
            }

            Debug.Log(posTrap);

            if (this.mapGrid[(int)gridPos.z][(int)gridPos.x].Trap != null)
            {
                return true;
            }

            return false;
        }

        public bool hasTrap(Vector3 posTrap, Vector2 size)
        {
            Vector3 gridPos = new Vector3(Mathf.Floor(posTrap.x / columnSize.x),
                0,
                Mathf.Round(posTrap.z / columnSize.y));

            if (gridPos.x < 0)
            {
                gridPos.x = 0;
            }

            if (gridPos.z < 0)
            {
                gridPos.z = 0;
            }

            if (gridPos.x >= columnCount.x)
            {
                gridPos.x = (columnCount.x - 1);
            }

            if (gridPos.z >= columnCount.y)
            {
                gridPos.z = (columnCount.y - 1);
            }

            //Check if the object position + size is bigger than the board
            if (((int)gridPos.y + size.y - 1) >= columnCount.y)
            {
                return true;
            }

            if (((int)gridPos.x + size.x - 1) >= columnCount.x)
            {
                return true;
            }

            //check for objects in all nodes
            for (int countY = 0; countY < size.y; countY++)
            {
                for (int countX = 0; countX < size.x; countX++)
                {
                    if (this.mapGrid[(int)gridPos.z + countY][(int)gridPos.x + countX].Trap != null)
                    {
                        return true;
                    }  
                }
            }

            //check for objects in the current nodes
            if (this.mapGrid[(int)gridPos.z][(int)gridPos.x].Trap != null)
            {
                return true;
            }

            return false;
        }

        public void setTrap(Vector3 posTrap, GameObject trapObj, Vector2 size)
        {
			Node tempNode = this.GetNodeByPosition(posTrap);

            //check for objects in all nodes
            for (int countY = 0; countY < size.y; countY++)
            {
                for (int countX = 0; countX < size.x; countX++)
                {
					this.mapGrid[(int)tempNode.gridPositionZ + countY][(int)tempNode.gridPositionX + countX].setTrap(trapObj, false);
                }
            }

			tempNode.setTrap(trapObj, true);
		}

		public void setUnit(Vector3 posTrap, Unit unitObj)
		{
			Node tempNode = this.GetNodeByPosition(posTrap);

			tempNode.setUnit(unitObj);

			unitObj.currentNode = tempNode;
		}

        public Node GetNode(int positionX, int positionY, int positionZ)
        {
            if (positionY != 0)
            {
                return null;
            }

            if (positionX < 0 || positionZ < 0 || positionZ >= this.mapGrid.Count)
            {
                return null;
            }

            if (positionX >= this.mapGrid[positionZ].Count)
            {
                return null;
            }
            
            return this.mapGrid[positionZ][positionX];
        }

        public Node GetNode(float positionX, float positionY, float positionZ)
        {
            return this.GetNode((int)positionX, (int)positionY, (int) positionZ);
		}

		public Node GetNode(Vector3 position)
		{
			return this.GetNodeByPosition(position);
		}

        public Node GetNodeByPosition(Vector3 posOriginal)
        {
            Vector3 gridPos = new Vector3(Mathf.Floor(posOriginal.x / columnSize.x),
                0,
                Mathf.Round(posOriginal.z / columnSize.y));

            if (gridPos.x < 0)
            {
                gridPos.x = 0;
            }

            if (gridPos.z < 0)
            {
                gridPos.z = 0;
            }

            if (gridPos.x >= columnCount.x)
            {
                gridPos.x = (columnCount.x - 1);
            }

            if (gridPos.z >= columnCount.y)
            {
                gridPos.z = (columnCount.y - 1);
            }

            return this.mapGrid[(int)gridPos.z][(int)gridPos.x];
        }

		public void addNode(Node node)
		{
			this.mapGrid [(int)node.gridPositionZ] [(int)node.gridPositionX] = node;

            if (node.tags.IndexOf("Spawn") != -1)
            {
                this.spawnList.Add(node);
            }
            else if (node.tags.IndexOf("Conquer") != -1)
            {
                this.groupConquerNode(node);
            }
		}

        void groupConquerNode(Node newNode)
        {

            Debug.Log(this.conquerList.Count, newNode.gameObject);

            for(int count = 0; count < this.conquerList.Count; count++)
            {
                for (int countNode = 0; countNode < this.conquerList[count].Count; countNode++)
                {
                    if (this.conquerList[count][countNode].isConnected(newNode) && this.conquerList[count].IndexOf(newNode) == -1)
                    {
                        this.conquerList[count].Add((newNode));
                        return;
                    }
                }
            }

            this.conquerList.Add(new List<Node>());

            this.conquerList[this.conquerList.Count - 1].Add(newNode);

        }

        /*void BlurPenaltyMap(int blurSize)
        {
            int kernelSize = blurSize * 2 + 1;
            int kernelExtents = (kernelSize - 1) / 2;

            int[,] penaltiesHorizontalPass = new int[this.columnCount.x, this.columnCount.y];
            int[,] penaltiesVerticalPass = new int[this.columnCount.x, this.columnCount.y];

            for (int countY = 0; countY < this.columnCount.y; countY++)
            {
                for (int countX = -kernelExtents; countX <= kernelExtents; countX++)
                {
                    int sampleX = Mathf.Clamp(countX, 0, kernelExtents);

                    penaltiesHorizontalPass[0, countY] += this.Grid[sampleX][countY]
                }
            }
        }*/
    }
}