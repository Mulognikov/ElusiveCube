using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAd : MonoBehaviour, IUnityAdsListener
{
    private string placement = "rewardedVideo";

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize("2967172", true);
    }

    public void ShowRewardedVideo()
    {
        Advertisement.Show(placement);
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == placement)
        {
            EventManager.Instance.PostNotification(EventTypes.AdReady, this);
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            EventManager.Instance.PostNotification(EventTypes.GameContinue, this);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
