using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace gridMaster
{
    namespace Pathfinding
    {
        public class PathfindingMaster : MonoBehaviour 
        {
            public static PathfindingMaster instance
            {
                get { return _instance; }//can also use just get;
                set { _instance = value; }//can also use just set;
            }

            //Creates a class variable to keep track of GameManger
            static PathfindingMaster _instance = null;

            [SerializeField]
            int maxJobs = 10;

            List<Pathfinding> currentJobs;
            List<Pathfinding> todoJobs;

            public delegate void PathfindingJobComplete(Stack<Node> path);

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

                currentJobs = new List<Pathfinding>();
                todoJobs = new List<Pathfinding>();
        		
        	}
        	
        	// Update is called once per frame
        	void Update () 
            {
                int i = 0;

                while(i < currentJobs.Count)
                {
                    if (currentJobs[i].jobDone)
                    {
                        currentJobs[i].NotifyComplete();
                        currentJobs.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                if (todoJobs.Count > 0 && currentJobs.Count < maxJobs)
                {
                    Pathfinding job = todoJobs[0];
                    todoJobs.RemoveAt(0);
                    currentJobs.Add(job);

                    Thread jobThread = new Thread(job.findPath);
                    jobThread.Start();
                }
        	}

            public void RequestPath(Node startNode, Node endNode, PathfindingMaster.PathfindingJobComplete callback)
            {
                Pathfinding newJob = new Pathfinding(startNode, endNode, callback);
                todoJobs.Add(newJob);
            }
        }
    }
}