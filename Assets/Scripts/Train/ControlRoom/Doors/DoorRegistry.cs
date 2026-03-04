using System.Collections.Generic;
using UnityEngine;

public class DoorRegistry : MonoBehaviour
{
    public static DoorRegistry Instance;

    private List<DoorController> activeDoors = new List<DoorController>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterDoor(DoorController door)
    {
        if (!activeDoors.Contains(door))
            activeDoors.Add(door);
    }

    public List<DoorController> GetDoors()
    {
        return activeDoors;
    }
}