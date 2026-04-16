using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPButton : MonoBehaviour
{
    public enum ItemType
    {
        Coins_2500,
        Coins_10000,
        Coins_25000,
        Coins_75000,
        Coins_150000,
        Remove_Ads,
    }

    public ItemType itemType;
    public Text priceText;

    void Start()
    {
        StartCoroutine(LoadPrice());
    }

    public void ClickBuy()
    {
        switch (itemType)
        {
            case ItemType.Coins_2500:
                Purchaser.Instance.Buy2500Coins();
                break;
            case ItemType.Coins_10000:
                Purchaser.Instance.Buy10000Coins();
                break;
            case ItemType.Coins_25000:
                Purchaser.Instance.Buy25000Coins();
                break;
            case ItemType.Coins_75000:
                Purchaser.Instance.Buy75000Coins();
                break;
            case ItemType.Coins_150000:
                Purchaser.Instance.Buy150000Coins();
                break;
            case ItemType.Remove_Ads:
                Purchaser.Instance.BuyRemoveAds();
                break;
        }
    }

    IEnumerator LoadPrice()
    {
        while (!Purchaser.Instance.IsInitialized())
            yield return null;

        string loadedPrice = "";

        switch (itemType)
        {
            case ItemType.Coins_2500:
                loadedPrice = Purchaser.Instance.GetProductPriceFromStore(Purchaser.Instance.COINS_2500);
                break;
            case ItemType.Coins_10000:
                loadedPrice = Purchaser.Instance.GetProductPriceFromStore(Purchaser.Instance.COINS_10000);
                break;
            case ItemType.Coins_25000:
                loadedPrice = Purchaser.Instance.GetProductPriceFromStore(Purchaser.Instance.COINS_25000);
                break;
            case ItemType.Coins_75000:
                loadedPrice = Purchaser.Instance.GetProductPriceFromStore(Purchaser.Instance.COINS_75000);
                break;
            case ItemType.Coins_150000:
                loadedPrice = Purchaser.Instance.GetProductPriceFromStore(Purchaser.Instance.COINS_150000);
                break;
            case ItemType.Remove_Ads:
                loadedPrice = Purchaser.Instance.GetProductPriceFromStore(Purchaser.Instance.REMOVE_ADS);
                break;
        }

        priceText.text = loadedPrice;
    }
}
