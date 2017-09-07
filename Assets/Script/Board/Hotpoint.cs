using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.Pathfinding;

namespace gridMaster
{
    namespace environment
    {
        [System.Serializable]
        public class Hotpoint 
        {
            public bool aiConquered = false;

            public List<Node> m_conquerGroup;

            public List<Node> conquerGroup
            {
                get
                {
                    return this.m_conquerGroup;
                }
                set
                {
                    this.m_conquerGroup = value;
                    this.conquerRemaining = new List<Node>(value);
                }
            }

            public List<Node> conquerRemaining;

            public Hotpoint()
            {
                this.conquerGroup     = new List<Node>();
                this.conquerRemaining = new List<Node>();
            }
        }
    }
}