using UnityEngine;

[System.Serializable]
public class CrowdPlacementData
{
    public string id = default;
    [Range(-360, 360)] public int minAngle = default;
    [Range(-360, 360)] public int maxAngle = default;
    [Range(0, 1)] public float height = default;
    [Range(0, 200)] public int radius = default;
}