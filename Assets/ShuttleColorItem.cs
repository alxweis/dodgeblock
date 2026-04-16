using UnityEngine;

[System.Serializable]
public class ShuttleColorItem
{
    [HideInInspector]
    public bool selected, primListenerAdded, secListenerAdded;
    public bool bought;

    public int price;
    public Color color;
}
