using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.environment;

namespace AI
{
    public class AIManager : PlayerTemplate 
    {

        public enum AIState
        {
            Normal,
            Defensive,
            Offensive
        }

        [SerializeField]
        AIState currState;

        [SerializeField]
        List<Transform> destList;

        List<Unit> myUnits;

        [Header("Update Interval")]
        [SerializeField]
        int lFrame = 15;

        [SerializeField]
        int lLateFrame = 35;

        int lFrame_Count = 0;
        int lLateFrame_Count = 0;

        delegate void EveryFrame();
        EveryFrame everyFrame;

        delegate void LateFrame();
        LateFrame lateFrame;

        delegate void LateLateFrame();
        LateLateFrame lateLateFrame;

        [Header("Hot Spots")]
        [SerializeField]
        Hotpoint[] hotpoints;

        [Header("Units")]
        [SerializeField]
        int totalUnits = 40;

        [SerializeField]
        int unitsPerGroup = 5;

        [SerializeField]
        int unitsPerTurn = 3;

        [SerializeField]
        GameObject[] unitPrefab;

        List<AILeader> leaders;

        int unitsThisTurn = 0;
        int totalUnitsGame = 0;
        int currentUnit = 0;


    	// Use this for initialization
    	void Awake () 
        {
            this.leaders = new List<AILeader>();
            this.currState = AIState.Normal;

            int totalgroups = this.totalUnits / this.unitsPerGroup;

            for (int count = 0; count < totalgroups; count++)
            {
                this.leaders.Add(new AILeader());
                this.leaders[count].totalUnits = this.unitsPerGroup;
            }
    	}

        void Start () 
        {
            base.Start();

            this.playerId = turnMaster.instance.registerPlayer(this);
        }
    	
    	// Update is called once per frame
    	void Update () 
        {
            if (everyFrame != null)
            {
                everyFrame();
            }

            if (lFrame_Count > lFrame)
            {
                if (lateFrame != null)
                {
                    lateFrame();
                }

                lFrame_Count = 0;
            }
                
            if (lLateFrame_Count > lLateFrame)
            {
                if(lateLateFrame != null)
                {
                    lateLateFrame();
                }

                lLateFrame_Count = 0;
            }

            lFrame_Count++;
            lLateFrame_Count++;
    	}

        void monitorState()
        {
            switch (this.currState)
            {
                case AIState.Defensive:
                case AIState.Normal:
                case AIState.Offensive:
                    break;
            }
        }

        public override void startTurn()
        {
            this.resourceAmount = turnMaster.instance.playerResourceAmount;

            this.unitsThisTurn = 0;

            int unitsToCreate = this.unitsPerTurn;
            int currentGroup = 0;


            if (this.totalUnitsGame >= totalUnits)
            {
                unitsToCreate = 0;
            }

            for (int count = 0; count < unitsToCreate; count++)
            {
                GameObject unit = Instantiate(this.unitPrefab[this.currentUnit], transform);

                Debug.Log(this.leaders.Count + " - " + currentGroup+" - "+this.leaders[currentGroup].isFull()+" - "+this.leaders[currentGroup].leader);

                if (this.leaders[currentGroup].isFull() && this.leaders[currentGroup].leader != null)
                {
                    currentGroup++;
                }

                if (this.leaders[currentGroup].leader == null)
                {
                    this.leaders[currentGroup].leader = unit;
                }
                else
                {
                        this.leaders[currentGroup].units.Add(unit);
                }

                this.currentUnit++;

                if (this.currentUnit >= this.unitPrefab.Length)
                {
                    this.currentUnit = 0;
                }
            }
        }
    }
}