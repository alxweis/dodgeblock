using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuttleTailManager : MonoBehaviour
{
    public static ShuttleTailManager Instance { set; get; }

    public bool inTrailMenu;

    public RectTransform trailItem;
    public RectTransform trailContent;
    public TrailItem[] trailItems;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < trailItems.Length; i++)
        {
            var item = Instantiate(trailItem, trailContent.position, Quaternion.identity);
            item.SetParent(trailContent, false);
            item.GetComponent<Toggle>().group = trailContent.GetComponent<ToggleGroup>();
            // Show gradient and width of trail in itemSprite
            item.GetChild(1).GetComponent<Image>().sprite = trailItems[i].icon;
            UpdateTrailItems(item, i);
        }
    }

    void Update()
    {
        if (HangarManager.Instance.hangarOpen)
        {
            if (HangarManager.Instance.designMenu.GetChild(2).gameObject.activeSelf)
                inTrailMenu = true;
            else
                inTrailMenu = false;

            if (inTrailMenu)
                for (int i = 0; i < trailContent.childCount; i++)
                {
                    UpdateTrailItems(trailContent.GetChild(i).GetComponent<RectTransform>(), i);
                    SaveTrailItem(trailContent.GetChild(i).GetComponent<RectTransform>(), i);
                }
        }
        else
            inTrailMenu = false;
    }

    void UpdateTrailItems(RectTransform rt, int i)
    {
        if (!trailItems[i].bought)
        {
            rt.GetComponent<Toggle>().interactable = false;
            rt.GetChild(2).gameObject.SetActive(true);
            rt.GetChild(3).GetComponent<Image>().raycastTarget = true; //+++
            if (!trailItems[i].listenerAdded)
            {
                rt.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OnTrailBuyButton(i); });//+++
                trailItems[i].listenerAdded = true;
            }
        }
        else
        {
            rt.GetComponent<Toggle>().interactable = true;
            rt.GetChild(2).gameObject.SetActive(false);
            rt.GetChild(3).GetComponent<Image>().raycastTarget = false; //+++
            rt.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners(); //+++
        }
    }

    void SaveTrailItem(RectTransform rt, int i)
    {
        if (rt.GetComponent<Toggle>().isOn)
        {
            trailItems[i].selected = true;
            PlayerPrefs.SetInt("Trail", i);
            // Show on shuttlePreview
            HangarManager.Instance.primTrail.colorGradient = HangarManager.Instance.secTrail.colorGradient = trailItems[i].gradient;
            HangarManager.Instance.primTrail.widthCurve = HangarManager.Instance.secTrail.widthCurve = trailItems[i].width;
            HangarManager.Instance.secTrail.widthMultiplier = HangarManager.Instance.widthMult;
        }
        else
            trailItems[i].selected = false;
    }

    public void OnTrailBuyButton(int i)
    {
        UIManager.Instance.TrailPopUp(i);
    }
}
