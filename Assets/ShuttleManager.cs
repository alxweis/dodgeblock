using UnityEngine;
using UnityEngine.UI;

public class ShuttleManager : MonoBehaviour
{
    public static ShuttleManager Instance { set; get; }

    public bool inShuttleMenu;

    public RectTransform shuttleItem;
    public RectTransform shuttleContent;
    public ShuttleItem[] shuttleItems;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < shuttleItems.Length; i++)
        {
            var item = Instantiate(shuttleItem, shuttleContent.position, Quaternion.identity);
            item.SetParent(shuttleContent, false);
            item.GetComponent<Toggle>().group = shuttleContent.GetComponent<ToggleGroup>();
            item.GetChild(1).GetComponent<Image>().sprite = shuttleItems[i].prim;
            item.GetChild(1).GetChild(0).GetComponent<Image>().sprite = shuttleItems[i].sec;
            item.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.gray;
            UpdateShuttleItems(item, i);
        }
    }

    void Update()
    {
        if (HangarManager.Instance.hangarOpen)
        {
            if (HangarManager.Instance.designMenu.GetChild(0).gameObject.activeSelf)
                inShuttleMenu = true;
            else
                inShuttleMenu = false;

            if (inShuttleMenu)
                for (int i = 0; i < shuttleContent.childCount; i++)
                {
                    UpdateShuttleItems(shuttleContent.GetChild(i).GetComponent<RectTransform>(), i);
                    SaveShuttleItem(shuttleContent.GetChild(i).GetComponent<RectTransform>(), i);
                }
        }
        else
            inShuttleMenu = false;
    }

    void UpdateShuttleItems(RectTransform rt, int i)
    {
        if (!shuttleItems[i].bought)
        {
            rt.GetComponent<Toggle>().interactable = false;
            rt.GetChild(2).gameObject.SetActive(true);
            rt.GetChild(3).GetComponent<Image>().raycastTarget = true; //+++
            if (!shuttleItems[i].listenerAdded)
            {
                rt.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OnShuttleBuyButton(i); });//+++
                shuttleItems[i].listenerAdded = true;
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

    void SaveShuttleItem(RectTransform rt, int i)
    {
        if (rt.GetComponent<Toggle>().isOn)
        {
            shuttleItems[i].selected = true;
            PlayerPrefs.SetInt("Shuttle", i);
            HangarManager.Instance.shuttlePreview.GetComponent<Image>().sprite = HangarManager.Instance.prim.sprite = shuttleItems[i].prim;
            HangarManager.Instance.shuttlePreview.GetChild(0).GetComponent<Image>().sprite = HangarManager.Instance.sec.sprite = shuttleItems[i].sec;
        }
        else
            shuttleItems[i].selected = false;
    }

    public void OnShuttleBuyButton(int i)
    {
        UIManager.Instance.DesignPopUp(i);
    }
}
