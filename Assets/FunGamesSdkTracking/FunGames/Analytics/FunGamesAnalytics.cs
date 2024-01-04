using System.Collections.Generic;
using UnityEngine;
using System;
using FunGames.Sdk.Analytics.Helpers;
using System.Collections;
using UnityEditor;


namespace FunGames.Sdk.Analytics
{
	internal class FunGamesAnalytics: MonoBehaviour
    {
        static Boolean hasResumed;
        static Boolean newSession = false;

        private static FunGamesAnalytics funGamesAnalytics;



        void Awake()
        {
            DontDestroyOnLoad(this);
            if (funGamesAnalytics == null) {
                funGamesAnalytics = this;
                GameAnalyticsHelpers.Initialize();
                FunGamesApiAnalytics.Initialize();
            } else {
                DestroyObject(gameObject);
            }
        }

        public void Start()
        {
        }

        IEnumerator StartCountdown(float countdownValue = 90)
        {
            yield return new WaitForSeconds(countdownValue);

            Debug.Log("Finished Coroutine at timestamp : " + Time.time);

            if (hasResumed == false)
            {
                string datetimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");   
                FunGamesApiAnalytics.NewEvent("ga_session_end",datetimeString);
                newSession = true;
            }
        }

        void OnApplicationFocus(bool hasFocus)
        {
             if (hasFocus == false)
            {
                hasResumed = false;
            }
            else
            {
                hasResumed = true;
                if (newSession == true)
                {
                    newSession = false;
                }
            }
        }

        void OnApplicationPause(bool pause)
        {
            if (pause==true)
            {
                string datetimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");   
                FunGamesApiAnalytics.NewEvent("ga_session_end",datetimeString);
            }

            /*else 
            {
                string datetimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");   
                FunGamesApiAnalytics.NewEvent("ga:session_start",datetimeString);
            }*/
        }

        void OnApplicationQuit()
        {
            string datetimeString = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");   
            FunGamesApiAnalytics.NewEvent("ga_session_end",datetimeString);
        }  

        internal static void NewProgressionEvent(string typeEvent, string level, string sublevel="", int score=-1)
        {
            switch (typeEvent)
            {
                case "Start":
                    OnLevelStart(level,sublevel);
                    break;

                case "Complete":
                    OnLevelComplete(level,sublevel,score);
                    break;

                case "Fail":
                    OnLevelFail(level,sublevel,score);
                    break;
                default:
                    Debug.LogError("typeEvent must set to either Start, Fail, Complete");
                    break;
            }
        }            

        internal static void OnLevelStart(string level, string sublevel="")
        {
            GameAnalyticsHelpers.ProgressionEvent("Start", level,sublevel);
            FunGamesApiAnalytics.NewEvent("ga_progression","Start;" + level + ";" + sublevel);
        }    

        internal static void OnLevelComplete(string level, string sublevel="", int score=-1)
        {
            GameAnalyticsHelpers.ProgressionEvent("Complete", level,sublevel,score);
            FunGamesApiAnalytics.NewEvent("ga_progression","Complete;" + level + ";" + sublevel + ";" + score.ToString());
        }   

        internal static void OnLevelFail(string level,string sublevel="",int score=-1)
        {
            GameAnalyticsHelpers.ProgressionEvent("Fail", level,sublevel,score);
            FunGamesApiAnalytics.NewEvent("ga_progression","Fail;" + level + ";" + sublevel + ";" + score.ToString());
        } 

        internal static void NewDesignEvent(string eventId, string eventValue="")
        {
            if (eventValue != "")
            {
                GameAnalyticsHelpers.NewDesignEvent(eventId,eventValue);
                FunGamesApiAnalytics.NewEvent("ga_design",eventId + ";" + eventValue);
            }
            else
            {
                GameAnalyticsHelpers.NewDesignEvent(eventId);
                FunGamesApiAnalytics.NewEvent("ga_design",eventId);
            }
        }

        internal static void NewAdEvent(string adFormat,string adEvent)
        {
            GameAnalyticsHelpers.NewDesignEvent("ad:" + adFormat + ":" + adEvent);
            FunGamesApiAnalytics.NewEvent("ga_design",adFormat + ";" + adEvent);
        }

        internal static void NewCohortEvent(string cohortName, string userCohortAssigned)
        {
            GameAnalyticsHelpers.NewDesignEvent("cohort:" + cohortName + ":" + userCohortAssigned);
            FunGamesApiAnalytics.NewEvent("ga_design",cohortName + ";" + userCohortAssigned);
        }   
    }
}