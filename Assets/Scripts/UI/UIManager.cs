using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { set; get; }

    public RectTransform carrier;
    public PopUp[] popUp;
    public RectTransform notification;
    public RectTransform infoPopUp;
    public RectTransform shieldPopUp;
    public RectTransform settingsPopUp;
    public RectTransform designPopUp;
    public RectTransform leavePopUp;

    const int shieldPrice = 300;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Set Settings
        Player.Instance.dragSpeed = 0.08f * PlayerPrefs.GetFloat("Sensivity") + 1f;
    }

    public void PopUp(string name)
    {
        PopUp p = Array.Find(popUp, popUp => popUp.name == name);
        if (p == null)
            Debug.LogWarning("Pop Up: " + name + " not found!");

        RectTransform clone = Instantiate(p.popUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("Open");

        if(name == "Shield")
        {
            if (ScoreManager.Instance.coins < 300)
                clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = false;
            else
            {
                clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OnBuyShieldButton(clone); });
                clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
            }
            clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = "You have " + ScoreManager.Instance.shields.ToString("0") + " Shields!";
            clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        }
        // Ok Button PopUps
        else if(name == "Failed")
        {
            clone.GetChild(1).GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
            clone.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        }
    }

    public void OnPopUpClose(RectTransform r, float time)
    {
        r.GetComponent<Animator>().SetTrigger("PopUpClose");
        Destroy(r.gameObject, time);
    }

    #region Info Pop Up
    public void InfoPopUp(string headline, string content)
    {
        RectTransform clone = Instantiate(infoPopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("Open");

        clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        clone.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = headline;
        clone.GetChild(1).GetChild(0).GetChild(2).GetComponent<Text>().text = content;
        clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
    }
    #endregion

    #region Welcome
    public void WelcomePopUp(string headline, string content)
    {
        RectTransform clone = Instantiate(infoPopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("Open");

        clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        clone.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = headline;
        clone.GetChild(1).GetChild(0).GetChild(2).GetComponent<Text>().text = content;
        clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OnWelcomeGetIt();  OnPopUpClose(clone, 0.2f); });
    }
    void OnWelcomeGetIt()
    {
        ScoreManager.Instance.coins += GameManager.Instance.startCoins;
        ScoreManager.Instance.shields += GameManager.Instance.startShields;
        InfoPopUp("SHIELDS", "Double tap while playing to activate an obstacle defense shield!");
    }
    #endregion

    #region Shield
    public void ShieldPopUp()
    {
        RectTransform clone = Instantiate(shieldPopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("PopUp");

        clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        //clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ShieldPopUpUpdate(clone); });
        clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { OnBuyShieldButton(clone); });

        ShieldPopUpUpdate(clone);
    }

    void ShieldPopUpUpdate(RectTransform r)
    {
        r.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = ScoreManager.Instance.shields.ToString("0");
        r.GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = shieldPrice.ToString("0");

        if (ScoreManager.Instance.coins < shieldPrice)
        {
            // Black-White Shader
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = false;
        }
        else
        {
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
        }
    }

    public void OnBuyShieldButton(RectTransform r)
    {
        ScoreManager.Instance.shields++;
        ScoreManager.Instance.coins = ScoreManager.Instance.coins - shieldPrice;

        ShieldPopUpUpdate(r);
    }
    #endregion

    #region Settings
    public void SettingsPopUp()
    {
        RectTransform clone = Instantiate(settingsPopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("PopUp");

        clone.GetChild(1).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        //clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ShieldPopUpUpdate(clone); });
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnMusicButton(clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>()); });
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnSoundeffectsButton(clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetComponent<Toggle>()); });
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate { OnVibrationButton(clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetComponent<Toggle>()); });

        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetComponent<Slider>().onValueChanged.AddListener(delegate { OnSensivitySlider(clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetComponent<Slider>()); });
        if(!AdvertismentManager.Instance.noAds)
            clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ShopManager.Instance.OnRemoveAdButton(); OnPopUpClose(clone, 0.2f); });
        else
        {
            clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponent<Button>().interactable = false;
            clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
        }


        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("Music", 1) == 1 ? true : false;
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("Soundeffects", 1) == 1 ? true : false;
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("Vibration", 0) == 1 ? true : false;
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensivity", 25f);
        //SettingsPopUpUpdate(clone);
    }

    void SettingsPopUpUpdate(RectTransform r)
    {

    }

    void OnMusicButton(Toggle t)
    {
        if (t.isOn)
        {
            ToggleOn(t);
            AudioManager.Instance.ContinueMusic();
        }
        else
        {
            ToggleOff(t);
            AudioManager.Instance.StopMusic();
        }

        PlayerPrefs.SetInt("Music", t.isOn == true ? 1 : 0);
    }
    void OnSoundeffectsButton(Toggle t)
    {
        if (t.isOn)
        {
            ToggleOn(t);
            AudioManager.Instance.UnStopSoundeffects();
        }
        else
        {
            ToggleOff(t);
            AudioManager.Instance.StopSoundeffects();
        }

        PlayerPrefs.SetInt("Soundeffects", t.isOn == true ? 1 : 0);
    }
    void OnVibrationButton(Toggle t)
    {
        if (t.isOn)
        {
            ToggleOn(t);
        }
        else
        {
            ToggleOff(t);
        }

        PlayerPrefs.SetInt("Vibration", t.isOn == true ? 1 : 0);
    }

    void ToggleOff(Toggle t)
    {
        t.transform.GetChild(1).GetComponent<Image>().color = new Color(209f/255f, 114f/255f, 130f/255f, 1f); // Red
        t.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "OFF";
    }
    void ToggleOn(Toggle t)
    {
        t.transform.GetChild(1).GetComponent<Image>().color = new Color(114f / 255f, 209f / 255f, 124f / 255f, 1f); // Green
        t.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "ON";
    }

    void OnSensivitySlider(Slider s)
    {
        Player.Instance.dragSpeed = 0.08f * s.value + 1f;
        PlayerPrefs.SetFloat("Sensivity", s.value);
    }
    #endregion

    #region Leave
    public void LeavePopUp()
    {
        RectTransform clone = Instantiate(leavePopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("PopUp");

        clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });

        clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.OnLeave(0f); OnPopUpClose(clone, 0.2f); });
        clone.GetChild(1).GetChild(0).GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { OnPopUpClose(clone, 0.2f); });
    }
    #endregion

    #region ShuttleDesign
    public void DesignPopUp(int i)
    {
        RectTransform clone = Instantiate(designPopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("PopUp");

        clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = ShuttleManager.Instance.shuttleItems[i].prim;
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = ShuttleManager.Instance.shuttleItems[i].sec;
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.gray;
        clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { OnDesignBuyButton(clone, i); OnPopUpClose(clone, 0.2f); });

        DesignPopUpUpdate(clone, ShuttleManager.Instance.shuttleItems[i].price);
    }

    void DesignPopUpUpdate(RectTransform r, int price)
    {
        r.GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = price.ToString("0");

        if (ScoreManager.Instance.coins < price)
        {
            // Black-White Shader
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = false;
        }
        else
        {
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
        }
    }

    public void OnDesignBuyButton(RectTransform r, int i)
    {
        AudioManager.Instance.Play("Buy");
        ShuttleManager.Instance.shuttleItems[i].bought = true;
        ShuttleManager.Instance.shuttleContent.GetChild(i).GetComponent<Toggle>().isOn = true;

        ScoreManager.Instance.coins = ScoreManager.Instance.coins - ShuttleManager.Instance.shuttleItems[i].price;

        DesignPopUpUpdate(r, ShuttleManager.Instance.shuttleItems[i].price);
        HangarManager.Instance.SaveHangarData();
    }
    #endregion

    #region ColorDesign
    public void ColorDesignPopUp(int i, bool isPrim)
    {
        RectTransform clone = Instantiate(designPopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("PopUp");

        clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        clone.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = "COLOR";
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().color = Color.clear;
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = ShuttleColorManager.Instance.colorItems[i].color;
        clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { OnColorDesignBuyButton(clone, i, isPrim); OnPopUpClose(clone, 0.2f); });

        ColorDesignPopUpUpdate(clone, ShuttleColorManager.Instance.colorItems[i].price);
    }

    void ColorDesignPopUpUpdate(RectTransform r, int price)
    {
        r.GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = price.ToString("0");

        if (ScoreManager.Instance.coins < price)
        {
            // Black-White Shader
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = false;
        }
        else
        {
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
        }
    }

    public void OnColorDesignBuyButton(RectTransform r, int i, bool isPrim)
    {
        AudioManager.Instance.Play("Buy");
        ShuttleColorManager.Instance.colorItems[i].bought = true;
        if (isPrim)
            ShuttleColorManager.Instance.colorPrimContent.GetChild(i).GetComponent<Toggle>().isOn = true;
        else
            ShuttleColorManager.Instance.colorSecContent.GetChild(i).GetComponent<Toggle>().isOn = true;

        ScoreManager.Instance.coins = ScoreManager.Instance.coins - ShuttleColorManager.Instance.colorItems[i].price;

        ColorDesignPopUpUpdate(r, ShuttleColorManager.Instance.colorItems[i].price);
        HangarManager.Instance.SaveHangarData();
    }
    #endregion

    #region TrailDesign
    public void TrailPopUp(int i)
    {
        RectTransform clone = Instantiate(designPopUp, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("PopUp");

        clone.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OnPopUpClose(clone, 0.2f); });
        clone.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = "TAIL";
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(270, 270);
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().color = Color.clear;
        clone.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = ShuttleTailManager.Instance.trailItems[i].icon;
        clone.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { OnTrailDesignBuyButton(clone, i); OnPopUpClose(clone, 0.2f); });

        TrailDesignPopUpUpdate(clone, ShuttleTailManager.Instance.trailItems[i].price);
    }

    void TrailDesignPopUpUpdate(RectTransform r, int price)
    {
        r.GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = price.ToString("0");

        if (ScoreManager.Instance.coins < price)
        {
            // Black-White Shader
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = false;
        }
        else
        {
            r.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
        }
    }

    public void OnTrailDesignBuyButton(RectTransform r, int i)
    {
        AudioManager.Instance.Play("Buy");
        ShuttleTailManager.Instance.trailItems[i].bought = true;
        ShuttleTailManager.Instance.trailContent.GetChild(i).GetComponent<Toggle>().isOn = true;

        ScoreManager.Instance.coins = ScoreManager.Instance.coins - ShuttleTailManager.Instance.trailItems[i].price;

        TrailDesignPopUpUpdate(r, ShuttleTailManager.Instance.trailItems[i].price);
        HangarManager.Instance.SaveHangarData();
    }
    #endregion

    public void Notification()
    {
        var clone = Instantiate(notification, carrier, false);
        clone.GetComponent<Animator>().SetTrigger("PopUp");
        Destroy(clone.gameObject, 1f);
    }
}
