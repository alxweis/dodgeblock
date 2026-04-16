using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertismentManager : MonoBehaviour
{
    public static AdvertismentManager Instance { set; get; }

    public GameObject adPanel;
    public int maxContinousAd;
    public bool noAds;

    void Awake()
    {
        Instance = this;
        if (!PlayerPrefs.HasKey("NoAds"))
            PlayerPrefs.SetInt("NoAds", 0);

        noAds = PlayerPrefs.GetInt("NoAds") == 1 ? true : false;
    }

    public void ShowAd(string tag, ShowOptions options)
    {
        //if(Application.internetReachability != NetworkReachability.NotReachable)
        //{
        //    adPanel.SetActive(true);
        //    if (Advertisement.IsReady(tag))
        //    {
        //        Advertisement.Show(tag, options);
        //    }
        //    else
        //    {
        //        adPanel.SetActive(false);
        //        UIManager.Instance.InfoPopUp("ERROR", "Playing the ad failed!");
        //    }
        //}
        //else
        //{
        //    UIManager.Instance.InfoPopUp("ERROR", "Please check your internet connection!");
        //}
    }

    public void ShowRandomAd(string tag)
    {
        //if (noAds)
        //    return;

        //if(Random.value < (AudioManager.Instance.continuousAd / maxContinousAd))
        //{
        //    if (Application.internetReachability != NetworkReachability.NotReachable && Advertisement.IsReady(tag))
        //    {
        //        adPanel.SetActive(true);
        //        Advertisement.Show(tag, new ShowOptions { resultCallback = ShowRandomAdResult });
        //    }
        //    AudioManager.Instance.continuousAd = 0;
        //}
        //else
        //{
        //    AudioManager.Instance.continuousAd++;
        //}
    }
    void ShowRandomAdResult(ShowResult result)
    {
        adPanel.SetActive(false);
    }

    public void BuyCoinsViaAd()
    {
        //ShowAd("rewardedVideo", new ShowOptions { resultCallback = BuyCoinsViaAdResult });
    }
    void BuyCoinsViaAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                ShopManager.Instance.BuyCoins(1000);
                adPanel.SetActive(false);
                break;
            case ShowResult.Skipped:
                UIManager.Instance.InfoPopUp("Error", "Playing the ad failed!");
                break;
            case ShowResult.Failed:
                UIManager.Instance.InfoPopUp("Error", "Playing the ad failed!");
                break;
        }
    }
}
