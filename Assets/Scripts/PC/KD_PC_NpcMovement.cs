using UnityEngine;
using UnityEngine.InputSystem;

public class KD_PC_NpcMovement : MonoBehaviour
{
    public GameObject pcPrefab;
    public Camera npcCam;

    public float speed;

    private ZF_NPC_Pathfinding npcPathfinding;
    private KD_PC_NpcMovement npcMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcPathfinding = GetComponent<ZF_NPC_Pathfinding>();
        npcMovement = GetComponent<KD_PC_NpcMovement>();

        speed = npcPathfinding.agent.speed;

        npcPathfinding.enabled = false;
        npcPathfinding.agent.enabled = false;

        npcCam = GetComponentInChildren<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            npcCam.enabled = false;
            npcPathfinding.enabled = true;
            npcMovement.enabled = false;
            npcPathfinding.agent.enabled = true;

            Instantiate(pcPrefab, transform.position, transform.rotation);
        }
    }
}
