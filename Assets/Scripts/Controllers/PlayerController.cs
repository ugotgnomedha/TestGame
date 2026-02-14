using InputSystem;
using interactionSystem;
using InventorySystem;
using Movement;
using UnityEngine;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        //Cache
        private InputSystem_Actions inputActions;
        private PlayerMovement movement;
        private InterManager interManager;
        public Inventory inventory;
        private Rigidbody rb;
        private Collider col;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            inputActions = new InputSystem_Actions();
            interManager = GetComponent<InterManager>();
            inventory = GetComponent<Inventory>();
            movement = GetComponent<PlayerMovement>();
            movement.SetUp(rb,col);
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
            bool runPressed = inputActions.Player.Sprint.IsPressed();

            // Pass input into movement script
            movement.ReceiveInput(moveInput, jumpPressed, runPressed);

            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                interManager.Interact(col,this);
            }

            if (inputActions.Player.DropItem.WasPressedThisFrame())
            {
                inventory.DropItem();
            }

            if (inputActions.Player.Attack.WasPressedThisFrame())
            {
                inventory.UseEquipedItem();
            }
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
