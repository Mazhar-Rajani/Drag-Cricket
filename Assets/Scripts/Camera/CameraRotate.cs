using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private Camera mainMenuCamera = default;
    [SerializeField] private float rotateSpeed = default;

    private bool canRotateCamera;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalPosition = mainMenuCamera.transform.position;
        originalRotation = mainMenuCamera.transform.rotation;
    }

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += EnableCamera;
        Button_Play.OnPlayMatch += DisableCamera;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= EnableCamera;
        Button_Play.OnPlayMatch -= DisableCamera;
    }

    private void Update()
    {
        if (canRotateCamera)
        {
            mainMenuCamera.transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }

    private void EnableCamera()
    {
        ResetCamera();
        canRotateCamera = true;
        mainMenuCamera.enabled = true;
    }

    private void DisableCamera()
    {
        mainMenuCamera.enabled = false;
        canRotateCamera = false;
        ResetCamera();
    }

    private void ResetCamera()
    {
        mainMenuCamera.transform.position = originalPosition;
        mainMenuCamera.transform.rotation = originalRotation;
    }
}