using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
public class AdManager : MonoBehaviour
{
    //// Start is called before the first frame update

    //private static AdManager _instance = null;
    //AudienceNetworkClientImpl fbadClient;
    ////AdMobClientImpl admobClient;
    //public static AdManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            Debug.LogError("cSingleton AdManager == null");
    //        return _instance;
    //    }
    //}
    //private void Awake()
    //{
    //    if (_instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        _instance = this;
    //    }

    //    if (!RuntimeManager.IsInitialized())
    //        RuntimeManager.Init();
    //}
    //void Start()
    //{
    //    // AdMob client.
    //    //admobClient = Advertising.AdMobClient;
    //    // Facebook Audience Network client.
    //    fbadClient = Advertising.AudienceNetworkClient;


    //    fbadClient.RewardedAdCompleted += FbanClient_RewardedAdCompleted;
    //    //admobClient.RewardedAdCompleted += AdmobClient_RewardedAdCompleted;

    //    fbadClient.ShowBannerAd(BannerAdPosition.Bottom,BannerAdSize.SmartBanner);
        
    //}
    
    //private void AdmobClient_RewardedAdCompleted(IAdClient arg1, AdPlacement arg2)
    //{
    //    //admob rewardcomplete   
    //}

    //private void FbanClient_RewardedAdCompleted(IAdClient arg1, AdPlacement arg2)
    //{
    //    //FireBaseRewardComplete 
    //}


    //// Update is called once per frame
    //void Update()
    //{
    //    if (fbadClient.IsInterstitialAdReady())
    //    {
    //    }
    //    else
    //    {
    //        fbadClient.LoadInterstitialAd();
    //    }
    //    //if (admobClient.IsRewardedAdReady())
    //    //{
    //    //}

    //}


    //public void ShowINterstitialAds()
    //{
        
    //    if (fbadClient.IsInterstitialAdReady())
    //    {
    //        fbadClient.ShowInterstitialAd();
    //        Debug.Log("페북 광고 나옴");
    //    }
    //    else
    //    {
    //        Debug.Log("페북 광고 없음");         
    //    }
    //}

}
