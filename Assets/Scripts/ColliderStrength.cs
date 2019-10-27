using UnityEngine;

public class ColliderStrength : MonoBehaviour
{
    [SerializeField] private float hitForce = default;
    public float HitForce => hitForce;
}