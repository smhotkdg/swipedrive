using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunGames;
using GameAnalyticsSDK;
using FunGames.Sdk.Analytics;
using FunGames.Sdk.Cohort;
public class FunGameSDKManager : MonoBehaviour
{    
    private static FunGameSDKManager _instance = null;

    public static FunGameSDKManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("cSingleton FunGameSDKManager == null");
            return _instance;
        }
    }
    void Start()
    {
        GameAnalytics.Initialize();
        float cohort = FunGamesCohort.GetCohort();
        UnityEngine.Debug.Log(cohort);
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;              
        }
    }
    public void StartLevel()
    {
        FunGamesAnalytics.NewProgressionEvent("Start", GameManager.Instance.Level.ToString());
    }
    public void LevelComplete()
    {
        FunGamesAnalytics.NewProgressionEvent("Complete", GameManager.Instance.Level.ToString());
    }
    public void LevelFail()
    {
        FunGamesAnalytics.NewProgressionEvent("Fail", GameManager.Instance.Level.ToString());
    }
    public void ShopEvent()
    {
        FunGamesAnalytics.NewDesignEvent("OpenShop", "clicked");
    }
}
