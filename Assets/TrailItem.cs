using UnityEngine;

[System.Serializable]
public class TrailItem
{
    [HideInInspector]
    public bool selected, listenerAdded;

    public bool bought;

    public int price;
    public AnimationCurve width;
    public Gradient gradient;
    public Sprite icon; 
}
