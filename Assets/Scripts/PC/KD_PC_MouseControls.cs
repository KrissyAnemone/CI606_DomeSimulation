using UnityEngine;
using UnityEngine.InputSystem;

public class KD_PC_MouseControls : MonoBehaviour
{
    public float possessionHeight = .5f;

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("NPC"))
                {
                    GameObject npc = hit.collider.gameObject;

                    transform.position = npc.transform.position + Vector3.up * possessionHeight;

                    KD_PC_NpcMovement npcMovement = npc.GetComponent<KD_PC_NpcMovement>();

                    if (npcMovement != null)
                    {
                        npcMovement.enabled = true;
                        if (npcMovement.npcCam != null)
                        {
                            Debug.Log("Hello");
                            npcMovement.npcCam.enabled = true;
                        }
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
