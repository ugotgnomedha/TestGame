using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorButton : MonoBehaviour
{
    public FloorManager floorManager;
    public Camera playerCamera;
    public float maxDistance = 3f;

    private Keyboard keyboard;

    private void Awake()
    {
        keyboard = Keyboard.current;
    }

    void Update()
    {
        if (keyboard == null) return;

        if (keyboard.eKey.wasPressedThisFrame)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    if (!floorManager.IsTransitioning())
                    {
                        // Debug.Log("Button pressed");
                        floorManager.OnElevatorButtonPressed();
                    }
                    else
                    {
                        // Debug.Log("Button ignored");
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * maxDistance);
    }
}
