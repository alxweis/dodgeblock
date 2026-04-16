using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { set; get; }

    public Gradient chanceToSpawnItems;

    [Header("Particle Systems")]
    public ParticleSystem scoreBoost;
    public ParticleSystem coinBoost;
    public ParticleSystem smallBoost;
    public ParticleSystem bigBoost;
    public ParticleSystem jitterBoost;
    [Header("UI")]
    public RectTransform carrier;
    public RectTransform itemTimer;
    public Gradient timerGradient;

    float xValue = -55f;
    float yStartValue = 100f;
    float yDistance = 180f;

    public Item[] items;
    public List<Item> currentlyDisplayedItems;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < HangarManager.Instance.upgrades.Count; i++)
        {
            Upgrade up = HangarManager.Instance.upgrades[i];
            Item(up.upgradeName).duration = up.levelValues[up.level];
        }
    }

    public Item Item(string name)
    {
        Item i = Array.Find(items, item => item.name == name);
        if (i == null)
            Debug.LogWarning("Item: " + name + " not found!");
        return i;
    }

    void Update()
    {
        foreach (var item in items)
        {
            Vector2 pos = new Vector2(xValue, yStartValue - yDistance * currentlyDisplayedItems.IndexOf(item));

            if (item.active)
            {
                if (GameManager.Instance.isGameStarted && !GameManager.Instance.isPaused)
                {
                    if (!item.displayed)
                    {
                        item.timer = item.duration;
                        RectTransform itemDisplay = Instantiate(itemTimer, pos, Quaternion.identity);
                        itemDisplay.SetParent(carrier.transform, false);
                        item.itemDisplay = itemDisplay;
                        RectTransform itemTimerIcon = Instantiate(item.timerPrefab, item.timerPrefab.position, Quaternion.identity);
                        itemTimerIcon.SetParent(itemDisplay.GetChild(0).GetChild(1), false);

                        StartCoroutine(Animation(item, true, 0.1f));
                        currentlyDisplayedItems.Insert(0, item);
                        item.displayed = true;
                    }
                    else
                    {
                        item.timer -= Time.deltaTime;

                        float t = (item.duration - item.timer) / item.duration;
                        item.itemDisplay.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = Mathf.Lerp(1f, 0f, t);
                        float value = Mathf.Lerp(0f, 1f, t);
                        item.itemDisplay.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().color = timerGradient.Evaluate(value);
                        //item.itemDisplay.GetChild(1).GetComponent<Text>().text = item.timer.ToString("0");
                        if (item.name == "Shield")
                        {
                            item.itemDisplay.GetChild(0).GetChild(2).GetComponent<Text>().enabled = true;
                            if (ScoreManager.Instance.shields == 0)
                                item.itemDisplay.GetChild(0).GetChild(2).GetComponent<Text>().color = new Color(1f, 0.44f, 0.44f);
                            item.itemDisplay.GetChild(0).GetChild(2).GetComponent<Text>().text = ScoreManager.Instance.shields.ToString("0");
                        }

                        if (item.collected)
                        {
                            currentlyDisplayedItems.Remove(item);
                            currentlyDisplayedItems.Insert(0, item);
                            item.collected = false;
                        }
                        item.itemDisplay.anchoredPosition = pos;
                    }
                    
                }
                
                if(item.timer < 0)
                    item.active = false;
            }
            else
            {
                if (item.displayed)
                {
                    if (item.name != "Shield")
                        StartCoroutine(Animation(item, false, 0.1f));
                    else
                        StartCoroutine(ShieldPulse(item, 4f, 2f));

                    item.itemDisplay.anchoredPosition = pos;

                    //currentlyDisplayedItems.Remove(item);
                    //item.displayed = false;
                }
            }
        }
    }

    IEnumerator Animation(Item item, bool b, float duration)
    {
        float counter = 0f;
        RectTransform r = item.itemDisplay;

        while (counter < duration && r)
        {
            if (GameManager.Instance.isGameStarted && !GameManager.Instance.isPaused)
            {
                counter += Time.deltaTime;
                float t = counter / duration;

                if (b)
                {
                    r.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, t);
                    r.localScale = Vector2.Lerp(new Vector2(0.8f, 0.8f), Vector2.one, t);
                }
                else
                {
                    r.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, t);
                    r.localScale = Vector2.Lerp(Vector2.one, new Vector2(0.8f, 0.8f), t);
                }
            }
            yield return null;
        }

        if (!b && counter >= duration && r)
        {
            Destroy(r.gameObject);
            currentlyDisplayedItems.Remove(item);
            item.displayed = false;
        }
    }

    IEnumerator ShieldPulse(Item item, float duration, float repeatRate)
    {
        float counter = 0f;
        RectTransform r = item.itemDisplay;

        while (counter < duration && r)
        {
            if (GameManager.Instance.isGameStarted && !GameManager.Instance.isPaused)
            {
                counter += Time.deltaTime;
                r.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = 0f;
                r.GetChild(0).GetChild(1).GetComponent<CanvasGroup>().alpha = Mathf.Cos(repeatRate * Mathf.PI * counter) * 0.5f + 0.5f;
            }
            yield return null;
        }

        if (counter >= duration && r)
        {
            StartCoroutine(Animation(item, false, 0.1f));
        }
    }
}
