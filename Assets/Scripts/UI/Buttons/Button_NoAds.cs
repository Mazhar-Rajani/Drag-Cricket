using UnityEngine;

public class Button_NoAds : MonoBehaviour
{
    public static event System.Action OnRemoveAds;

    public void OnClick_RemoveAds()
    {
        Debug.Log("Remove Ads Clicked");
        OnRemoveAds?.Invoke();
    }
}