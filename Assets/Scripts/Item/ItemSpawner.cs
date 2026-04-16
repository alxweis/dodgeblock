using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public bool isSingle;
    public float chanceToSpawn;
    public bool deactiveBoosts;

    // Chances To Spawn Items
    float j;
    const float chanceToSpawnScoreboost = 0.3f; // 0 < j < 0.3
    const float chanceToSpawnCoinboost = 0.3f; // 0.3 < j < 0.6
    const float chanceToSpawnSmallboost = 0.25f; // 0.6 < j < 0.85
    const float chanceToSpawnBigboost = 0.15f; // 0.8 < j < 0.95
    const float chanceToSpawnJitterboost = 0.05f; // 0.95 < j < 1
    Gradient gr;

    void OnEnable()
    {
        if (!isSingle)
        {
            ItemSpawn();
        }
        else
        {
            if(Random.value < chanceToSpawn)
            {
                ItemSpawn();
            }
        }
    }

    void ItemSpawn()
    {
        //int r = Random.Range(0, transform.childCount - deactivateLastItems);

        float random = Random.value;
        int index = 0;
        gr = ItemManager.Instance.chanceToSpawnItems;

        if (!deactiveBoosts)
        {
            if (random < gr.colorKeys[0].time)
                index = 0;
            else if (random > gr.colorKeys[0].time && random < gr.colorKeys[1].time)
                index = 1;
            else if (random > gr.colorKeys[1].time && random < gr.colorKeys[2].time)
                index = 2;
            else if (random > gr.colorKeys[2].time && random < gr.colorKeys[3].time)
                index = 3;
            //else if (random > gr.colorKeys[3].time && random < gr.colorKeys[4].time)
                //index = 4;
        }
        else
        {
            if (random < 0.5f)
                index = 0;
            else
                index = 1;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
