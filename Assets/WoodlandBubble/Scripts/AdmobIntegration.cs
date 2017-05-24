using UnityEngine;
using System.Collections;
using System;
//using GoogleMobileAds.Api;

public class AdmobIntegration : MonoBehaviour
{
    public string adUnitIdAndroidInterstitial;
    public string adUnitIdIOSInterstitial;
    public string adUnitIdAndroidBanner;
    public string adUnitIdIOSBanner;

    //private BannerView bannerView;
    //private InterstitialAd interstitial;
    public static AdmobIntegration THIS;

    //public bool showBanner;
    //public bool showInterstitial;

    void Awake()
    {
       // DontDestroyOnLoad(this);
        THIS = this;
    }

    //public void RequestBanner(AdPosition position = AdPosition.Bottom)
    //{
//#if UNITY_EDITOR
//        string adUnitId = "unused";
//#elif UNITY_ANDROID
//            string adUnitId = adUnitIdAndroidBanner;
//#elif UNITY_IPHONE
//            string adUnitId = adUnitIdIOSBanner;
//#else
//            string adUnitId = "unexpected_platform";
//#endif

//        // Create a 320x50 banner at the top of the screen.
//        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, position);
//        // Register for ad events.
//        bannerView.OnAdLoaded += HandleAdLoaded;
//        bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
//        bannerView.OnAdOpening += HandleAdOpened;
//        bannerView.OnAdClosed += HandleAdClosed;
//        bannerView.OnAdLeavingApplication += HandleAdLeftApplication;
//        // Load a banner ad.
//        bannerView.LoadAd(createAdRequest());
    //}

    public void RequestInterstitial()
    {
//#if UNITY_EDITOR
//        string adUnitId = "unused";
//#elif UNITY_ANDROID
//            string adUnitId = adUnitIdAndroidInterstitial;
//#elif UNITY_IPHONE
//            string adUnitId = adUnitIdIOSInterstitial;
//#else
//            string adUnitId = "unexpected_platform";
//#endif

//        // Create an interstitial.
//        interstitial = new InterstitialAd(adUnitId);
//        // Register for ad events.
//        interstitial.OnAdLoaded += HandleInterstitialLoaded;
//        interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
//        interstitial.OnAdOpening += HandleInterstitialOpened;
//        interstitial.OnAdClosed += HandleInterstitialClosed;
//        interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;
//        // Load an interstitial ad.
//        interstitial.LoadAd(createAdRequest());
    }

    // Returns an ad request with custom ad targeting.
    //private AdRequest createAdRequest()
    //{
    //    return new AdRequest.Builder()
    //    //.AddTestDevice(AdRequest.TestDeviceSimulator)
    //    //.AddTestDevice("A4EEFE36040C8D0DAF446924CEEA53E7")
    //            .AddKeyword("game")
    //            .SetGender(Gender.Male)
    //            .SetBirthday(new DateTime(1985, 1, 1))
    //            .TagForChildDirectedTreatment(false)
    //            .AddExtra("color_bg", "9B30FF")
    //            .Build();

    //}

    public void ShowInterstitial()
    {
        //if (interstitial.IsLoaded())
        //{
        //    interstitial.Show();
        //}
        //else
        //{
        //    TacoConfig.print("Interstitial is not ready yet.");
        //}
    }

    public void ShowBanner()
    {
        //bannerView.Show();
    }

    //public void HideBanner()
    //{
    //    bannerView.Hide();
    //}


    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        //TacoConfig.print("HandleAdLoaded event received.");
    }

    //public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //{
    //    TacoConfig.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    //}

    public void HandleAdOpened(object sender, EventArgs args)
    {
        //TacoConfig.print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        //TacoConfig.print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        //TacoConfig.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        //TacoConfig.print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        InitScript.adReceived = true;
        //TacoConfig.print("HandleInterstitialLoaded event received.");
    }

    //public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //{
    //    print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    //}

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
       print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion
}
