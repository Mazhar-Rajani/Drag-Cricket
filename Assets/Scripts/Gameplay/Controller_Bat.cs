using System.Collections;
using UnityEngine;

public class Controller_Bat : MonoBehaviour
{
    public static event System.Action OnShotPlayed;
    public static event System.Action<Vector3> OnShotPlayed_Point;

    [SerializeField] private GameObject arrowIndicator = default;
    [SerializeField] private Collider[] batColliders = default;
    [SerializeField] private Controller_Ball ball = default;
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
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        initialBallPosition = ball.transform.position;
        StartCoroutine(ResetBat(0));
        startTime = Time.time;
    }

    private void Update()
    {
        if (!canSwing)
            return;

        SwingBat();
    }

    private IEnumerator ResetBat(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isPressed = false;
        canSwing = false;
        isBatReleased = false;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        dragRotation = Vector3.zero;
        arrowIndicator.SetActive(true);
        for (int i = 0; i < batColliders.Length; i++)
        {
            batColliders[i].enabled = true;
        }
    }

    private void OnMouseDown()
    {
        float distanceFromCamera = Vector3.Distance(mainCamera.transform.position, transform.position);

        initialMousePosition = mainCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));

        isPressed = true;

        StartCoroutine(ball.Throw(2));
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
            float timing = collision.GetContact(0).thisCollider.GetComponent<ColliderStrength>().Timing_Easy;
            Vector3 hitForce = transform.forward * ((ballHitForce * timing));
            hitForce.y = Mathf.Clamp(hitForce.y, -ballMaxHeight, ballMaxHeight) * timing;
            Debug.Log("Hit Force" + hitForce);
            Debug.Log("Timing " + collision.GetContact(0).thisCollider.name + " - " + timing);
            ballRigidbody.AddForce(hitForce, ForceMode.Impulse);

            OnShotPlayed?.Invoke();
            OnShotPlayed_Point?.Invoke(collision.GetContact(0).point);
        }
    }
}