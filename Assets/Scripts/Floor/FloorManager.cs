using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Floor
{
    public class FloorManager : MonoBehaviour
    {
        [Header("Drag your FloorDatabase here")]
        public FloorDatabase floorDatabase;

        private List<string> visitedFloors = new List<string>();
        private string currentFloor = "";

        private bool isTransitioning = false;

        public void OnElevatorButtonPressed()
        {
            if (isTransitioning) return;

            if (visitedFloors.Count >= floorDatabase.floorSceneNames.Count)
            {
                Debug.Log("All floors visited");
                return;
            }

            StartCoroutine(ElevatorSequence());
        }

        IEnumerator ElevatorSequence()
        {
            isTransitioning = true;

            // Debug.Log("Elevator button pressed");

            float timer = 3f;

            while (timer > 0)
            {
                Debug.Log("Timer: " + timer);
                yield return new WaitForSeconds(1f);
                timer--;
            }

            // Debug.Log("Timer finished");
            // Debug.Log("Doors closing");

            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(LoadRandomFloor());

            isTransitioning = false;
        }

        IEnumerator LoadRandomFloor()
        {
            if (!string.IsNullOrEmpty(currentFloor))
            {
                yield return SceneManager.UnloadSceneAsync(currentFloor);
            }

            List<string> availableFloors = new List<string>();

            foreach (var floor in floorDatabase.floorSceneNames)
            {
                if (!visitedFloors.Contains(floor))
                    availableFloors.Add(floor);
            }

            if (availableFloors.Count == 0)
            {
                Debug.Log("All floors visited");
                yield break;
            }

            string nextFloor = availableFloors[Random.Range(0, availableFloors.Count)];

            visitedFloors.Add(nextFloor);
            currentFloor = nextFloor;

            Debug.Log("Loading: " + nextFloor);

            yield return SceneManager.LoadSceneAsync(nextFloor, LoadSceneMode.Additive);

            // Debug.Log("Doors opening");

            if (visitedFloors.Count >= floorDatabase.floorSceneNames.Count)
            {
                Debug.Log("All floors visited");
            }
        }

        public bool IsTransitioning()
        {
            return isTransitioning;
        }

    }
}