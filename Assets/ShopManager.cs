using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { set; get; }

    public Animator shopAnim;
    public bool shopOpen;
    [HideInInspector]
    public RectTransform shopContent;

    [Header("Shield")]
    public int shieldPrice = 300;
    [HideInInspector]
    public Button shieldBuy;

    [Header("Remove Ads")]
    public Button removeAds;
    public GameObject checkMark;

    void Awake()
    {
        Instance = this;

        if (AdvertismentManager.Instance.noAds)
        {
            removeAds.interactable = false;
            checkMark.SetActive(true);
        }
    }

    void Update()
    {
        shieldBuy.interactable = ScoreManager.Instance.coins < shieldPrice ? false : true; 
    }

    public void OnShopOpen()
    {
        shopAnim.SetTrigger("HangarOpen");
        shopOpen = true;

        Vector2 pos = shopContent.anchoredPosition;
        pos.y = 0f;
        shopContent.anchoredPosition = pos;
    }
    public void OnShopClose()
    {
        shopAnim.SetTrigger("HangarClose");
        shopOpen = false;
    }

    public void OnShieldPlusButton()
    {
        if(!shopOpen)
            OnShopOpen();

        if (HangarManager.Instance.hangarOpen)
            HangarManager.Instance.OnHangarClose();

        Vector2 pos = shopContent.anchoredPosition;
        pos.y = 325f;
        shopContent.anchoredPosition = pos;
    }
    public void OnCoinPlusButton()
    {
        if (!shopOpen)
            OnShopOpen();

        if (HangarManager.Instance.hangarOpen)
            HangarManager.Instance.OnHangarClose();

        Vector2 pos = shopContent.anchoredPosition;
        pos.y = 990f;
        shopContent.anchoredPosition = pos;
    }
    public void OnRemoveAdButton()
    {
        if (!shopOpen)
            OnShopOpen();

        if (HangarManager.Instance.hangarOpen)
            HangarManager.Instance.OnHangarClose();

        Vector2 pos = shopContent.anchoredPosition;
        pos.y = 0f;
        shopContent.anchoredPosition = pos;
    }

    public void OnShieldBuy()
    {
        AudioManager.Instance.Play("Buy");
        ScoreManager.Instance.coins -= shieldPrice;
        ScoreManager.Instance.shields++;
    }

    public void BuyRemoveAds()
    {
        AudioManager.Instance.Play("Buy");
        PlayerPrefs.SetInt("NoAds", 1);
        AdvertismentManager.Instance.noAds = true;
        removeAds.interactable = false;
        checkMark.SetActive(true);
    }
    public void BuyCoins(int amount)
    {
        AudioManager.Instance.Play("Buy");
        GameManager.Instance.CollectCoins(amount);
    }
}
