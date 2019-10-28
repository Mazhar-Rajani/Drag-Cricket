using UnityEngine;

public class Button_Pause : MonoBehaviour
{
    public static event System.Action OnPauseGame;

    public void OnClick_Pause()
    {
        Debug.Log("Pause Clicked");
        OnPauseGame?.Invoke();
    }
}