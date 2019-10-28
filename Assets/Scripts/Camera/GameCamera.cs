using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Camera gameCamera = default;

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += DisableCamera;
        Button_Play.OnPlayMatch += EnableCamera;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= DisableCamera;
        Button_Play.OnPlayMatch -= EnableCamera;
    }

    private void EnableCamera()
    {
        gameCamera.enabled = true;
    }

    private void DisableCamera()
    {
        gameCamera.enabled = false;
    }
}