using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class ZF_Goal : MonoBehaviour
{
    private Vector3 endPos;
    [SerializeField] GameObject[] Normal_Waypoints;
    [SerializeField] GameObject[] Accessibility_Waypoints;

    [SerializeField] LayerMask LayerToMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LayerToMask = ~LayerToMask;
        endPos = transform.position;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public Vector3 GetPosition() { return endPos; }

    GameObject[] GetWaypointArray(bool isHandicapped) // Get waypoint array
    {
        GameObject[] waypointList;
        if (isHandicapped) waypointList = Accessibility_Waypoints;
        else waypointList = Normal_Waypoints;

        if (Normal_Waypoints.Length == 0) waypointList = Accessibility_Waypoints; // Default to accessible if there is no seperate normal path
        return waypointList;
    }

    public Vector3 GetNextWaypoint(bool isHandicapped, GameObject currentWP)
    {
        GameObject[] waypointList = GetWaypointArray(isHandicapped);

        int index = FindWP(waypointList, currentWP);
        if (index == -1)
        {
            Debug.Log("Error: Next waypoint not found");
            return endPos;
        }
        else
        {
            if (index == waypointList.Length - 1) return endPos; // Go to goal
            return waypointList[index+1].transform.position; // Go to next waypoint
        }
    }

    public int FindWP(GameObject[] wpAr, GameObject find)
    {
        for (int i = 0; i < wpAr.Length; i++)
        {
            if (wpAr[i] == find) return i; // Found index of waypoint
        }
        return -1; // Couldn't find waypoint in array
    }

    public GameObject GetFirstWaypoint(ZF_NPC_Pathfinding pathfinding)
    {
        GameObject npc = pathfinding.gameObject;
        GameObject[] waypointAr = GetWaypointArray(pathfinding.isHandicapped);

        for (int i = waypointAr.Length-1; i >= 0; i--) // Start from final waypoint and work back
        {
            GameObject waypoint = waypointAr[i]; // Waypoint
            if (WaypointInSight(npc, waypoint))
            {
                return waypoint;
            }
        }

        Debug.Log("Default to first wp");
        return waypointAr[0];
    }

    bool WaypointInSight(GameObject npc, GameObject waypoint)
    {
        Vector3 startPos = npc.transform.position; // Npc position
        Vector3 endPos = waypoint.transform.position; // Waypoint position

        Vector3 direction = (endPos - startPos).normalized; // Direction from npc to waypoint

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, Mathf.Infinity, LayerToMask))
        {
            if (hit.collider.gameObject == waypoint)
            {
                return true;
            }
        }
        return false;
    }
}
