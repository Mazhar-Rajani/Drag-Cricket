using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event System.Action OnBallThrow;
    public static event System.Action OnThrowReset;
    public static event System.Action OnBallReset;

    [SerializeField] private BallTrajectory ballTrajectory = default;
    [SerializeField] private float ballThrowSpeed = default;
    [SerializeField] private float ballBounceSpeed = default;

    private Rigidbody ballRigidbody;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool canThrowBall;
    private float moveProgress;
    private float bounceProgress;
    private Vector3 ballPos;

    private void Awake()
    {
        StoreDefaultBallProperties();
        ResetThrow();
        ResetBall();
    }

    private void FixedUpdate()
    {
        MoveBall();
    }

    private void StoreDefaultBallProperties()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        originalPosition = ballTrajectory.Point0;
        originalRotation = transform.rotation;
    }

    public void ThrowBall()
    {
        canThrowBall = true;
        OnBallThrow?.Invoke();
    }

    public void ResetThrow()
    {
        canThrowBall = false;
        moveProgress = 0;
        bounceProgress = 0;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        OnThrowReset?.Invoke();
    }

    public void ResetBall()
    {
        transform.position = ballTrajectory.Point0;
        transform.rotation = originalRotation;
        OnBallReset?.Invoke();
    }

    private void MoveBall()
    {
        if (!canThrowBall)
            return;

        if (moveProgress < 1)
        {
            moveProgress += ballThrowSpeed * Time.deltaTime;
            moveProgress = Mathf.Clamp01(moveProgress);
            ballPos = Vector3.Lerp(ballTrajectory.Point0, ballTrajectory.Point1, moveProgress);
        }
        else
        {
            bounceProgress += ballBounceSpeed * Time.deltaTime;
            bounceProgress = Mathf.Clamp01(bounceProgress);
            ballPos = Vector3.Lerp(ballTrajectory.Point1, ballTrajectory.Point2, bounceProgress);
        }

        ballRigidbody.MovePosition(ballPos);
    }
}