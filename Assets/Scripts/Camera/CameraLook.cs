using System.Collections;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private Transform ball = default;
    [SerializeField] private float rotateSpeed = default;
    [SerializeField] private Camera gameCamera = default;
    [SerializeField] private float minFocalLength = default;
    [SerializeField] private float maxFocalLength = default;
    [SerializeField] private float zoomSpeed = default;

    private bool canFollowBall;
    private bool canResetCamera;
    private Quaternion originalRotation;
    float progress;
    float zoomProgress;

    private void Awake()
    {
        originalRotation = transform.rotation;
    }

    private void OnEnable()
    {
        Bat.OnShotPlayed += FollowBall;
        Ball.OnBallReset += ResetCamera;
    }

    private void OnDisable()
    {
        Bat.OnShotPlayed -= FollowBall;
        Ball.OnBallReset -= ResetCamera;
    }

    private void LateUpdate()
    {
        if (canFollowBall)
        {
            progress += rotateSpeed * Time.deltaTime;
            progress = Mathf.Clamp01(progress);
            Vector3 distance = ball.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(distance, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, lookRotation.eulerAngles.y, transform.eulerAngles.z), progress);

            zoomProgress += zoomSpeed * Time.deltaTime;
            zoomProgress = Mathf.Clamp01(zoomProgress);
            gameCamera.focalLength = Mathf.Lerp(minFocalLength, maxFocalLength, zoomProgress);
        }
        if (canResetCamera)
        {
            progress += rotateSpeed * Time.deltaTime;
            progress = Mathf.Clamp01(progress);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, progress);

            zoomProgress += zoomSpeed * Time.deltaTime;
            zoomProgress = Mathf.Clamp01(zoomProgress);

            if (gameCamera.focalLength != minFocalLength)
                gameCamera.focalLength = Mathf.Lerp(maxFocalLength, minFocalLength, zoomProgress);
        }
    }

    private void ResetCamera()
    {
        progress = 0;
        zoomProgress = 0;
        canFollowBall = false;
        canResetCamera = true;
    }

    private void FollowBall(Vector3 hitPoint)
    {
        progress = 0;
        zoomProgress = 0;
        canResetCamera = false;
        canFollowBall = true;
    }
}