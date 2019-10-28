using UnityEngine;

public class Button_Play : MonoBehaviour
{
    public static event System.Action OnPlayMatch;

    public void OnClick_PlayMatch()
    {
        Debug.Log("Play Clicked");
        OnPlayMatch?.Invoke();
    }
}