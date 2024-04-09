using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SweetSugar.Scripts.System
{
    public class ServerTime : MonoBehaviour
    {
        public static ServerTime THIS;
        public DateTime serverTime;
        public bool dateReceived;
        public delegate void DateReceived();
        public static event DateReceived OnDateReceived;

        private void Awake()
        {
            THIS = this;
            GetServerTime();
        }
               
        void GetServerTime()
        {
            StartCoroutine(IEGetTime());
        }

        //获取网络时间
        IEnumerator IEGetTime()
        {
            using (UnityWebRequest www = UnityWebRequest.Get("https://worldtimeapi.org/api/ip"))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to get server time: " + www.error);
                    serverTime = DateTime.Now;
                }
                else
                {
                    string response = www.downloadHandler.text;
                    WorldTimeApiResponse timeResponse = JsonUtility.FromJson<WorldTimeApiResponse>(response);
                    serverTime = DateTime.Parse(timeResponse.datetime);
                }

                dateReceived = true;
                OnDateReceived?.Invoke();
            }
        }
    }

    [Serializable]
    public class WorldTimeApiResponse
    {
        public string datetime;
    }
}
