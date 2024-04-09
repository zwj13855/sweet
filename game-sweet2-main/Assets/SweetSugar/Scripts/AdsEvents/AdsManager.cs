#if CHARTBOOST_ADS
using ChartboostSDK;
#endif
using System;
using System.Collections.Generic;
using SweetSugar.Scripts.Core;
using SweetSugar.Scripts.Integrations;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_WEBGL || UNITY_WEBGL_API
using UnityEngine.Advertisements;

#endif
#if GOOGLE_MOBILE_ADS
using GoogleMobileAds.Api;

#endif
#if UNITY_ADS
using UnityEngine.Advertisements;

#endif
namespace SweetSugar.Scripts.AdsEvents
{
    /// <summary>
    /// Ads manager responsible for initialization and showing
    /// </summary>
    public class AdsManager : UnityEngine.MonoBehaviour, IUnityAdsLoadListener,IUnityAdsInitializationListener
    {
        public static AdsManager THIS;

        //EDITOR: ads events
        public List<AdEvents> adsEvents = new List<AdEvents>();

        //is unity ads enabled
        public bool enableUnityAds;

        //is admob enabled
        public bool enableGoogleMobileAds;

        //is chartboost enabled
        public bool enableChartboostAds;

        //rewarded zone for Unity ads
        public string rewardedVideoZone;

        private bool IsRewardeAdready = false;
        //admob stuff
#if GOOGLE_MOBILE_ADS
        public InterstitialAd interstitial;
        private AdRequest requestAdmob;
#endif
        public string admobUIDAndroid;
        public string admobRewardedUIDAndroid;
        public string admobUIDIOS;
        public string admobRewardedUIDIOS;
        private AdManagerScriptable adsSettings;

        private void Awake()
        {
            if (THIS == null) THIS = this;
            else if (THIS != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            adsSettings = Resources.Load<AdManagerScriptable>("Scriptable/AdManagerScriptable");
            adsEvents = adsSettings.adsEvents;
            admobUIDAndroid = adsSettings.admobUIDAndroid;
            admobUIDIOS = adsSettings.admobUIDIOS;
            admobRewardedUIDAndroid = adsSettings.admobRewardedUIDAndroid;
            admobRewardedUIDIOS = adsSettings.admobRewardedUIDIOS;
            // 初始化 Unity Ads
            InitializeUnityAds();

            // 初始化 Chartboost
            InitializeChartboost();

            // 初始化 AdMob
            InitializeAdMob();
    }

#if GOOGLE_MOBILE_ADS

        public void HandleInterstitialLoaded(object sender, EventArgs args)
        {
            print("HandleInterstitialLoaded event received.");
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            print("HandleInterstitialFailedToLoad event received with message: " + args.LoadAdError);
        }
#endif

        //广告准备
        public bool GetRewardedUnityAdsReady()
        {
#if APPODEAL
        return AppodealIntegration.THIS.IsRewardedLoaded();
#endif
#if UNITY_ADS
            rewardedVideoZone = "rewardedVideo";
            CheckRewardedsReady(rewardedVideoZone);
            return IsRewardeAdready;
#endif
            #if GOOGLE_MOBILE_ADS
            return RewAdmobManager.THIS.IsRewardedAdIsLoaded();
            #endif

            return false;
        }

        private void OnDisable()
        {
#if GOOGLE_MOBILE_ADS
            if (interstitial != null)
            {
                interstitial.OnAdLoaded -= HandleInterstitialLoaded;
                interstitial.OnAdFailedToLoad -= HandleInterstitialFailedToLoad;
            }

#endif
        }

        public delegate void RewardedShown();

        public static event RewardedShown OnRewardedShown;


        //展示奖励广告
        public void ShowRewardedAds()
        {
#if APPODEAL
        Debug.Log("show Rewarded ads video in " + LevelManager.THIS.gameStatus);

        if (GetRewardedUnityAdsReady())
        {
            AppodealIntegration.THIS.ShowRewardedAds();
        }
        else{
            #if UNITY_ADS
            Advertisement.Show(rewardedVideoZone, new ShowOptions
            {
                resultCallback = result =>
                {
                    if (result == ShowResult.Finished)
                    {
                        OnRewardedShown?.Invoke();
                        InitScript.Instance.ShowReward();
                    }
                }
            });
            #endif
        }
#elif UNITY_ADS
            Debug.Log("show Rewarded ads video in " + LevelManager.THIS.gameStatus);

            if (GetRewardedUnityAdsReady())
            {
#if UNITY_ADS
                Advertisement.Load(rewardedVideoZone,this);
#endif
            }
#elif GOOGLE_MOBILE_ADS //2.2
            bool stillShow = true;

#if UNITY_ADS
        stillShow = !GetRewardedUnityAdsReady ();
#endif
            if (stillShow)
                RewAdmobManager.THIS.ShowRewardedAd(InitScript.Instance.ShowReward);
#endif
        }

        //检查广告状态
        public void CheckAdsEvents(GameState state)
        {
            foreach (var item in adsEvents)
            {
                if (item.gameEvent == state)
                {
                    item.calls++;
                    if (item.calls % item.everyLevel == 0)
                        ShowAdByType(item.adType);
                }
            }
        }

        //广告类型
        void ShowAdByType(AdType adType)
        {
            if (adType == AdType.AdmobInterstitial && enableGoogleMobileAds)
                ShowAds(false);
            else if (adType == AdType.UnityAdsVideo && enableUnityAds)
                ShowVideo();
            else if (adType == AdType.ChartboostInterstitial && enableChartboostAds)
                ShowAds(true);
            else if (adType == AdType.Appodeal)
                ShowAds(false);
        }

        //显示广告
        public void ShowVideo()
        {
#if UNITY_ADS
            Debug.Log("show Unity ads video in " + LevelManager.THIS.gameStatus);

            Advertisement.Load("video");
#elif GOOGLE_MOBILE_ADS
            Debug.Log("show Admob rewarded video ads in " + LevelManager.THIS.gameStatus);

#endif
        }

        public void CacheRewarded()
        {
#if APPODEAL
        AppodealIntegration.THIS.RequestRewarded();
#endif
        }


        public void ShowAds(bool chartboost = true)
        {
#if APPODEAL
        if(AppodealIntegration.THIS.IsInterstitialLoaded())
        {
            Debug.Log("show  Interstitial in " + LevelManager.THIS.gameStatus);
            AppodealIntegration.THIS.ShowInterstitial();
        }
#endif
            if (chartboost)
            {
#if CHARTBOOST_ADS
            Debug.Log("show Chartboost Interstitial in " + LevelManager.THIS.gameStatus);

            Chartboost.showInterstitial(CBLocation.Default);
            Chartboost.cacheInterstitial(CBLocation.Default);
#endif
            }
            else
            {
#if GOOGLE_MOBILE_ADS
                Debug.Log("show admob Interstitial in " + LevelManager.THIS.gameStatus);
                if (interstitial.IsLoaded())
                {
                    interstitial.Show();
#if UNITY_ANDROID
                    interstitial = new InterstitialAd(admobUIDAndroid);
#elif UNITY_IOS
                interstitial = new InterstitialAd(admobUIDIOS);
#else
				interstitial = new InterstitialAd (admobUIDAndroid);
#endif

                    // Create an empty ad request.
                    requestAdmob = new AdRequest.Builder().Build();
                    // Load the interstitial with the request.
                    interstitial.LoadAd(requestAdmob);
                }
#endif
            }
        }

        //rewardADS 奖励广告加载
        public void CheckRewardedsReady(string placementId)
        {
            Advertisement.Load(placementId,THIS);
        }

        //奖励广告加载
        public void OnUnityAdsAdLoaded(string placementId)
        {
            IsRewardeAdready = true;
        }
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            if (placementId == "video")
            {
                placementId = "defaultZone";
                Advertisement.Load(placementId);
            }
            else if (placementId == "rewardedVideo")
            {
                placementId = "rewardedVideoZone";
                Debug.Log(error);
                IsRewardeAdready = true;
            }
        }

