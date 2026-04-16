using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public string upgradeName;
    public int startPrice;
    public int level;

    int price;
    int lastPrice;

    public float[] levelValues;

    [Space(10)]
    public Animator anim;
    public Button buyButton;
    public Text priceText;
    public RectTransform preview;
    public Text now, upgrade, nextUp;
    Color red = new Color(255f / 255f, 118f / 255f, 117f / 255f);

    void Start()
    {
        CalcPrice();
        priceText.text = ScoreManager.Instance.ShortNumStr(price);
        ItemManager.Instance.Item(upgradeName).duration = levelValues[level];
        UpgradePreview();
    }

    void Update()
    {
        if (ScoreManager.Instance.coins < price)
            buyButton.interactable = false;
        else
            buyButton.interactable = true;

        if (price != lastPrice)
        {
            priceText.text = ScoreManager.Instance.ShortNumStr(price);
            lastPrice = price;
        }
    }

    public void OnUpgradeButton()
    {
        AudioManager.Instance.Play("Buy");
        ItemManager.Instance.Item(upgradeName).duration = levelValues[level + 1];
        ScoreManager.Instance.coins -= price;

        level++;
        if (level < (levelValues.Length - 1))
        {
            CalcPrice();
            anim.SetTrigger("Upgrade");
        }

        UpgradePreview();
        HangarManager.Instance.SaveHangarData();
    }

    void UpgradePreview()
    {
        if (level == 0)
        {
            upgrade.text = levelValues[level] + " s";
            nextUp.text = levelValues[level + 1] + " s";
        }
        else if (level < (levelValues.Length - 1))
        {
            now.text = levelValues[level - 1] + " s";
            upgrade.text = levelValues[level] + " s";
            if ((level + 1) < levelValues.Length)
                nextUp.text = levelValues[level + 1] + " s";
        }
        else
        {
            buyButton.gameObject.SetActive(false);
            for (int i = 0; i < preview.childCount; i++)
            {
                preview.GetChild(i).gameObject.SetActive(false);
                if (i == preview.childCount - 1)
                {
                    preview.GetChild(i).gameObject.SetActive(true);
                    preview.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = levelValues[level] + " s";
                }
            }
        }

        if (level == (levelValues.Length - 2))
            nextUp.transform.GetComponent<Text>().color = red;
    }

    void CalcPrice()
    {
        float f = startPrice * Mathf.Pow(2f, level);

        if (f > 99f && f < 1000f)
            price = (int)(Mathf.Round(f / 10f) * 10f);
        else
            price = (int)(Mathf.Round(f / 100f) * 100f);
    }
}
