using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI = default;

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += EnableMainMenuUI;
        Button_Play.OnPlayMatch += DisableMainMenuUI;
        Button_RateUs.OnRateGame += Button_RateUs_OnRateGame;
        Button_Share.OnShareGame += Button_Share_OnShareGame;
        Button_NoAds.OnRemoveAds += Button_NoAds_OnRemoveAds;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= EnableMainMenuUI;
        Button_Play.OnPlayMatch -= DisableMainMenuUI;
        Button_RateUs.OnRateGame -= Button_RateUs_OnRateGame;
        Button_Share.OnShareGame -= Button_Share_OnShareGame;
        Button_NoAds.OnRemoveAds -= Button_NoAds_OnRemoveAds;
    }

    private void EnableMainMenuUI()
    {
        mainMenuUI.SetActive(true);
    }

    private void DisableMainMenuUI()
    {
        mainMenuUI.SetActive(false);
    }

    private void Button_RateUs_OnRateGame()
    {
        Application.OpenURL("market://details?id=com.halfcarrotstudio.tomatocatcher");
    }

    private void Button_Share_OnShareGame()
    {
    }

    private void Button_NoAds_OnRemoveAds()
    {
    }
}