using Controller;
using interactionSystem;
using UnityEngine;

namespace Inventory
{
    public class Pickupable : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController controller)
        {
            controller.givePPUD().setItem(gameObject);
        }
    }
}
