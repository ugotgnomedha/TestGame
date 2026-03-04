using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ControlRoomManager : MonoBehaviour
{
    [Header("RenderTexture to display cameras")]
    public RenderTexture screenTexture;

    [Header("Quad/MeshRenderer that will display the camera feed")]
    public MeshRenderer screenRenderer;

    [Header("UI Text to show camera info")]
    public TMP_Text cameraInfoText;

    [Header("UI Button Examples (assign inactive example buttons)")]
    public GameObject cameraButtonExample;
    public RectTransform camerasParent;
    public int cameraRows = 1;
    public int cameraColumns = 5;

    public GameObject doorButtonExample;
    public RectTransform doorsParent;
    public int doorRows = 1;
    public int doorColumns = 5;

    private List<GameObject> cameraButtonClones = new List<GameObject>();
    private List<GameObject> doorButtonClones = new List<GameObject>();

    private int lastDoorCount = -1;
    private float doorPollTimer = 0f;
    private const float doorPollInterval = 0.5f;

    private int currentCameraIndex = 0;
    private StationCamera activeCamera;
    private List<StationCamera> cameras = new List<StationCamera>();

    private void Start()
    {
        if (screenRenderer == null || screenTexture == null)
        {
            Debug.LogError("ControlRoomManager: screenRenderer or screenTexture not assigned!");
            return;
        }

        screenRenderer.material.mainTexture = screenTexture;

        if (CameraRegistry.HasInstance)
        {
            CameraRegistry.Instance.OnCameraRegistered += OnCameraRegistered;
            CameraRegistry.Instance.OnCameraUnregistered += OnCameraUnregistered;

            var existing = CameraRegistry.Instance.GetActiveCameras();
            foreach (var cam in existing)
                OnCameraRegistered(cam);
        }

        UpdateDoorButtonsImmediate();
        UpdateCameraList();
        UpdateCameraButtons();
    }

    private void OnDestroy()
    {
        if (CameraRegistry.HasInstance)
        {
            CameraRegistry.Instance.OnCameraRegistered -= OnCameraRegistered;
            CameraRegistry.Instance.OnCameraUnregistered -= OnCameraUnregistered;
        }
    }

    private void OnCameraRegistered(StationCamera cam)
    {
        UpdateCameraList();
        UpdateCameraButtons();

        if (activeCamera == null && cameras.Count > 0)
        {
            currentCameraIndex = 0;
            ActivateCamera(cameras[0]);
        }
        else
        {
            UpdateUI();
        }
    }

    private void OnCameraUnregistered(StationCamera cam)
    {
        UpdateCameraList();
        UpdateCameraButtons();

        if (cam == activeCamera)
        {
            if (cameras.Count > 0)
            {
                currentCameraIndex = Mathf.Clamp(currentCameraIndex, 0, cameras.Count - 1);
                ActivateCamera(cameras[currentCameraIndex]);
            }
            else
            {
                activeCamera = null;
                UpdateUI();
            }
        }
    }

    private void UpdateCameraList()
    {
        cameras = CameraRegistry.Instance.GetActiveCameras();
    }

    private void ActivateCamera(StationCamera cam)
    {
        if (cam == null || cam.cameraComponent == null)
            return;

        foreach (var c in cameras)
        {
            if (c.cameraComponent != null)
            {
                c.cameraComponent.targetTexture = null;
                c.cameraComponent.enabled = false;
            }
        }

        cam.cameraComponent.enabled = true;
        cam.cameraComponent.targetTexture = screenTexture;
        activeCamera = cam;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (cameraInfoText != null)
        {
            if (activeCamera != null)
            {
                int total = cameras.Count;
                string camName = activeCamera.cameraID;
                cameraInfoText.text = $"Camera {currentCameraIndex + 1}/{total} - {camName}";
            }
            else
            {
                cameraInfoText.text = "No cameras available";
            }
        }
    }

    public void ShowNextCamera()
    {
        if (cameras.Count == 0) return;
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;
        ActivateCamera(cameras[currentCameraIndex]);
    }

    public void ShowPreviousCamera()
    {
        if (cameras.Count == 0) return;
        currentCameraIndex--;
        if (currentCameraIndex < 0) currentCameraIndex = cameras.Count - 1;
        ActivateCamera(cameras[currentCameraIndex]);
    }

    public void ToggleDoor(int index)
    {
        var doors = DoorRegistry.Instance.GetDoors();
        if (index < 0 || index >= doors.Count) return;
        doors[index].ToggleDoor();
    }

    private void Update()
    {
        doorPollTimer += Time.deltaTime;
        if (doorPollTimer >= doorPollInterval)
        {
            doorPollTimer = 0f;
            UpdateDoorButtonsIfNeeded();
        }
    }

    public void ShowCameraAtIndex(int index)
    {
        if (index < 0 || index >= cameras.Count) return;
        currentCameraIndex = index;
        ActivateCamera(cameras[index]);
    }

    private void UpdateCameraButtons()
    {
        if (cameraButtonExample == null || camerasParent == null)
            return;

        foreach (var go in cameraButtonClones)
            if (go != null) Destroy(go);
        cameraButtonClones.Clear();

        int camCount = cameras?.Count ?? 0;
        int camCapacity = Mathf.Max(1, cameraRows) * Mathf.Max(1, cameraColumns);

        if (camCount == 0)
        {
            cameraButtonExample.SetActive(false);
            return;
        }

        cameraButtonExample.SetActive(true);
        cameraButtonExample.transform.SetParent(camerasParent, false);

        RectTransform exampleRT = cameraButtonExample.GetComponent<RectTransform>();
        Vector2 cellSize = exampleRT != null ? exampleRT.sizeDelta : new Vector2(100, 30);
        Vector2 basePos = exampleRT != null ? exampleRT.anchoredPosition : Vector2.zero;
        Vector2 spacing = new Vector2(cellSize.x, cellSize.y);

        ConfigureButtonForCamera(cameraButtonExample, 0);

        int toCreate = Mathf.Min(camCount, camCapacity) - 1;
        for (int i = 0; i < toCreate; i++)
        {
            int camIndex = i + 1;
            GameObject clone = Instantiate(cameraButtonExample, camerasParent);
            clone.name = cameraButtonExample.name + "_cam" + camIndex;
            clone.SetActive(true);

            int globalIndex = camIndex;
            int col = globalIndex % cameraColumns;
            int row = globalIndex / cameraColumns;
            if (exampleRT != null)
            {
                RectTransform rt = clone.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(basePos.x + col * spacing.x, basePos.y - row * spacing.y);
            }

            ConfigureButtonForCamera(clone, camIndex);
            cameraButtonClones.Add(clone);
        }

        if (camCount > camCapacity)
            Debug.LogWarning($"ControlRoomManager: {camCount} cameras but capacity is {camCapacity}. Only first {camCapacity} have buttons.");
    }

    private void ConfigureButtonForCamera(GameObject buttonObj, int camIndex)
    {
        TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
        if (label != null && cameras != null && camIndex < cameras.Count)
            label.text = cameras[camIndex].cameraID;

        Button btn = buttonObj.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick = new Button.ButtonClickedEvent();
            int idxCopy = camIndex;
            btn.onClick.AddListener(() => ShowCameraAtIndex(idxCopy));
        }
    }

    private void UpdateDoorButtonsIfNeeded()
    {
        if (DoorRegistry.Instance == null) return;
        var doors = DoorRegistry.Instance.GetDoors();
        int doorCount = doors?.Count ?? 0;
        if (doorCount != lastDoorCount)
        {
            lastDoorCount = doorCount;
            UpdateDoorButtonsImmediate();
        }
    }

    private void UpdateDoorButtonsImmediate()
    {
        if (doorButtonExample == null || doorsParent == null)
            return;

        var doors = DoorRegistry.Instance.GetDoors();
        int doorCount = doors?.Count ?? 0;

        foreach (var go in doorButtonClones)
            if (go != null) Destroy(go);
        doorButtonClones.Clear();

        if (doorCount == 0)
        {
            doorButtonExample.SetActive(false);
            return;
        }

        doorButtonExample.SetActive(true);
        doorButtonExample.transform.SetParent(doorsParent, false);

        RectTransform exampleRT = doorButtonExample.GetComponent<RectTransform>();
        Vector2 cellSize = exampleRT != null ? exampleRT.sizeDelta : new Vector2(100, 30);
        Vector2 basePos = exampleRT != null ? exampleRT.anchoredPosition : Vector2.zero;
        Vector2 spacing = new Vector2(cellSize.x, cellSize.y);

        ConfigureButtonForDoor(doorButtonExample, 0);

        int doorCapacity = Mathf.Max(1, doorRows) * Mathf.Max(1, doorColumns);
        int toCreate = Mathf.Min(doorCount, doorCapacity) - 1;

        for (int i = 0; i < toCreate; i++)
        {
            int doorIndex = i + 1;
            GameObject clone = Instantiate(doorButtonExample, doorsParent);
            clone.name = doorButtonExample.name + "_door" + doorIndex;
            clone.SetActive(true);

            int globalIndex = doorIndex;
            int col = globalIndex % doorColumns;
            int row = globalIndex / doorColumns;
            if (exampleRT != null)
            {
                RectTransform rt = clone.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(basePos.x + col * spacing.x, basePos.y - row * spacing.y);
            }

            ConfigureButtonForDoor(clone, doorIndex);
            doorButtonClones.Add(clone);
        }

        if (doorCount > doorCapacity)
            Debug.LogWarning($"ControlRoomManager: {doorCount} doors but capacity is {doorCapacity}. Only first {doorCapacity} have buttons.");
    }

    private void ConfigureButtonForDoor(GameObject buttonObj, int doorIndex)
    {
        TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
        var doors = DoorRegistry.Instance?.GetDoors();
        if (label != null && doors != null && doorIndex < doors.Count)
        {
            string id = doors[doorIndex] != null ? doors[doorIndex].doorID : $"Door {doorIndex + 1}";
            label.text = id;
        }

        Button btn = buttonObj.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick = new Button.ButtonClickedEvent();
            int idxCopy = doorIndex;
            btn.onClick.AddListener(() => ToggleDoor(idxCopy));
        }
    }
}