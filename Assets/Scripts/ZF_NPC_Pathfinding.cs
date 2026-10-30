using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class ZF_NPC_Pathfinding : MonoBehaviour
{
    GameObject Goal;
    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //if (Goal) agent.SetDestination(Goal.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (math.trunc(Time.time) % 2 == 0)
        {
            if (Vector3.Distance(transform.position, Goal.transform.position) < 1.5)
            {
                agent.isStopped = true;
            }
        }*/
    }

    public void SetGoal(GameObject newGoal)
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        Goal = newGoal;
        if (Goal) agent.SetDestination(Goal.transform.position);
    }

}
