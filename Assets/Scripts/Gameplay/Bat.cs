using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public static event System.Action<Vector3> OnShotPlayed;

    [SerializeField] private Vector3 centerOfMass = default;
    [SerializeField] private Collider dragCollider = default;
    [SerializeField] private Collider[] batColliders = default;
    [SerializeField] private GameObject arrowIndicator = default;
    [SerializeField] private Ball ball = default;
    [SerializeField] private Collider ballCollider = default;
    [SerializeField] private Rigidbody ballRigidbody = default;
    [SerializeField] private Camera mainCamera = default;
    [SerializeField] private Vector2 maxRotation = default;
    [SerializeField] private Vector2 dragSmoothness = default;
    [SerializeField] private float maxBallHeight = default;
    [SerializeField] private float maxBallHitForce = default;
    [SerializeField] private float swingSpeed = default;

    private Rigidbody rb;
    private Quaternion originalRotation;
    private Vector3 initialBallPosition;
    private Vector3 dragRotation;
    private Vector3 originalPosition;
    private Vector3 initialMousePosition;
    private Vector3 currentMousePosition;
    private Vector3 invertedRotation;
    private Vector2 offsetMousePos;
    private Vector2 dragDistance;
    private Vector3 ballVelocityBeforeHit;
    private Vector3 batVelocityBeforeHit;
    private bool isPressed;
    private bool canSwing;
    private bool isBatReleased;
    private float startTime;
    private float xVelocity;
    private float yVelocity;
    private float timing;
    private float ballHeight;
    private float ballHitForce;
    private Vector3 direction;
    private Vector3 force;

    public float DragDistance { get => dragDistance.magnitude; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        initialBallPosition = ball.transform.position;
        StartCoroutine(ResetBat(0));
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        ballVelocityBeforeHit = ballRigidbody.velocity;
        batVelocityBeforeHit = rb.velocity;

        if (canSwing)
            SwingBat();
    }

    private IEnumerator ResetBat(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rb.isKinematic = true;
        isPressed = false;
        canSwing = false;
        isBatReleased = false;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        dragRotation = Vector3.zero;
        arrowIndicator.SetActive(true);
        dragCollider.enabled = true;
        for (int i = 0; i < batColliders.Length; i++)
        {
            batColliders[i].enabled = true;
        }
        rb.isKinematic = false;
    }

    private void OnMouseDown()
    {
        float distanceFromCamera = Vector3.Distance(mainCamera.transform.position, transform.position);

        initialMousePosition = mainCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));

        isPressed = true;

        ball.ThrowBall();
    }

    private void OnMouseDrag()
    {
        if (!isPressed)
            return;

        arrowIndicator.SetActive(false);

        float distanceFromCamera = Vector3.Distance(mainCamera.transform.position, transform.position);

        currentMousePosition = mainCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));

        offsetMousePos = currentMousePosition - initialMousePosition;
        dragDistance = Vector2.ClampMagnitude(offsetMousePos, 1);

        float xRot = Mathf.SmoothDampAngle(transform.eulerAngles.x, -(dragDistance.y * maxRotation.x), ref xVelocity, dragSmoothness.x);
        float yRot = Mathf.SmoothDampAngle(transform.eulerAngles.y, -(dragDistance.x * maxRotation.y), ref yVelocity, dragSmoothness.y);
        xRot = Mathf.Clamp(xRot, 0, 90);
        dragRotation = new Vector3(xRot, yRot, 0);
        transform.eulerAngles = dragRotation;

        float xInvRot = -transform.eulerAngles.x;
        float yInvRot = transform.eulerAngles.y;
        float zInvRot = transform.eulerAngles.z;
        invertedRotation = new Vector3(xInvRot, yInvRot, zInvRot);
    }

    private void OnMouseUp()
    {
        if (!isPressed)
            return;

        canSwing = true;
        isBatReleased = true;
        dragCollider.enabled = false;
        StartCoroutine(ResetBat(2));
    }

    private void SwingBat()
    {
        rb.MoveRotation(Quaternion.Lerp(
            transform.rotation, Quaternion.Euler(invertedRotation), Time.deltaTime * swingSpeed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != ballCollider)
            return;

        if (isBatReleased)
        {
            isBatReleased = false;
            for (int i = 0; i < batColliders.Length; i++)
            {
                batColliders[i].enabled = false;
            }

            ballRigidbody.velocity = Vector3.zero;
            timing = collision.GetContact(0).thisCollider.GetComponent<ColliderStrength>().Timing;

            OnShotPlayed?.Invoke(collision.GetContact(0).point);

            ballHeight = (-maxBallHeight * Mathf.Deg2Rad) * (dragRotation.normalized.magnitude * timing);
            ballHitForce = maxBallHitForce * (dragRotation.normalized.magnitude * timing);
            ballHitForce = Mathf.Clamp(ballHitForce, 0, maxBallHitForce);
            direction = new Vector3(dragDistance.x, ballHeight, dragDistance.y);
            force = ballHitForce * direction;
            ball.canThrowBall = false;
            ballRigidbody.useGravity = true;
            ballRigidbody.isKinematic = false;
            ballRigidbody.AddForce(-force, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.rotation * centerOfMass, 0.1f);
    }
}