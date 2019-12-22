using UnityEngine;

public class BatRotate : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = default;
    [SerializeField] private Vector2 maxRotation = default;
    [SerializeField] private Vector2 dragSmoothness = default;
    [SerializeField] private float swingSpeed = default;

    private Rigidbody batRigidbody;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 initialMousePosition;
    private Vector3 currentMousePosition;
    private Vector2 offsetMousePos;
    private Vector3 invertedRotation;
    private Vector2 dragDistance;
    private Vector3 dragRotation;
    private bool isPressed;
    private bool canSwing;
    private float xVelocity;
    private float yVelocity;

    private void Awake()
    {
        batRigidbody = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (canSwing)
            SwingBat();
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
    }

    private void SwingBat()
    {
        batRigidbody.MoveRotation(Quaternion.Lerp(
            transform.rotation, Quaternion.Euler(invertedRotation), Time.deltaTime * swingSpeed));
    }
}