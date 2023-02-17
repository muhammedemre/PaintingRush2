using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private InterstitialAd interstitial;
    string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // testid: ca-app-pub-3940256099942544/1033173712 // real id: ca-app-pub-4426570100779360/7306994392
    void Start()
    {     
        
    }

    public void PlayInterstitialAd()
    {
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoaded;

        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpened;

        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
    }

    private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        // Show the interstitial.
        interstitial.Show();
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // Handle the ad failed to load event.
    }

    private void HandleOnAdOpened(object sender, EventArgs args)
    {
        // Handle the ad opened event.
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        // Create a new interstitial ad.
        interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    private void OnDestroy()
    {
        // Destroy the interstitial when it's no longer needed.
        interstitial.Destroy();
    }
}

