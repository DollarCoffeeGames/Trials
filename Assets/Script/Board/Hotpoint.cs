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
            [SerializeField]
            bool aiConquered = false;

            [SerializeField]
            Vector3 startPos;

            [SerializeField]
            Vector2 size;
        }
    }
}