        //加载完成
        private void OnUnityAdsShowComplete(string placementId)
        {
            OnRewardedShown?.Invoke();
            InitScript.Instance.ShowReward();
        }
        

        public void OnInitializationComplete()
        {
            //((IUnityAdsInitializationListener)THIS).OnInitializationComplete();
            ShowRewardedAds();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            ((IUnityAdsInitializationListener)THIS).OnInitializationFailed(error, message);
            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                Advertisement.Initialize(null, THIS);
            }
            else if(Application.platform == RuntimePlatform.Android)
            {
                enableUnityAds = true;
                var unityAds = Resources.Load<UnityAdsID>("Scriptable/UnityAdsID");
                Advertisement.Initialize(unityAds.androidID, THIS);
            }
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                enableUnityAds = true;
                var unityAds = Resources.Load<UnityAdsID>("Scriptable/UnityAdsID");
                Advertisement.Initialize(unityAds.iOSID, THIS);
            }
        }

        private void InitializeUnityAds()
        {
#if UNITY_ADS
            enableUnityAds = true;
            var unityAds = Resources.Load<UnityAdsID>("Scriptable/UnityAdsID");     //获取广告id
#if UNITY_ANDROID
            if(!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(unityAds.androidID, false, THIS);
            }
#elif UNITY_IOS
            if(!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(unityAds.iOSID, false, true, HandleUnityAdsInitializationError);
            }    
#endif
#else
    enableUnityAds = false;
#endif
        }

        private void InitializeChartboost()
        {
            // 初始化 Chartboost
#if CHARTBOOST_ADS //1.6.1
		enableChartboostAds = true;
#else
            enableChartboostAds = false;
#endif
        }

        private void InitializeAdMob()
        {
#if GOOGLE_MOBILE_ADS
            enableGoogleMobileAds = true; //1.6.1
#if UNITY_ANDROID
            MobileAds.Initialize(initStatus => { }); //2.1.6
            interstitial = new InterstitialAd(admobUIDAndroid);
#elif UNITY_IOS
        MobileAds.Initialize(admobUIDIOS);//2.1.6
        interstitial = new InterstitialAd(admobUIDIOS);
#else
        MobileAds.Initialize(admobUIDAndroid);//2.1.6
		interstitial = new InterstitialAd (admobUIDAndroid);
#endif

            // Create an empty ad request.
            requestAdmob = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(requestAdmob);
            interstitial.OnAdLoaded += HandleInterstitialLoaded;
            interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
#else
            enableGoogleMobileAds = false;//1.6.1
#endif
        }
    }
}