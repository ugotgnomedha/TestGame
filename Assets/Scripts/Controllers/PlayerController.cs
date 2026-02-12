using InputSystem;
using Movement;
using UnityEngine;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        //Cache
        private InputSystem_Actions inputActions;
        private PlayerMovement movement;

        private void Awake()
        {
            inputActions = new InputSystem_Actions();
            movement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            movement.CursorLock();
        }

        private void Update()
        {
            // Read input
            Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
            bool jumpPressed = inputActions.Player.Jump.WasPressedThisFrame();

            // Pass input into movement script
            movement.ReceiveInput(moveInput, jumpPressed);
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
    }
}
