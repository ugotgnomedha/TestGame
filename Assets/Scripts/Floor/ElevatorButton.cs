using Controller;
using interactionSystem;
using UnityEngine;

namespace Floor
{
    public class ElevatorButton : MonoBehaviour, IInteractable
    {
        public FloorManager floorManager;

        public void Interact(PlayerController controller)
        {
            floorManager.OnElevatorButtonPressed();
        }
    }
}
