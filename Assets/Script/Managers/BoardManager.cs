using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<GameObject> tiles;
    public List<GameObject> triggers;

    public GameObject triggerHandler;
    private GameObject trigger;

    public GameObject areaHandler;
    private GameObject areas;

    private List<int> map;
    [HideInInspector] public List<string> filenames;

    private Dictionary<int, GameObject> toInstantiate;
    
	void Start ()
    {
        map = new List<int>();
        filenames = new List<string>();
        toInstantiate = new Dictionary<int, GameObject>();
        PopulateLists();

        trigger = Instantiate(triggerHandler) as GameObject;

        for (int i = 0; i < triggers.Count; i++)
            Instantiate(triggers[i]).transform.SetParent(trigger.transform, false);

        ReadFile(filenames[12], 13);
	}

    public void DestroyLevel(int area)
    {
        if (toInstantiate[area] == null)
            return;

        Destroy(toInstantiate[area]);
        toInstantiate[area] = null;
    }

    private void SetupLevel(int area)
    {
        int index = 0;
        float X = 0;
        float Y = 0;

        switch (area)
        {
            case 1: X = 0; Y = 128; break;
            case 2: X = 30; Y = 128; break;
            case 3: X = 60; Y = 128; break;
            case 4: X = 90; Y = 128; break;
            case 5: X = 0; Y = 96; break;
            case 6: X = 30; Y = 96; break;
            case 7: X = 60; Y = 96; break;
            case 8: X = 90; Y = 96; break;
            case 9: X = 0; Y = 64; break;
            case 10: X = 30; Y = 64; break;
            case 11: X = 60; Y = 64; break;
            case 12: X = 90; Y = 64; break;
            case 13: X = 0; Y = 32; break;
            case 14: X = 30; Y = 32; break;
            case 15: X = 60; Y = 32; break;
            case 16: X = 90; Y = 32; break;
        }

        areas = Instantiate(areaHandler) as GameObject;

        for (float i = 0; i < 25; i++)
            for (int j = 0; j < 25; j++)
            {
                Instantiate(tiles[map[index]], new Vector3(X + (j * 1.2f), Y - (1.28f * i), 0f), Quaternion.identity).transform.SetParent(areas.transform, false);
                index++;
            }

        toInstantiate[area] = areas;
        toInstantiate[area].name = "Area" + area;

    }

    private void PopulateLists()
    {
        filenames.Add("Assets/Files/area1.dat");
        filenames.Add("Assets/Files/area2.dat");
        filenames.Add("Assets/Files/area3.dat");
        filenames.Add("Assets/Files/area4.dat");
        filenames.Add("Assets/Files/area5.dat");
        filenames.Add("Assets/Files/area6.dat");
        filenames.Add("Assets/Files/area7.dat");
        filenames.Add("Assets/Files/area8.dat");
        filenames.Add("Assets/Files/area9.dat");
        filenames.Add("Assets/Files/area10.dat");
        filenames.Add("Assets/Files/area11.dat");
        filenames.Add("Assets/Files/area12.dat");
        filenames.Add("Assets/Files/area13.dat");
        filenames.Add("Assets/Files/area14.dat");
        filenames.Add("Assets/Files/area15.dat");
        filenames.Add("Assets/Files/area16.dat");


        toInstantiate.Add(1, null);
        toInstantiate.Add(2, null);
        toInstantiate.Add(3, null);
        toInstantiate.Add(4, null);
        toInstantiate.Add(5, null);
        toInstantiate.Add(6, null);
        toInstantiate.Add(7, null);
        toInstantiate.Add(8, null);
        toInstantiate.Add(9, null);
        toInstantiate.Add(10, null);
        toInstantiate.Add(11, null);
        toInstantiate.Add(12, null);
        toInstantiate.Add(13, null);
        toInstantiate.Add(14, null);
        toInstantiate.Add(15, null);
        toInstantiate.Add(16, null);
    }

    public void ReadFile(string filename, int area)
    {
        if ( toInstantiate[area] != null )
            return;

        string line;

        map.Clear();

        StreamReader theReader = new StreamReader(filename, Encoding.Default);
        
        do
        {
            line = theReader.ReadLine();

            if (line != null)
            {
                string[] entries = line.Split(' ');
                if (entries.Length > 0)
                    for (int i = 0; i < entries.Length; i++)
                        map.Add(int.Parse(entries[i]));
            }
        }
        while (line != null);
        theReader.Close();

        SetupLevel(area);
    }
}
