using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangarManager : MonoBehaviour
{
    public static HangarManager Instance { set; get; }

    public Animator hangarAnim;
    public bool hangarOpen;
    public RectTransform hangarToggle;
    public RectTransform shuttlePreview;

    [Header("Design")]
    public SpriteRenderer prim;
    public SpriteRenderer sec;
    public TrailRenderer primTrail, secTrail;
    [HideInInspector]
    public float widthMult = 0.6f;

    public RectTransform designHeader, designMenu;
    public CanvasGroup design;

    [Header("Upgrade")]
    public CanvasGroup upgrade;
    public RectTransform upgradeContent;
    public List<Upgrade> upgrades;
    
    void Awake()
    {
        Instance = this;

        prim.sprite = ShuttleManager.Instance.shuttleItems[PlayerPrefs.GetInt("Shuttle")].prim;
        prim.color = ShuttleColorManager.Instance.colorItems[PlayerPrefs.GetInt("PrimColor")].color;

        sec.sprite = ShuttleManager.Instance.shuttleItems[PlayerPrefs.GetInt("Shuttle")].sec;
        sec.color = ShuttleColorManager.Instance.colorItems[PlayerPrefs.GetInt("SecColor")].color;

        primTrail.colorGradient = secTrail.colorGradient = ShuttleTailManager.Instance.trailItems[PlayerPrefs.GetInt("Trail")].gradient;
        primTrail.widthCurve = secTrail.widthCurve = ShuttleTailManager.Instance.trailItems[PlayerPrefs.GetInt("Trail")].width;
        secTrail.widthMultiplier = widthMult;

        for (int i = 0; i < upgradeContent.childCount; i++)
        {
            Upgrade up = upgradeContent.GetChild(i).GetComponent<Upgrade>();
            if(up != null)
            {
                upgrades.Add(up);
            }
        }

        LoadHangarData();
    }

    void Update()
    {
        if (hangarOpen || hangarAnim.transform.GetChild(0).gameObject.activeSelf)
        {
            if (design.interactable)
                DesignMenus();

            for (int i = 0; i < hangarToggle.childCount; i++)
            {
                if (hangarToggle.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    if (i == 0)
                        ToggleHangarMenus(design, upgrade);
                    else if (i == 1)
                        ToggleHangarMenus(upgrade, design);
                }
            }
        }
        #region DesignMenu
        for (int i = 0; i < ShuttleManager.Instance.shuttleItems.Length; i++)
            ShuttleManager.Instance.shuttleContent.GetChild(i).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("Shuttle") == i ? true : false;

        for (int i = 0; i < ShuttleColorManager.Instance.colorItems.Length; i++)
        {
            ShuttleColorManager.Instance.colorPrimContent.GetChild(i).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("PrimColor") == i ? true : false;
            ShuttleColorManager.Instance.colorSecContent.GetChild(i).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("SecColor") == i ? true : false;
        }

        for (int i = 0; i < ShuttleTailManager.Instance.trailItems.Length; i++)
            ShuttleTailManager.Instance.trailContent.GetChild(i).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("Trail") == i ? true : false;

        shuttlePreview.GetComponent<Image>().sprite = ShuttleManager.Instance.shuttleItems[PlayerPrefs.GetInt("Shuttle")].prim;
        shuttlePreview.GetChild(0).GetComponent<Image>().sprite = ShuttleManager.Instance.shuttleItems[PlayerPrefs.GetInt("Shuttle")].sec;

        shuttlePreview.GetComponent<Image>().color = ShuttleColorManager.Instance.colorItems[PlayerPrefs.GetInt("PrimColor")].color;
        shuttlePreview.GetChild(0).GetComponent<Image>().color = ShuttleColorManager.Instance.colorItems[PlayerPrefs.GetInt("SecColor")].color;
        #endregion
    }

    public void OnHangarOpen()
    {
        hangarAnim.SetTrigger("HangarOpen");
        hangarOpen = true;

        for (int i = 0; i < designHeader.childCount; i++)
        {
            if (i == 0)
                designHeader.GetChild(i).GetComponent<Toggle>().isOn = true;
            else
                designHeader.GetChild(i).GetComponent<Toggle>().isOn = false;
        }
        for (int i = 0; i < hangarToggle.childCount; i++)
        {
            if (i == 0)
                hangarToggle.GetChild(i).GetComponent<Toggle>().isOn = true;
            else
                hangarToggle.GetChild(i).GetComponent<Toggle>().isOn = false;
        }
    }
    public void OnHangarClose()
    {
        hangarAnim.SetTrigger("HangarClose");
        hangarOpen = false;
    }

    void ToggleHangarMenus(CanvasGroup enable, CanvasGroup disable)
    {
        enable.alpha = 1f;
        enable.blocksRaycasts = true;

        disable.alpha = 0f;
        disable.blocksRaycasts = false;
    }

    void DesignMenus()
    {
        for (int i = 0; i < designHeader.childCount; i++)
            designMenu.GetChild(i).gameObject.SetActive(designHeader.GetChild(i).GetComponent<Toggle>().isOn);
    }

    public void SaveHangarData()
    {
        SaveSystem.SaveHangarData(ShuttleManager.Instance, ShuttleColorManager.Instance, ShuttleTailManager.Instance, Instance);
    }
    public void LoadHangarData()
    {
        HangarData data = SaveSystem.LoadHangarData();

        if (data != null)
        {
            for (int i = 0; i < ShuttleManager.Instance.shuttleItems.Length; i++)
            {
                ShuttleManager.Instance.shuttleItems[i].bought = data.boughtShuttleItems[i];
            }

            for (int i = 0; i < ShuttleColorManager.Instance.colorItems.Length; i++)
            {
                ShuttleColorManager.Instance.colorItems[i].bought = data.boughtColorItems[i];
            }

            for (int i = 0; i < ShuttleTailManager.Instance.trailItems.Length; i++)
            {
                ShuttleTailManager.Instance.trailItems[i].bought = data.boughtTailItems[i];
            }

            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].level = data.upgradesLevel[i];
            }
        }
    }
}
