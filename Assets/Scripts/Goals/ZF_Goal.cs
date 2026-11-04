using UnityEngine;
using UnityEngine.UIElements;

public class ZF_Goal : MonoBehaviour
{
    private Vector3 endPos;
    [SerializeField] GameObject[] Normal_Waypoints;
    [SerializeField] GameObject[] Accessability_Waypoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endPos = transform.position;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public Vector3 GetPosition() { return endPos; } 
}
