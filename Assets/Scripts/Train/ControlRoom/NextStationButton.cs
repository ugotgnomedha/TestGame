using Controller;
using interactionSystem;
using UnityEngine;

namespace Station
{
    public class TrainButton : MonoBehaviour, IInteractable
    {
        public StationManager stationManager;

        public void Interact(PlayerController controller)
        {
            stationManager.OnTrainButtonPressed();
        }
    }
}