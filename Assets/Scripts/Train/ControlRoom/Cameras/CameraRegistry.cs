using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegistry : MonoBehaviour
{
    public static CameraRegistry Instance;
    public static bool HasInstance => Instance != null;

    private List<StationCamera> activeCameras = new List<StationCamera>();

    public event Action<StationCamera> OnCameraRegistered;
    public event Action<StationCamera> OnCameraUnregistered;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterCamera(StationCamera cam)
    {
        if (!activeCameras.Contains(cam))
        {
            activeCameras.Add(cam);
            OnCameraRegistered?.Invoke(cam);
        }
    }

    public void UnregisterCamera(StationCamera cam)
    {
        if (activeCameras.Contains(cam))
        {
            activeCameras.Remove(cam);
            OnCameraUnregistered?.Invoke(cam);
        }
    }

    public List<StationCamera> GetActiveCameras() => activeCameras;
}