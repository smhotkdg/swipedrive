using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.Analytics;
using System;
using FunGames.Sdk.Analytics.Helpers;
using System.Collections;
using System.Security.Cryptography;
using System.Text;  

namespace FunGames.Sdk.Analytics.Helpers
{
	internal class FunGamesApiAnalytics
    {
        static string AnalyticsUrl = "https://api.tnapps.xyz/v1/tracking";
        static string idfa = "";

        internal static void Initialize()
        {
            string datetimeString = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            if (Application.isEditor== false)
            {
                Application.RequestAdvertisingIdentifierAsync(
                    (string advertisingId, bool trackingEnabled, string error) =>{ 
                        idfa = advertisingId;
                        NewEvent("ga_user",datetimeString);
                    }
                );
            }
            else
            {
                idfa = "unity-editor";
                NewEvent("ga_user",datetimeString);
            }
        }
        
        internal static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        internal static Dictionary<string, string> GetUserInfo(Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            string userId = AnalyticsSessionInfo.userId;
            string sessionId = AnalyticsSessionInfo.sessionId.ToString();
            parameters.Add("bundle_id", Application.identifier);
            parameters.Add("user_id", userId);
            parameters.Add("session_id",sessionId);
            parameters.Add("idfa",idfa);
            parameters.Add("os",SystemInfo.operatingSystem);
            return parameters;
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
        
        internal static void NewEvent(string eventName, string value)
        {
            Dictionary<string, string> userInfo = GetUserInfo();

            FunGamesTracking trackingParams = new FunGamesTracking()
            {
                idfa = userInfo["idfa"],
                bundle_id = userInfo["bundle_id"],
                session_id = userInfo["session_id"],
                os = userInfo["os"],
                metrics = new List<Metrics>
                {
                    new Metrics()
                    {
                        evt = eventName,
                        value = value.ToString(),
                        ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
                    },
                }
            };

            char[] array1 = { '\u0074','\u0061','\u0070','\u006E','\u0061','\u0074','\u0069','\u006F','\u006E','\u002D','\u0073','\u0065','\u0063','\u0072','\u0065','\u0074' };
            var myString = new string(array1);

            string parametersString = JsonUtility.ToJson(trackingParams);
            Debug.Log (parametersString);

            string hash = CreateToken(parametersString,myString);
            hash = hash.Remove(hash.Length-1);

            MD5 md5Hash = MD5.Create();
            byte [] result = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(myString));
            string bitString = BitConverter.ToString(result).Replace("-","").ToLower();

            RestClient.DefaultRequestHeaders["Content-Type"] = "application/json";
            RestClient.DefaultRequestHeaders["Authorization"] = "hmac " + bitString + " " + hash;
            RestClient.DefaultRequestHeaders["User-Agent"] = SystemInfo.deviceModel;

            Debug.Log (bitString);
            RestClient.Post(FunGamesApiAnalytics.AnalyticsUrl, trackingParams).Then(response => {
                ParseResponse(response.Text);
            }).Catch(err => {
                Debug.Log (err.Message);
            });
        }

        internal static void ParseResponse(string response)
        {
            Debug.Log(response);
        }
    }
}