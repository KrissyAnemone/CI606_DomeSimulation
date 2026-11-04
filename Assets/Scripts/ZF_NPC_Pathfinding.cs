using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class ZF_NPC_Pathfinding : MonoBehaviour
{
    ZF_Goal Goal;
    NavMeshAgent agent;
    public int npcID;
    public List<ZF_NPC_Pathfinding> npcGroup;
    public bool inGroup;
    public GameObject leader;

    private NavMeshAgent leaderAgent;

    // Editor Variables
    [Header("Editor Variables")]
    [SerializeField] float startSpeed;
    [SerializeField] float LeaderWaitSpeedReduction = 2;
    [SerializeField] float FollowerCatchupSpeedIncrease = 3;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = startSpeed;
    }

    public void SetGoal(GameObject newGoal) // By GameObject
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (Goal) agent.SetDestination(newGoal.transform.position);
    }

    public void SetGoal(ZF_Goal newGoal) // By Goal Script
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        Goal = newGoal;
        if (Goal) agent.SetDestination(Goal.transform.position);
    }
    
    private void Update()
    {
        if (inGroup && leader && leader != gameObject)
        {
            SetGoal(leader);
        }
        if (leader == gameObject)
        {
            if (LeaderTooAhead(2)) agent.speed = startSpeed - LeaderWaitSpeedReduction;
            else agent.speed = startSpeed;
        }
    }

    public void UpdateLeader() // Set leader of group as slowest
    {
        if (!leader) leader = npcGroup[0].gameObject;

        float leaderSpeed = leader.GetComponent<NavMeshAgent>().speed;
        GameObject slowestMember = leader;
        foreach (ZF_NPC_Pathfinding npc in npcGroup)
        {
            float currentSpeed = npc.agent.speed;
            if (currentSpeed < leaderSpeed)
            {
                leaderSpeed = currentSpeed;
                slowestMember = npc.gameObject;
            }
        }

        leader = slowestMember;
        leaderAgent = leader.GetComponent<NavMeshAgent>();

        foreach (ZF_NPC_Pathfinding npc in npcGroup)
        {
            npc.leader = leader;
            npc.leaderAgent = leaderAgent;
        }
    }

    public void AddToGroup(ZF_NPC_Pathfinding addAI)
    {
        List<ZF_NPC_Pathfinding> currentList = npcGroup.ToList();
        foreach (ZF_NPC_Pathfinding ai in currentList)
        {
            ai.npcGroup.Add(addAI);
        }
        addAI.inGroup = true;
        addAI.npcGroup = npcGroup.ToList();
    }

    public void TriggerGroupLeaderCatchup()
    {
        foreach (ZF_NPC_Pathfinding npc in npcGroup)
        {
            if (npc.gameObject != leader) npc.agent.speed = leader.GetComponent<ZF_NPC_Pathfinding>().startSpeed + FollowerCatchupSpeedIncrease;
            Debug.Log(leader.GetComponent<ZF_NPC_Pathfinding>().startSpeed);
        }
    }

    // Slow down if leader and too far
    bool LeaderTooAhead(float maxDistance)
    {
        float furthest = 0;
        foreach (ZF_NPC_Pathfinding ai in npcGroup)
        {
            if (ai == this) continue;

            float distance = Vector3.Distance(transform.position, ai.transform.position);
            if (distance > furthest) furthest = distance;
            if (furthest > maxDistance) return true;
        }

        return furthest > maxDistance;
    }
}
