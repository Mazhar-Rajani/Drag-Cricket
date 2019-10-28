using UnityEngine;

public class UI_ModalWindow : MonoBehaviour
{
    [SerializeField] private GameObject modalWindowUI = default;

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += DisableModalWindowUI;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= DisableModalWindowUI;
    }

    private void DisableModalWindowUI()
    {
        modalWindowUI.SetActive(false);
    }
}