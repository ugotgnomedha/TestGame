using UnityEngine;

public class StationCamera : MonoBehaviour
{
    [Header("Unique ID for this camera")]
    public string cameraID;

    [Header("Assign the Camera component here")]
    public Camera cameraComponent;

    private void Awake()
    {
        if (cameraComponent == null)
            cameraComponent = GetComponent<Camera>();

        cameraComponent.enabled = false;
    }

    private void OnEnable()
    {
        if (CameraRegistry.HasInstance)
            CameraRegistry.Instance.RegisterCamera(this);
    }

    private void OnDisable()
    {
        if (CameraRegistry.HasInstance)
            CameraRegistry.Instance.UnregisterCamera(this);
    }
}