using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    public float percentageReward;
    public int rewardCoins;
    Text coinText;
    Button button;
    bool b;

    void Start()
    {
        coinText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        if (GameManager.Instance.isDead && !b)
        {
            rewardCoins = (int)(ScoreManager.Instance.coinScore / 100f * percentageReward);

            if (rewardCoins >= 1000 && rewardCoins < 1000000)
                coinText.text = "+" + (rewardCoins / 1000f).ToString("F1") + "K";
            else if (rewardCoins >= 1000000 && rewardCoins < 1000000000)
                coinText.text = "+" + (rewardCoins / 1000000f).ToString("F2") + "M";
            else
                coinText.text = "+" + rewardCoins.ToString("#,0");
        }
    }

    public void OnReward(Button b)
    {
        button = b;
        //AdvertismentManager.Instance.ShowAd("rewardedVideo", new ShowOptions { resultCallback = HandleAdResult });
    }

    void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Reward();
                AdvertismentManager.Instance.adPanel.SetActive(false);
                break;
            case ShowResult.Skipped:
                UIManager.Instance.InfoPopUp("ERROR", "Playing the ad failed!");
                break;
            case ShowResult.Failed:
                UIManager.Instance.InfoPopUp("ERROR", "Playing the ad failed!");
                break;
        }
    }

    void Reward()
    {
        b = true;
        GameManager.Instance.CollectCoins(rewardCoins);
        button.interactable = false;
    }
}
