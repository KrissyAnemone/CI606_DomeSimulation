using UnityEngine;
using UnityEngine.InputSystem;

public class KD_PC_NpcMovement : MonoBehaviour
{
    public GameObject pcPrefab;
    public Camera npcCam;
    public Rigidbody rb;

    public float moveSpeed;
    public float lookSensitivity = 0.15f;

    private InputAction moveAction;
    private InputAction lookAction;

    private ZF_NPC_Pathfinding npcPathfinding;
    private KD_PC_NpcMovement npcMovement;

    private float pitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcPathfinding = GetComponent<ZF_NPC_Pathfinding>();
        npcMovement = GetComponent<KD_PC_NpcMovement>();

        moveSpeed = npcPathfinding.agent.speed;

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");

        lookAction = new InputAction("Look", InputActionType.Value);
        lookAction.AddBinding("<Mouse>/delta");

        moveAction.Enable();
        lookAction.Enable();

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

        Vector2 look = lookAction.ReadValue<Vector2>();

        pitch -= look.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.Rotate(Vector3.up * look.x * lookSensitivity);

        npcCam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void FixedUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        Vector3 movement = npcCam.transform.forward * move.y
                         + transform.right * move.x;

        if (Keyboard.current.spaceKey.isPressed)
            movement += Vector3.up;

        if (Keyboard.current.leftCtrlKey.isPressed)
            movement += Vector3.down;

        movement = Vector3.ClampMagnitude(movement, 1f);

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
