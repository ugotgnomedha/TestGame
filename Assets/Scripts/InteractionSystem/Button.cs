using Controller;
using UnityEngine;

namespace interactionSystem
{
    public class Button : MonoBehaviour, IInteractable
    {
        [SerializeField] string text;
        public void Interact(PlayerController controller)
        {
            Debug.Log(text);
        }
    }
}
