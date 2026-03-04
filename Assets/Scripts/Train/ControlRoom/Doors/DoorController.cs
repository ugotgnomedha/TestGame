using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public string doorID;
    public float liftAmount = 2f;
    public float speed = 2f;

    private static int maxOpenDoors = 1;
    private static int currentOpenCount = 0;

    public static int MaxOpenDoors
    {
        get => maxOpenDoors;
        set => maxOpenDoors = Mathf.Max(0, value);
    }

    public static int GetCurrentOpenCount() => currentOpenCount;
    public static int GetMaxOpenDoors() => maxOpenDoors;

    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 closedPos;
    private Vector3 openPos;

    private void OnEnable()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * liftAmount;

        DoorRegistry.Instance.RegisterDoor(this);
    }

    private void OnDestroy()
    {
        if (isOpen)
        {
            currentOpenCount = Mathf.Max(0, currentOpenCount - 1);
        }
    }

    public void ToggleDoor()
    {
        if (isMoving) return;
        if (!isOpen && currentOpenCount >= maxOpenDoors)
        {
            Debug.Log($"Cannot open {doorID}: max open doors ({maxOpenDoors}) reached.");
            return;
        }

        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(MoveDoor());
    }

    private IEnumerator MoveDoor()
    {
        isMoving = true;

        if (isOpen)
            currentOpenCount++;
        else
            currentOpenCount--;

        Vector3 target = isOpen ? openPos : closedPos;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}