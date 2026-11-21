using UnityEngine;
using UnityEngine.InputSystem;

public class KD_PC_CamMovement : MonoBehaviour
{
    public Camera cam;

    public float moveSpeed = 10f;
    public float lookSensitivity = 0.15f;

    private InputAction moveAction;
    private InputAction lookAction;

    private float pitch;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s");

        lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
    }

    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector2 look = lookAction.ReadValue<Vector2>();

        Vector3 movement = cam.transform.forward * move.y + transform.right * move.x;

        if (Keyboard.current.spaceKey.isPressed)
            movement += Vector3.up;

        if (Keyboard.current.leftCtrlKey.isPressed)
            movement += Vector3.down;

        transform.position += movement * moveSpeed * Time.deltaTime;

        pitch -= look.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.Rotate(Vector3.up * look.x * lookSensitivity);

        cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}