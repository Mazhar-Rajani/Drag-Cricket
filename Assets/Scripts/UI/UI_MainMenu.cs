using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI = default;

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += EnableMainMenuUI;
        Button_Play.OnPlayMatch += DisableMainMenuUI;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= EnableMainMenuUI;
        Button_Play.OnPlayMatch -= DisableMainMenuUI;
    }

    private void EnableMainMenuUI()
    {
        mainMenuUI.SetActive(true);
    }

    private void DisableMainMenuUI()
    {
        mainMenuUI.SetActive(false);
    }
}