using UnityEngine;

public class ColliderStrength : MonoBehaviour
{
    [SerializeField] private float timing_Easy = default;
    public float Timing_Easy => timing_Easy;

    [SerializeField] private float timing_Difficult = default;
    public float Timing_Difficult => timing_Difficult;
}