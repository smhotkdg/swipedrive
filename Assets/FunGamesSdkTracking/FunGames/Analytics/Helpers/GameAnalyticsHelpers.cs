using System;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

/*Game Analytics Tracking Events
1. Track Progression Event for every level:
    a/call on start: 
ProgressionEvent("Start",level_number,sublevel_number)
If there is no sublevel, please call:
ProgressionEvent("Start",level_number,"")

    b/end level fail
ProgressionEvent("Fail",level_number,sublevel_number)

    c/end level complete
ProgressionEvent("Complete",level_number,sublevel_number,score)
If score is available, please add it

2. Track Design Event:
    a/ track UI clicks
For every clicks on UI, please call:
NewDesignEvent("button_id",-1)

    b/ Other useful tracking events
shopButton
restartButton
itemPurchased
coinsCollected


How to use this script, simple call:

using FunGames.Sdk.Analytics.Helpers;
GameAnalyticsHelpers.ProgressionEvent(statusString,level, sublevel,score)
GameAnalyticsHelpers.NewDesignEvent(args,-1)
*/


namespace FunGames.Sdk.Analytics.Helpers
{
    public class GameAnalyticsHelpers
    {

        internal static void Initialize()
        {
            FunGamesSettings settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
            bool flag = !settings.GameAnalyticsAndroidGameKey.Equals(string.Empty) && !settings.GameAnalyticsAndroidSecretKey.Equals(string.Empty);
            if (!flag){
                flag = !settings.GameAnalyticsIosGameKey.Equals(string.Empty) && !settings.GameAnalyticsIosSecretKey.Equals(string.Empty);

            }
            GameAnalytics gameAnalytics = UnityEngine.Object.FindObjectOfType<GameAnalytics>();
            if (gameAnalytics == null)
            {
                GameAnalyticsHelpers.AddOrUpdatePlatform(RuntimePlatform.IPhonePlayer,settings.GameAnalyticsIosGameKey, settings.GameAnalyticsIosSecretKey);
                if (flag)
                {
                    GameAnalyticsHelpers.AddOrUpdatePlatform(RuntimePlatform.Android,settings.GameAnalyticsAndroidGameKey, settings.GameAnalyticsAndroidSecretKey);
                }
                else
                {
                    GameAnalyticsHelpers.RemovePlatform(RuntimePlatform.Android);
                }
                GameAnalytics.SettingsGA.InfoLogBuild = false;
                GameAnalytics.SettingsGA.InfoLogEditor = false;
                GameAnalytics.Initialize();
                return;
            }
            throw new Exception("Looks like GameAnalytics has been added manually to the scene.");
        }

        internal static void AddOrUpdatePlatform(RuntimePlatform platform, string gameKey, string secretKey)
        {
            if (!GameAnalytics.SettingsGA.Platforms.Contains(platform))
            {
                GameAnalytics.SettingsGA.AddPlatform(platform);
            }
            int index = GameAnalytics.SettingsGA.Platforms.IndexOf(platform);
            GameAnalytics.SettingsGA.UpdateGameKey(index, gameKey);
            GameAnalytics.SettingsGA.UpdateSecretKey(index, secretKey);
            GameAnalytics.SettingsGA.Build[index] = Application.version;
        }

        internal static void RemovePlatform(RuntimePlatform platform)
        {
            if (GameAnalytics.SettingsGA.Platforms.Contains(platform))
            {
                int index = GameAnalytics.SettingsGA.Platforms.IndexOf(platform);
                GameAnalytics.SettingsGA.RemovePlatformAtIndex(index);
            }
        }

        internal static void ProgressionEvent(string statusString, string level, string sublevel="", int score=-1){

            GAProgressionStatus status = GAProgressionStatus.Start;

            if (statusString.ToLower() == "complete")
            {
                status = GAProgressionStatus.Complete;
            }

            else if (statusString.ToLower() == "fail")
            {
                status = GAProgressionStatus.Fail;
            }
   
            if (score==-1)
            {
                GameAnalytics.NewProgressionEvent(status, level, sublevel);
            }
            else
            {
                GameAnalytics.NewProgressionEvent(status, level, sublevel,score);
            }
        }
        internal static void NewDesignEvent(string eventId,string eventValue="")
        {
            if (eventValue != "")
            {
                try
                {
                    float score = float.Parse(eventValue);
                    GameAnalytics.NewDesignEvent(eventId,score);
                }
                catch
                {
                    GameAnalytics.NewDesignEvent(eventId + ":" + eventValue);
                }
            }
            else
            {
                GameAnalytics.NewDesignEvent(eventId);
            }
        }
    }
}
