using Controller;
using InventorySystem;
using UnityEngine;

namespace interactionSystem
{
    public class GiveItem : MonoBehaviour, IInteractable
    {
        [SerializeField] string text;
        [SerializeField] string itemID;
        public void Interact(PlayerController controller)
        {
            ItemRegistery.instance.AddItem(itemID, controller.inventory);
            Debug.Log(text);
            Destroy(gameObject);
        }
    }
}
