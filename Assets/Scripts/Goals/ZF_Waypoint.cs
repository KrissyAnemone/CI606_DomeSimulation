using UnityEngine;

public class ZF_Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        // Check if collided with PC
        if (!collision.gameObject.CompareTag("NPC")) return;
        ZF_NPC_Pathfinding npc = collision.gameObject.GetComponent<ZF_NPC_Pathfinding>();
        if (!npc) return;

        if (!npc.inGroup || npc.leader == npc.gameObject) // Continue if alone or leader
        {
            Vector3 destination = npc.agent.destination;
            destination.y = transform.position.y;

            if (destination != transform.position) return; // Continue if this waypoint was the destination of that npc

            // Get next waypoint and set it in npc
            Vector3 nextwaypoint = npc.goal.GetNextWaypoint(npc.isHandicapped, gameObject);
            npc.SetGoal(nextwaypoint);

            Debug.Log("NEXT");
        }
    }
}
