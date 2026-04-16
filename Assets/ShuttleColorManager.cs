using UnityEngine;
using UnityEngine.UI;

public class ShuttleColorManager : MonoBehaviour
{
    public static ShuttleColorManager Instance { set; get; }

    public bool inColorMenu;

    public RectTransform colorItem;
    public Transform colorPrimContent, colorSecContent;
    public ShuttleColorItem[] colorItems;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < colorItems.Length; i++)
        {
            var primItem = Instantiate(colorItem, colorPrimContent.position, Quaternion.identity);
            primItem.SetParent(colorPrimContent, false);
            primItem.GetComponent<Toggle>().group = colorPrimContent.GetComponent<ToggleGroup>();
            primItem.GetChild(1).GetComponent<Image>().color = colorItems[i].color;
            UpdateColorItems(primItem, i, true);

            var secItem = Instantiate(colorItem, colorSecContent.position, Quaternion.identity);
            secItem.SetParent(colorSecContent, false);
            secItem.GetComponent<Toggle>().group = colorSecContent.GetComponent<ToggleGroup>();
            secItem.GetChild(1).GetComponent<Image>().color = colorItems[i].color;
            UpdateColorItems(secItem, i, false);
        }
    }

    void Update()
    {
        if (HangarManager.Instance.hangarOpen)
        {
            if (HangarManager.Instance.designMenu.GetChild(1).gameObject.activeSelf) //+++
                inColorMenu = true;
            else
                inColorMenu = false;

            if (inColorMenu)
                for (int i = 0; i < colorPrimContent.childCount; i++)
                {
                    UpdateColorItems(colorPrimContent.GetChild(i).GetComponent<RectTransform>(), i, true);
                    SaveColorItem(colorPrimContent.GetChild(i).GetComponent<RectTransform>(), i, true);

                    UpdateColorItems(colorSecContent.GetChild(i).GetComponent<RectTransform>(), i, false);
                    SaveColorItem(colorSecContent.GetChild(i).GetComponent<RectTransform>(), i, false);
                }
        }
        else
            inColorMenu = false;
    }

    void UpdateColorItems(RectTransform rt, int i, bool isPrim)
    {
        if (!colorItems[i].bought)
        {
            rt.GetComponent<Toggle>().interactable = false;
            rt.GetChild(2).gameObject.SetActive(true);
            rt.GetChild(3).GetComponent<Image>().raycastTarget = true; //+++
            if (isPrim)
            {
                if (!colorItems[i].primListenerAdded)
                {
                    rt.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OnColorBuyButton(i, isPrim); });//+++
                    colorItems[i].primListenerAdded = true;
                }
            }
            else
            {
                if (!colorItems[i].secListenerAdded)
                {
                    rt.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OnColorBuyButton(i, isPrim); });//+++
                    colorItems[i].secListenerAdded = true;
                }
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

    void SaveColorItem(RectTransform rt, int i, bool isPrim)
    {
        if (rt.GetComponent<Toggle>().isOn)
        {
            colorItems[i].selected = true;
            if (isPrim)
            {
                PlayerPrefs.SetInt("PrimColor", i);
                HangarManager.Instance.shuttlePreview.GetComponent<Image>().color = HangarManager.Instance.prim.color = colorItems[i].color;
            }
            else
            {
                PlayerPrefs.SetInt("SecColor", i);
                HangarManager.Instance.shuttlePreview.GetChild(0).GetComponent<Image>().color = HangarManager.Instance.sec.color = colorItems[i].color;
            }
        }
        else
            colorItems[i].selected = false;
    }

    public void OnColorBuyButton(int i, bool isPrim)
    {
        UIManager.Instance.ColorDesignPopUp(i, isPrim);
    }
}
