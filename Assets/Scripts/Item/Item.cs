using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public bool active;
    public float duration;
    public RectTransform timerPrefab;

    [HideInInspector]
    public float timer;
    [HideInInspector]
    public bool displayed;
    [HideInInspector]
    public bool collected;
    [HideInInspector]
    public RectTransform itemDisplay;
}
