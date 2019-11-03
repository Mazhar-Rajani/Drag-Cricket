using UnityEngine;

public class Stumps : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool isHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    private void OnEnable()
    {
        Ball.OnBallReset += ResetStumps;
    }

    private void OnDisable()
    {
        Ball.OnBallReset -= ResetStumps;
    }

    private void ResetStumps()
    {
        rb.isKinematic = true;
        transform.position = originalPos;
        transform.rotation = originalRot;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHit)
        {
            isHit = true;
            rb.isKinematic = false;
        }
    }
}