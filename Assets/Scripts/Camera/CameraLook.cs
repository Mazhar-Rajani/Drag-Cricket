using System.Collections;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private Transform ball = default;
    [SerializeField] private float rotateSpeed = default;

    private bool canFollowBall;
    private bool canResetCamera;
    private Quaternion originalRotation;
    float progress;
    float angle;

    private void Awake()
    {
        originalRotation = transform.rotation;
    }

    private void OnEnable()
    {
        Bat.OnShotPlayed += LookAtBall;
        Ball.OnBallReset += ResetCamera;
    }

    private void OnDisable()
    {
        Bat.OnShotPlayed -= LookAtBall;
        Ball.OnBallReset -= ResetCamera;
    }

    private void LateUpdate()
    {
        if (canFollowBall)
        {
            Vector3 distance = ball.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(distance, Vector3.up);
            progress = rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, progress);
        }
        if (canResetCamera)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, (rotateSpeed * 2) * Time.deltaTime);
        }
    }

    private void ResetCamera()
    {
        transform.rotation = originalRotation;
    }

    private void LookAtBall()
    {
        StartCoroutine(LookAtBallCoroutine());
    }

    private IEnumerator LookAtBallCoroutine()
    {
        angle = Vector3.Dot(transform.forward, ball.forward) * Mathf.Rad2Deg;

        progress = 0;
        canFollowBall = true;
        yield return new WaitForSeconds(1f);
        canFollowBall = false;
        yield return new WaitForSeconds(1f);
        canResetCamera = true;
        yield return new WaitForSeconds(1);
        canResetCamera = false;
    }
}