using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorDatabase", menuName = "Elevator/Floor Database")]
public class FloorDatabase : ScriptableObject
{
    [Tooltip("Scene names of floors to load.")]
    public List<string> floorSceneNames = new List<string>();
}
