using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI = default;

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += DisablePauseMenuUI;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= DisablePauseMenuUI;
    }

    private void DisablePauseMenuUI()
    {
        pauseMenuUI.SetActive(false);
    }
}