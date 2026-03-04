using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Station
{
    public class StationManager : MonoBehaviour
    {
        [Header("Drag your StationDatabase here")]
        public StationDatabase stationDatabase;

        private List<string> visitedStations = new List<string>();
        private string currentStation = "";

        private bool isTransitioning = false;

        public void OnTrainButtonPressed()
        {
            if (isTransitioning) return;

            if (visitedStations.Count >= stationDatabase.stationSceneNames.Count)
            {
                Debug.Log("All stations visited");
                return;
            }

            StartCoroutine(TrainSequence());
        }

        IEnumerator TrainSequence()
        {
            isTransitioning = true;

            // Debug.Log("Train button pressed");

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

            yield return StartCoroutine(LoadRandomStation());

            isTransitioning = false;
        }

        IEnumerator LoadRandomStation()
        {
            if (!string.IsNullOrEmpty(currentStation))
            {
                yield return SceneManager.UnloadSceneAsync(currentStation);
            }

            List<string> availableStations = new List<string>();

            foreach (var station in stationDatabase.stationSceneNames)
            {
                if (!visitedStations.Contains(station))
                    availableStations.Add(station);
            }

            if (availableStations.Count == 0)
            {
                Debug.Log("All stations visited");
                yield break;
            }

            string nextStation = availableStations[Random.Range(0, availableStations.Count)];

            visitedStations.Add(nextStation);
            currentStation = nextStation;

            Debug.Log("Loading: " + nextStation);

            yield return SceneManager.LoadSceneAsync(nextStation, LoadSceneMode.Additive);

            // Debug.Log("Doors opening");

            if (visitedStations.Count >= stationDatabase.stationSceneNames.Count)
            {
                Debug.Log("All stations visited");
            }
        }

        public bool IsTransitioning()
        {
            return isTransitioning;
        }

    }
}