using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public static event System.Action OnShotPlayed;
    public static event System.Action<Vector3> OnShotPlayed_Point;
    public static event System.Action OnBoundaryScored;
    public static event System.Action<int> OnRunsScored;

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
    [SerializeField] private float ballMaxHeight = default;
    [SerializeField] private float swingSpeed = default;
    [SerializeField] private float ballHitForce = default;

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

        //StartCoroutine(ball.Throw(2));
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
        StartCoroutine(ResetBat(5));
    }

    private void SwingBat()
    {
        rb.MoveRotation(Quaternion.Lerp(
            transform.rotation, Quaternion.Euler(invertedRotation), Time.deltaTime * swingSpeed));

        //rb.isKinematic = false;
        //rb.AddTorque(-transform.right * swingSpeed, ForceMode.VelocityChange);
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
            float timing = collision.GetContact(0).thisCollider.GetComponent<ColliderStrength>().Timing_Easy;

            Debug.Log("M1.V1 Before - " + ballRigidbody.mass * ballVelocityBeforeHit);
            Debug.Log("M2.V2 Before - " + rb.mass * batVelocityBeforeHit);
            Debug.Log("M2.V2 After - " + rb.mass * rb.velocity);
            Vector3 c = ((ballRigidbody.mass * ballVelocityBeforeHit) + (rb.mass * batVelocityBeforeHit) - (rb.mass * rb.velocity));
            Debug.Log("C - " + c);
            Vector3 hitForce = c / ballRigidbody.mass;
            hitForce = new Vector3(hitForce.x, -hitForce.normalized.y * ((ballHitForce * timing)), -hitForce.normalized.z * ((ballHitForce * timing)));
            //hitForce.y = Mathf.Clamp(hitForce.y, -ballMaxHeight, ballMaxHeight) * timing;
            Debug.Log(transform.forward);
            Debug.Log("Hit Force" + hitForce);
            Debug.Log("Timing " + collision.GetContact(0).thisCollider.name + " - " + timing);
            ballRigidbody.AddForce(-hitForce, ForceMode.Impulse);

            OnShotPlayed?.Invoke();
            OnShotPlayed_Point?.Invoke(collision.GetContact(0).point);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.rotation * centerOfMass, 0.1f);
    }
}