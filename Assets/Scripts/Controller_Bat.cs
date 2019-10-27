using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Bat : MonoBehaviour
{
    public static event System.Action OnShotPlayed;
    public static event System.Action<Vector3> OnShotPlayed_Point;

    [SerializeField] private GameObject arrowIndicator = default;
    [SerializeField] private Collider batCollider = default;
    [SerializeField] private Controller_Ball ball = default;
    [SerializeField] private Collider ballCollider = default;
    [SerializeField] private Rigidbody ballRigidbody = default;
    [SerializeField] private Camera mainCamera = default;
    [SerializeField] private Vector2 maxRotation = default;
    [SerializeField] private float swingSpeed = default;
    [SerializeField] private float ballHitForce = default;
    [SerializeField] private Vector2 dragSmoothness = default;

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
        batCollider.enabled = true;
        arrowIndicator.SetActive(true);
    }

    private void OnMouseDown()
    {
        float distanceFromCamera = Vector3.Distance(mainCamera.transform.position, transform.position);

        initialMousePosition = mainCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));

        isPressed = true;
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
            batCollider.enabled = false;
            ballRigidbody.velocity = transform.forward * (ballHitForce * dragDistance.magnitude);
            OnShotPlayed?.Invoke();
            OnShotPlayed_Point?.Invoke(collision.GetContact(0).point);
        }
    }
}