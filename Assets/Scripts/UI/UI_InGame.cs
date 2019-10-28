using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private GameObject inGameUI = default;

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += DisableInGameUI;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= DisableInGameUI;
    }

    private void DisableInGameUI()
    {
        inGameUI.SetActive(false);
    }
}