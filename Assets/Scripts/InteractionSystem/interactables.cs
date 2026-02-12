using Controller;
using UnityEngine;

namespace interactionSystem
{
    public interface IInteractable
    {
        public void Interact(PlayerController controller);
    }
}

