using System;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine.Analytics;
using System.Linq;
using TinyJson;
using FunGames.Sdk.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;


#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID

#endif

namespace FunGames.Sdk.Cohort
{
    internal class FunGamesCohort: MonoBehaviour
    {

        public static FunGamesSettings funGamesSettings ;

        private void Awake()
        {
            funGamesSettings = Resources.Load<FunGamesSettings>("FunGamesSettings");
        }

        internal static int GetCohort()
        {
            if (funGamesSettings.runCohort)
            {
                int localCohort = CheckLocalCohort();
                
                if (localCohort!=-1)
                {
                    return localCohort;
                }
                else
                {
                    localCohort = CreateNewLocalCohort();
                    return localCohort;
                }
            }
            else
            {
                return 0;
            }
        }

        internal static int CheckLocalCohort()
        {
            string cohortTestName = funGamesSettings.cohortTestName;
            if (PlayerPrefs.HasKey("tnappsCohort:" + cohortTestName))
            {
                return PlayerPrefs.GetInt("tnappsCohort:" + cohortTestName);
            }
            else
            {
                return -1;
            }
        }
        
        internal static int CreateNewLocalCohort()
        {
            float cohortPercentage = funGamesSettings.cohortPercentage;
            string cohortTestName = funGamesSettings.cohortTestName;
            double userCohortAssigned = UnityEngine.Random.value;
            if (userCohortAssigned < cohortPercentage)
            {
                PlayerPrefs.SetInt("tnappsCohort:" + cohortTestName,1);
                FunGamesAnalytics.NewCohortEvent(cohortTestName,"1");
                return 1;
            }
            else
            {
                PlayerPrefs.SetInt("tnappsCohort:" + cohortTestName,0);
                FunGamesAnalytics.NewCohortEvent(cohortTestName,"0");
                return 0;
            }
        }
    }
}