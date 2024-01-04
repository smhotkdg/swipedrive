using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Analytics;
using UnityEditor;
using GameAnalyticsSDK;
using Proyecto26;
using System.Text;  

namespace FunGames.Sdk.Replay
{
    [Serializable]
    public class TrackingParameters
    {
        public string bundle_id;
        public string user_id;
        public string session_id;
        public string os;
        public string model;
        public string path;
    }
	internal class FunGamesReplay: MonoBehaviour
    {
        internal static List<int> touchTsAllSessions = new List<int>();
        internal static List<int> touchTypeAllSessions = new List<int>();
        internal static List<float> touchXAllSessions = new List<float>(); 
        internal static List<float> touchYAllSessions =new List<float>(); 
        internal static List<long> touchMilliAllSessions =new List<long>(); 
        internal static float screenWidth =  Screen.width / 2.0f;
        internal static DateTime dt0 = DateTime.Now;
        internal static long milli0 = dt0.Ticks / TimeSpan.TicksPerMillisecond;
        internal static float screenHeight = Screen.height / 2.0f;
        internal string gameStoryAllSessions; 
        internal static int maxStringLength = 7000;
        internal static string url = "https://api.tnapps.xyz/v1/user_path";
        internal static string idfa = "";  
        internal static bool runReplay = true;
        internal static float xPreviousDragged = 0 ; 
        internal static float yPreviousDragged = 0 ; 
        internal static float epsilon = 0.001f;

    
        void Awake()
        {
            FunGamesSettings settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
            if (settings.runReplay == false)
            {
                runReplay = false;
            }

             if (Application.isEditor == false)
            {
                Application.RequestAdvertisingIdentifierAsync(
                    (string advertisingId, bool trackingEnabled, string error) =>{ 
                        idfa = advertisingId;
                    }
                );
            }
            else
            {
                idfa = "unity-editor";
            }
        }

        internal static void Start()
        {

        }

        internal static void ResetVariables()
        {
            touchTsAllSessions.Clear();
            touchTypeAllSessions.Clear();
            touchXAllSessions.Clear();            
            touchYAllSessions.Clear();
            touchMilliAllSessions.Clear();
            
            dt0 = DateTime.Now;
            milli0 = dt0.Ticks / TimeSpan.TicksPerMillisecond;
        }

        internal static string CreateGameStory(List<Int32> touchTs_,List<long> touchMilli_,List<int> touchType_,List<float> touchX_,List<float> touchY_)
        {
            string gameStoryTmp = milli0.ToString() + "_";
            for (int i = 0; i < touchX_.Count; i++)
            {
                if ( i == touchX_.Count-1)
                {
                    gameStoryTmp += touchMilli_[i].ToString() + "_" + touchType_[i].ToString() + "_" + touchX_[i].ToString() + "_" + touchY_[i].ToString() + ";";
                }
                else
                {
                    gameStoryTmp += touchMilli_[i].ToString() + "_" + touchType_[i].ToString() + "_" + touchX_[i].ToString() + "_" + touchY_[i].ToString() + ";";
                }
            }
            return gameStoryTmp;   
        }

        internal static void AddTouchEvent(int eventType,float x, float y)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            long milli = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long milliDiff = milli - milli0;

            touchXAllSessions.Add(x);
            touchYAllSessions.Add(y);
            touchTsAllSessions.Add(unixTimestamp);
            touchMilliAllSessions.Add(milliDiff);
            touchTypeAllSessions.Add(eventType);
        }

