using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        private float speed;

        //Cache
        private Camera mainCam;
        private Rigidbody rb;
        private Collider col;

        //Input
        private Vector2 moveInput;
        private bool jumpPressed;

        //Rotation
        [SerializeField] float rotateSpeed = 15f;
        [SerializeField] bool useRotSpeed = false;

        //Move
        [SerializeField] private float groundWalkSpeed = 25;
        [SerializeField] private float airSpeed = 6;

        //Jump
        [SerializeField] private float jumpForce = 8;

        //Gravity
        [SerializeField] private float gravity = 2.5f;

        //Ground Check
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask Ground;
        private bool isGrounded;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            speed = isGrounded ? groundWalkSpeed : airSpeed;

            ApplyGravity();

            GroundCheck();

            Rotate();

            Jump();
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void SetUp(Rigidbody rigidBody,Collider collider)
        {
            rb = rigidBody;
            col = collider;
        }

        public void ReceiveInput(Vector2 move, bool jump)
        {
            moveInput = move;
            jumpPressed = jump;
        }

        public void ApplyGravity()
        {
            if (!isGrounded)
            {
                Vector3 force = new Vector3(0, -gravity, 0);
                rb.AddForce(force, ForceMode.Acceleration);
            }
        }

        private void Rotate()
        {
            Vector3 camForward = mainCam.transform.forward;
            camForward.y = 0f;

            if (camForward.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRot = Quaternion.LookRotation(camForward);

            Quaternion newRot = useRotSpeed ? Quaternion.Slerp(rb.rotation, targetRot, rotateSpeed * Time.deltaTime) : targetRot;

            rb.MoveRotation(newRot);

        }

        private void Move()
        {
            Vector3 move = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
            rb.AddForce(move * speed, ForceMode.Acceleration);
        }

        public void Jump()
        {
            if (isGrounded && jumpPressed)
            {
                Vector3 v = rb.linearVelocity;
                v.y = jumpForce;
                rb.linearVelocity = v;
            }
        }

        private void GroundCheck()
        {
            Bounds b = col.bounds;
            Vector3 spherePos = new Vector3(b.center.x, b.min.y, b.center.z);

            Collider[] hits = Physics.OverlapSphere(spherePos, groundCheckRadius, Ground, QueryTriggerInteraction.Ignore);

            isGrounded = false;
            foreach (var hit in hits)
            {
                if (hit == col) continue;
                isGrounded = true;
                break;
            }
        }

        public void CursorLock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void CursorUnlock()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
