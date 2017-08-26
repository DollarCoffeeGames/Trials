using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class AILeader {

        public GameObject leader = null;
        public int totalUnits;
        public List<GameObject> units;

    	// Use this for initialization
        public AILeader() 
        {
            this.units = new List<GameObject>();
    	}
    	
    	// Update is called once per frame
    	void Update () {
    		
    	}

        public bool isFull()
        {
            if (this.units.Count >= (this.totalUnits - 1))
            {
                return true;
            }

            return false;
        }
    }
}
