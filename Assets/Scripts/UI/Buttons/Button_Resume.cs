using UnityEngine;

public class Button_Resume : MonoBehaviour
{
    public static event System.Action OnResumeGame;

    public void OnClick_Resume()
    {
        Debug.Log("Resume Clicked");
        OnResumeGame?.Invoke();
    }
}