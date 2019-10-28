using UnityEngine;

public class Button_Share : MonoBehaviour
{
    public static event System.Action OnShareGame;

    public void OnClick_Share()
    {
        Debug.Log("Share Clicked");
        OnShareGame?.Invoke();
    }
}