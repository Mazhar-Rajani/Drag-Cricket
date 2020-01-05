using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = default;
    [SerializeField] private float shakeAmount = default;

    private bool canShake;
    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    void OnEnable()
    {
        Bat.OnShotPlayed += ShakeCamera;
    }

    private void OnDisable()
    {
        Bat.OnShotPlayed -= ShakeCamera;
    }

    void Update()
    {
        if (!canShake)
            return;

        transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
    }

    private void ShakeCamera(Vector3 hitPoint)
    {
        StartCoroutine(ShakeCameraCoroutine());
    }

    private IEnumerator ShakeCameraCoroutine()
    {
        canShake = true;
        yield return new WaitForSeconds(shakeDuration);
        canShake = false;
        transform.localPosition = originalPos;
    }
}