using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gridMaster.environment;
using gridMaster;
using gridMaster.Pathfinding;

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
        List<List<Node>> conquerList;

        int unitsThisTurn = 0;
        int totalUnitsGame = 0;
        int currentUnit = 0;


    	// Use this for initialization
    	void Awake () 
        {
            this.leaders = new List<AILeader>();
            this.currState = AIState.Normal;

            float tempUnit = this.totalUnits;
            float tempPerGroup = this.unitsPerGroup;

            int totalgroups = Mathf.CeilToInt(tempUnit / tempPerGroup);

            for (int count = 0; count < totalgroups; count++)
            {
                this.leaders.Add(new AILeader());
                this.leaders[count].totalUnits = this.unitsPerGroup;
            }
    	}

        void Start () 
        {
            base.Start();

            this.ChangeState(AIState.Normal);
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


        public void ChangeState(AIState targetState)
        {
            currState = targetState;

            everyFrame = null;
            lateFrame = null;
            lateLateFrame = null;

            switch (targetState)
            {
                case AIState.Defensive:
                    break;
                case AIState.Normal:
                    lateFrame = updateLeaders;
                    lateLateFrame = checkLeaderConquerGroup;
                    break;
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
                return;
            }

            if (this.totalUnits - this.totalUnitsGame < unitsToCreate)
            {
                unitsToCreate = this.totalUnits - this.totalUnitsGame;
            }

			List<Node> tempSpawn = new List<Node>(GridMaster.instance.spawnList);

			Node posNode = null;

            for (int count = 0; count < unitsToCreate; count++)
            {
				while(tempSpawn.Count > 0 && posNode == null)
				{
					Node tempNode = tempSpawn[Random.Range(0, 100) % tempSpawn.Count];
					tempSpawn.Remove(tempNode);

					if(tempNode.currUnit == null)
					{
						posNode = tempNode;
						break;
					}
				}

				if (posNode != null)
				{
					GameObject unit = Instantiate(this.unitPrefab[this.currentUnit], transform);
					unit.transform.position = posNode.worldPosition + Vector3.up;
                    AIUnits tempUnit = unit.GetComponent<AIUnits>();

                    tempUnit.playerId = this.playerId;

					tempUnit.currentNode = posNode;

					gridMaster.GridMaster.instance.setUnit(unit.transform.position, tempUnit);

					if (this.leaders[currentGroup].isFull() && this.leaders[currentGroup].leader != null)
					{
						currentGroup++;
					}

					if (this.leaders[currentGroup].leader == null)
					{
                        this.leaders[currentGroup].leader = tempUnit;
					}
					else
					{
                        this.leaders[currentGroup].units.Add(tempUnit);
					}

					this.currentUnit++;
                    this.totalUnitsGame++;

					if (this.currentUnit >= this.unitPrefab.Length)
					{
						this.currentUnit = 0;
					}

					posNode = null;
				}
            }

            for (int count = 0; count < this.leaders.Count; count++)
            {
                this.leaders[count].updateUnits();
            }
        }

        void checkLeaderConquerGroup()
        {
            if (this.conquerList == null)
            {
                this.conquerList = GridMaster.instance.conquerList;
            }

            for (int count = 0; count < this.leaders.Count; count++)
            {
                if (this.leaders[count].conquer.conquerGroup.Count <= 0)
                {
                    float prevDist = -1;

                    for (int countList = 0; countList < this.conquerList.Count; countList++)
                    {
                        float dist = Vector3.Distance(this.leaders[count].leader.transform.position, this.conquerList[countList][0].worldPosition);

                        Debug.Log("Conquer Point -"+dist, this.conquerList[countList][0].gameObject);

                        if (dist < prevDist || prevDist == -1)
                        {
                            this.leaders[count].conquer.conquerGroup = this.conquerList[countList];
                            prevDist = dist;
                        }
                    }
                }
            }
        }

        void updateLeaders()
        {
            for (int count = 0; count < this.leaders.Count; count++)
            {
                this.leaders[count].Update();
            }
        }
    }
}