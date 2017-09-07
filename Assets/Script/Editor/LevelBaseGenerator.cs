using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable] 
public class SerializableGUIMetaData {

    public string nodeLevelFile;
    public string prefabLevelFile;

    public GameObject[] nodesPrefab;
    public GameObject[] wallsPrefab;
    public GameObject[] propsPrefab;
    // etc
}
public class BaseLevelDataObject : ScriptableObject { // MonoBehaviour should also work if the instance is in the scene
    public SerializableGUIMetaData boxData = new SerializableGUIMetaData();

}

public class LevelBaseGenerator : EditorWindow 
{
    BaseLevelDataObject GUIBoxData;

    string nodeLevelFile;
    string prefabLevelFile;

    int currTab;

    int nodesPrefabSize = 0;
    GameObject[] nodesPrefab = new GameObject[0];
    Vector2 scrollPositionNodes;

    int wallsPrefabSize = 0;
    GameObject[] wallsPrefab = new GameObject[0];
    Vector2 scrollPositionWalls;

    int propsPrefabSize = 0;
    GameObject[] propsPrefab = new GameObject[0];
    Vector2 scrollPositionProps;

    [MenuItem("Tools/Base Level Creator")]
	// Use this for initialization
	public static void showWindow () 
    {
        EditorWindow.GetWindow<LevelBaseGenerator>("Base Level Creator");	
	}

    void OnEnable()
    {
        //NewAssetss
/*        this.GUIBoxData = ScriptableObject.CreateInstance<BaseLevelDataObject> ();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath ("Script/Editor/Data_" + typeof(BaseLevelDataObject).ToString() + ".asset");

        Debug.Log(assetPathAndName+" - "+typeof(BaseLevelDataObject).ToString());
        AssetDatabase.CreateAsset (this.GUIBoxData, assetPathAndName);
        AssetDatabase.SaveAssets ();*/
    }

    void OnDisable()
    {
        /*if (GUIBoxData != null)
        {
            OnEnable();
        }

        GUIBoxData.boxData.nodeLevelFile = this.nodeLevelFile;
        GUIBoxData.boxData.prefabLevelFile = this.prefabLevelFile;
        GUIBoxData.boxData.nodesPrefab = this.nodesPrefab;
        GUIBoxData.boxData.wallsPrefab = this.wallsPrefab;
        GUIBoxData.boxData.propsPrefab = this.propsPrefab;

        EditorUtility.SetDirty(GUIBoxData); // This will make it "save" for you*/
    }
	
    void OnGUI()
    {
        #region MAP_FILES
        EditorGUILayout.LabelField("Map Files", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        this.nodeLevelFile = EditorGUILayout.TextField("Node File", this.nodeLevelFile);

        if (GUILayout.Button("..."))
        {
            this.nodeLevelFile = EditorUtility.OpenFolderPanel("Node File", "", "");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        this.prefabLevelFile = EditorGUILayout.TextField("Prefab File", this.prefabLevelFile);

        if (GUILayout.Button("..."))
        {
            this.prefabLevelFile = EditorUtility.OpenFolderPanel("Prefab File", "", "");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        this.prefabLevelFile = EditorGUILayout.TextField("Walls File", this.prefabLevelFile);

        if (GUILayout.Button("..."))
        {
            this.prefabLevelFile = EditorUtility.OpenFolderPanel("Walls File", "", "");
        }
        GUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.LabelField("Prefabs", EditorStyles.boldLabel);

        this.currTab = GUILayout.Toolbar (this.currTab, new string[] {"Nodes", "Walls", "Props"});

        switch(this.currTab)
        {
            case 0:
                GUILayout.BeginHorizontal();

                this.nodesPrefabSize = EditorGUILayout.IntField("Size Prefabs", this.nodesPrefabSize);

                if (this.nodesPrefabSize != this.nodesPrefab.Length)
                {
                    this.nodesPrefab = new GameObject[nodesPrefabSize];
                }

                this.scrollPositionNodes = EditorGUILayout.BeginScrollView(this.scrollPositionNodes);

                for (int count = 0; count < this.nodesPrefab.Length; count++)
                {
                    this.nodesPrefab[count] = (GameObject)EditorGUILayout.ObjectField(this.nodesPrefab[count], typeof(GameObject), true);
                    //this.nodesPrefab[count] = EditorGUILayout.ObjectField( this.nodesPrefab[count].name, this.nodesPrefab[count], typeof( GameObject ) );
                } 

                EditorGUILayout.EndScrollView();
                GUILayout.EndHorizontal();

                break;
            case 1:
                GUILayout.BeginHorizontal();

                this.wallsPrefabSize = EditorGUILayout.IntField("Size Prefabs", this.wallsPrefabSize);

                if (this.wallsPrefabSize != this.wallsPrefab.Length)
                {
                    this.wallsPrefab = new GameObject[wallsPrefabSize];
                }

                this.scrollPositionWalls = EditorGUILayout.BeginScrollView(this.scrollPositionWalls);

                for (int count = 0; count < this.wallsPrefab.Length; count++)
                {
                    this.wallsPrefab[count] = (GameObject)EditorGUILayout.ObjectField(this.wallsPrefab[count], typeof(GameObject), true);
                    //this.nodesPrefab[count] = EditorGUILayout.ObjectField( this.nodesPrefab[count].name, this.nodesPrefab[count], typeof( GameObject ) );
                } 

                EditorGUILayout.EndScrollView();
                GUILayout.EndHorizontal();

                break;
            case 2:
                GUILayout.BeginHorizontal();

                this.propsPrefabSize = EditorGUILayout.IntField("Size Prefabs", this.propsPrefabSize);

                if (this.propsPrefabSize != this.propsPrefab.Length)
                {
                    this.propsPrefab = new GameObject[propsPrefabSize];
                }

                this.scrollPositionProps = EditorGUILayout.BeginScrollView(this.scrollPositionProps);

                for (int count = 0; count < this.propsPrefab.Length; count++)
                {
                    this.propsPrefab[count] = (GameObject)EditorGUILayout.ObjectField(this.propsPrefab[count], typeof(GameObject), true);
                    //this.nodesPrefab[count] = EditorGUILayout.ObjectField( this.nodesPrefab[count].name, this.nodesPrefab[count], typeof( GameObject ) );
                } 

                EditorGUILayout.EndScrollView();
                GUILayout.EndHorizontal();

                break;
        }

        /*folderPath = EditorGUILayout.TextField("Folder Name", folderPath);*/

        if (GUILayout.Button("Create"))
        {
            //takeScreenShoot();
        }
    }
}
