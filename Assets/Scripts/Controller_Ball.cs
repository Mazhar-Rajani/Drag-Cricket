using System.Collections;
using UnityEngine;

public class Controller_Ball : MonoBehaviour
{
    public static event System.Action OnBallReset;

    [SerializeField] private Collider pitchCollider = default;
    [SerializeField] private float minThrowAngle = default;
    [SerializeField] private float maxThrowAngle = default;
    [SerializeField] private float minThrowSpeed = default;
    [SerializeField] private float maxThrowSpeed = default;
    
    private Rigidbody rb;
    private Collider ballCollider;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private int[] xPos = { 1, -1 };

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballCollider = GetComponent<Collider>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        StartCoroutine(Reset(2));
    }

    public IEnumerator Throw(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ThrowBall();
    }

    public IEnumerator Reset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ResetBall();
    }

    private void ResetBall()
    {
        pitchCollider.enabled = true;
        rb.isKinematic = true;
        int r = Random.Range(0, xPos.Length);
        transform.position = new Vector3(xPos[r], originalPosition.y, originalPosition.z);
        transform.rotation = originalRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        OnBallReset?.Invoke();
    }

    private void ThrowBall()
    {
        rb.isKinematic = false;
        float angleInRadians = Random.Range(minThrowAngle, maxThrowAngle) * Mathf.Deg2Rad;
        Vector3 throwDirection = new Vector3(0, Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));
        Vector3 throwForce = -throwDirection * Random.Range(minThrowSpeed, maxThrowSpeed);
        rb.AddForce(throwForce, ForceMode.Impulse);
        StartCoroutine(Reset(5));
    }
}