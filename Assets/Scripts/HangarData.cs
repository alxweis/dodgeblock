[System.Serializable]
public class HangarData
{
    public bool[] boughtShuttleItems;
    public bool[] boughtColorItems;
    public bool[] boughtTailItems;
    public int[] upgradesLevel;

    public HangarData(ShuttleManager shuttleManager, ShuttleColorManager colorManager, ShuttleTailManager tailManager, HangarManager hangarManager)
    {
        boughtShuttleItems = new bool[shuttleManager.shuttleItems.Length];
        for (int i = 0; i < shuttleManager.shuttleItems.Length; i++)
        {
            boughtShuttleItems[i] = shuttleManager.shuttleItems[i].bought;
        }

        boughtColorItems = new bool[colorManager.colorItems.Length];
        for (int i = 0; i < colorManager.colorItems.Length; i++)
        {
            boughtColorItems[i] = colorManager.colorItems[i].bought;
        }

        boughtTailItems = new bool[tailManager.trailItems.Length];
        for (int i = 0; i < tailManager.trailItems.Length; i++)
        {
            boughtTailItems[i] = tailManager.trailItems[i].bought;
        }

        upgradesLevel = new int[hangarManager.upgrades.Count];
        for (int i = 0; i < hangarManager.upgrades.Count; i++)
        {
            upgradesLevel[i] = hangarManager.upgrades[i].level;
        }
    }
}
