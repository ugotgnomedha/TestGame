using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StationDatabase", menuName = "Train/Station Database")]
public class StationDatabase : ScriptableObject
{
    [Tooltip("Scene names of stations to load.")]
    public List<string> stationSceneNames = new List<string>();
}