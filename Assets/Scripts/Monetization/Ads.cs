using System.Collections;
using UnityEngine;
using UnityEngine.Monetization;

public class Ads : MonoBehaviour
{
    [SerializeField] private string placementId = default;
    [SerializeField] private string gameId = default;
    [SerializeField] private bool testMode = default;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        Monetization.Initialize(gameId, testMode);
    }

    public void ShowAd()
    {
        StartCoroutine(ShowAdWhenReady());
    }

    private IEnumerator ShowAdWhenReady()
    {
        while (!Monetization.IsReady(placementId))
        {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show();
        }
    }
}