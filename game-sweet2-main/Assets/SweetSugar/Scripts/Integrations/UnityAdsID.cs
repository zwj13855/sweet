using System;
using SweetSugar.Scripts.System;
using UnityEngine;

namespace SweetSugar.Scripts.Integrations
{
[CreateAssetMenu(fileName = "UnityAdsID", menuName = "UnityAdsID", order = 1)]
    public class UnityAdsID : ScriptableObject
    {
        public bool enable;
        public string androidID;
        public string iOSID;

        private void OnValidate()
        {
            #if UNITY_EDITOR
            if(enable) DefineSymbolsUtils.AddSymbol("UNITY_ADS");
            else DefineSymbolsUtils.DeleteSymbol("UNITY_ADS");
            #endif
        }
    }
}