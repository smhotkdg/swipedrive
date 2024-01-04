using System;
using UnityEngine;

namespace FunGames.Sdk
{
    [CreateAssetMenu(fileName = "Assets/Resources/FunGamesSettings", menuName = "FunGamesSdk/Settings", order = 1000)]
    public class FunGamesSettings : ScriptableObject
    {

        [Header("Sdk Version")]
        [Tooltip("Sdk Version")]
        public string version = "1.2";

        [Header("Cohort")]
        [Tooltip("Run Cohort")]
        public bool runCohort = false;

        [Tooltip("Cohort Test Name")]
        public string cohortTestName = "cohortTest0";

        [Tooltip("Cohort Test Name")]
        public float cohortPercentage = 0.2f;

        [Header("Replay")]
        [Tooltip("Run Replay")]
        public bool runReplay = true;


        [Header("GameAnalytics")]
        [Tooltip("GameAnalytics Ios Game Key")]
        public string GameAnalyticsIosGameKey;

        [Tooltip("GameAnalytics Ios Secret Key")]
        public string GameAnalyticsIosSecretKey;

        [Tooltip("GameAnalytics Android Game Key")]
        public string GameAnalyticsAndroidGameKey;

        [Tooltip("GameAnalytics Android Secret Key")]
        public string GameAnalyticsAndroidSecretKey;

        [Header("MaxAds")]
        [Tooltip("Max Sdk Key")]
        public string maxSdkKey;

        [Header("iOS")]
        public string iOSInterstitialAdUnitId;

        public string iOSRewardedAdUnitId;

        public string iOSBannerAdUnitId;

        [Header("Android")]
        public string androidInterstitialAdUnitId;
        public string androidRewardedAdUnitId;

        public string androidBannerAdUnitId; 
    }
}