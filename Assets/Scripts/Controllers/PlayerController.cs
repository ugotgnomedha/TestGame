using InputSystem;
using interactionSystem;
using Inventory;
using Movement;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        // Cache
        private InputSystem_Actions inputActions;
        private PlayerMovement movement;
        private InterManager interManager;
        private PlayerPickUpDrop playerPickUpDrop;
        private Rigidbody rb;
        private Collider col;

        [Header("FPS UI Raycast")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float uiInteractDistance = 5f;
        [SerializeField] private LayerMask uiLayer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            inputActions = new InputSystem_Actions();
            interManager = GetComponent<InterManager>();
            playerPickUpDrop = GetComponent<PlayerPickUpDrop>();
            movement = GetComponent<PlayerMovement>();
            movement.SetUp(rb, col);

            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            movement.CursorLock();
        }

        private void Update()
        {
            // Movement input
            Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
            bool jumpPressed = inputActions.Player.Jump.WasPressedThisFrame();
            bool runPressed = inputActions.Player.Sprint.IsPressed();
            movement.ReceiveInput(moveInput, jumpPressed, runPressed);

            // Interact
            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                interManager.Interact(col, this);
            }
            RaycastUI();

        }

        private void RaycastUI()
        {
            if (playerCamera == null) return;

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, uiInteractDistance, uiLayer))
            {
                Button button = hit.collider.GetComponent<Button>();
                if (button != null)
                    button.onClick.Invoke();
            }
        }

        public PlayerPickUpDrop givePPUD()
        {
            return playerPickUpDrop;
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