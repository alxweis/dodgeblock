using UnityEngine;

[System.Serializable]
public class ShuttleItem
{
    [HideInInspector]
    public bool selected, listenerAdded;

    public bool bought;

    public int price;
    public Sprite prim, sec;
}
