using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    [SerializeField] private float ballThrowPos = default;
    [SerializeField] private float ballSwingRange = default;

    [SerializeField] private Vector3 point0 = default;
    public Vector3 Point0 => point0;

    [SerializeField] private Vector3 point1 = default;
    public Vector3 Point1 => point1;

    [SerializeField] private Vector3 point2 = default;
    public Vector3 Point2 => point2;

    private void OnEnable()
    {
        Ball.OnThrowReset += OnThrowReset;
    }

    private void OnDisable()
    {
        Ball.OnThrowReset -= OnThrowReset;
    }

    private void OnThrowReset()
    {
        point0.x = Random.value > 0.5f ? ballThrowPos : -ballThrowPos;
        point1.x = point0.x;
        point2.x = point0.x > 0 ? Random.Range(0, ballSwingRange) : Random.Range(0, -ballSwingRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point0, 0.05f);
        Gizmos.DrawWireSphere(point1, 0.05f);
        Gizmos.DrawWireSphere(point2, 0.05f);
        Gizmos.DrawLine(point0, point1);
        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point0, new Vector3(point0.x + ballThrowPos, point0.y, point0.z));
        Gizmos.DrawLine(point0, new Vector3(point0.x - ballThrowPos, point0.y, point0.z));
        Gizmos.DrawLine(point2, new Vector3(point2.x + ballSwingRange, point2.y, point2.z));
        Gizmos.DrawLine(point2, new Vector3(point2.x - ballSwingRange, point2.y, point2.z));
    }
}