using System.Collections.Generic;
using UnityEngine;
using System;

namespace FunGames.Sdk.Analytics.Helpers
{

    [System.Serializable]
    public class FunGamesTracking
    {
        public string idfa;
        public string bundle_id;
        public string session_id;
        public string os;
        public List<Metrics> metrics;
    }

    [System.Serializable]
    public class Metrics
    {
        public string evt;
        public string value;
        public string ts;
    }

}