        internal static Boolean TrackTouchEvent()
        {
            Boolean newTouchEvent = false;

            if(Input.touchCount == 1)
            {    
                Touch touch = Input.GetTouch(0);
                Vector2 pos = touch.position;
                float x = (pos.x - screenWidth) / screenWidth;
                float y = (pos.y - screenHeight) / screenHeight;

                x = Convert.ToSingle(Math.Round(x, 4, MidpointRounding.ToEven)); 
                y = Convert.ToSingle(Math.Round(y, 4, MidpointRounding.ToEven)); 

                if(touch.phase == TouchPhase.Began)
                {
                    AddTouchEvent(0,x,y);
                }
                // release touch/dragging
                else if((Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
                {
                    AddTouchEvent(1,x,y);
                }
                //dragging
                else
                {
                    if ((Math.Abs(x - xPreviousDragged) < epsilon) & (Math.Abs(y - yPreviousDragged) < epsilon))
                    {
                        UnityEngine.Debug.Log("same input");
                    }
                    else
                    {
                        AddTouchEvent(2,x,y);
                        xPreviousDragged = x;
                        yPreviousDragged = y;
                    }
                }
                newTouchEvent = true;
            }

            //if (Input.GetButtonDown("Fire1") | Input.GetMouseButton(0))
            if (Input.GetButtonDown("Fire1"))
            {
                Vector2 pos = Input.mousePosition;
                //UnityEngine.Debug.Log(pos.x + "_" + pos.y);

                float x = (pos.x - screenWidth) / screenWidth;
                float y = (pos.y - screenHeight) / screenHeight;

                x = Convert.ToSingle(Math.Round(x, 4, MidpointRounding.ToEven)); 
                y = Convert.ToSingle(Math.Round(y, 4, MidpointRounding.ToEven)); 
                AddTouchEvent(1,x,y);

                newTouchEvent = true;
            }    

            else if (Input.GetButtonUp("Fire1"))
            {
                Vector2 pos = Input.mousePosition;
                //UnityEngine.Debug.Log(pos.x + "_" + pos.y);
                float x = (pos.x - screenWidth) / screenWidth;
                float y = (pos.y - screenHeight) / screenHeight;

                x = Convert.ToSingle(Math.Round(x, 4, MidpointRounding.ToEven)); 
                y = Convert.ToSingle(Math.Round(y, 4, MidpointRounding.ToEven)); 

                AddTouchEvent(2,x,y);
                newTouchEvent = true;
            }
            dt0 = DateTime.Now;   
            return newTouchEvent;
        }

        void Update()
        {
            Boolean newTouchEvent  = TrackTouchEvent();
            //UnityEngine.Debug.Log(newTouchEvent);
            if (newTouchEvent)
            {
                gameStoryAllSessions = CreateGameStory(touchTsAllSessions,touchMilliAllSessions,touchTypeAllSessions,touchXAllSessions,touchYAllSessions);
                if ((gameStoryAllSessions.Length >= maxStringLength) & (runReplay))
                {
                    UnityEngine.Debug.Log(gameStoryAllSessions);
                    GameAnalytics.NewErrorEvent(GAErrorSeverity.Info,gameStoryAllSessions);
                    UnityEngine.Debug.Log(gameStoryAllSessions);
                    PostRequest(gameStoryAllSessions);
                    ResetVariables();
                }
            }
        }

        void OnApplicationPause(bool pause)
        {
            if (pause==true)
            {
                gameStoryAllSessions = CreateGameStory(touchTsAllSessions,touchMilliAllSessions,touchTypeAllSessions,touchXAllSessions,touchYAllSessions);
                if ((gameStoryAllSessions!= "" ) & (runReplay))
                {
                    UnityEngine.Debug.Log(gameStoryAllSessions);
                    GameAnalytics.NewErrorEvent(GAErrorSeverity.Info,gameStoryAllSessions);
                    UnityEngine.Debug.Log(gameStoryAllSessions);
                    PostRequest(gameStoryAllSessions);
                    ResetVariables();
                }
            }
        }

        internal static string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        internal static void PostRequest(string gameStory)
        {
            //Dictionary <string, string> trackingParams = new Dictionary<string, string>();
            TrackingParameters trackingParameters = new TrackingParameters();
            trackingParameters.bundle_id = Application.identifier;
            trackingParameters.user_id = AnalyticsSessionInfo.userId;
            trackingParameters.session_id = AnalyticsSessionInfo.sessionId.ToString();
            trackingParameters.os = SystemInfo.operatingSystem;
            trackingParameters.model = SystemInfo.deviceModel;
            trackingParameters.path = gameStory;

            string parametersString = JsonUtility.ToJson(trackingParameters);

            char[] array1 = { '\u0074','\u0061','\u0070','\u006E','\u0061','\u0074','\u0069','\u006F','\u006E','\u002D','\u0073','\u0065','\u0063','\u0072','\u0065','\u0074' };
            var myString = new string(array1);

            string hash = CreateToken(parametersString,myString);
            hash = hash.Remove(hash.Length-1);

            MD5 md5Hash = MD5.Create();
            byte [] result = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(myString));
            string bitString = BitConverter.ToString(result).Replace("-","").ToLower();  


            RestClient.DefaultRequestHeaders["Content-Type"] = "application/json";
            RestClient.DefaultRequestHeaders["Authorization"] = "hmac " + bitString + " " + hash;
            RestClient.DefaultRequestHeaders["x-device-id"] = idfa;
            
            RestClient.Post(url, trackingParameters).Then(response => 
            {
                ParseResponse(response.Text);
            }).Catch(err => {
                UnityEngine.Debug.Log (err.Message);
            });
        }

        internal static void ParseResponse(string message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}