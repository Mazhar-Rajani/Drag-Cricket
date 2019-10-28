using UnityEngine;

public class Button_RateUs : MonoBehaviour
{
    public static event System.Action OnRateGame;

    public void OnClick_RateUs()
    {
        Debug.Log("Rate Us Clicked");
        OnRateGame?.Invoke();
    }
}