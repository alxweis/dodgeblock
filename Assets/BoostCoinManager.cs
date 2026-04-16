using System.Collections.Generic;
using UnityEngine;

public class BoostCoinManager : MonoBehaviour
{
    public static BoostCoinManager Instance { set; get; }

    public CoinSpawner coinLine;
    public ItemSpawner itemSpawner;
    float space = 0.75f;

    float x, y;
    [HideInInspector]
    public float lineLength = 4f;
    bool spawnCoinLine;
    List<float> xPos;
    int lastIndex;

    void Awake()
    {
        Instance = this;

        xPos = new List<float>();

        xPos.Insert(0, -2f * space);
        xPos.Insert(1, -space);
        xPos.Insert(2, 0f);
        xPos.Insert(3, space);
        xPos.Insert(4, 2f * space);

        for (int i = 0; i < coinLine.availableCoins.Length; i++)
            coinLine.availableCoins[i] = true;
        coinLine.spawnAllSelectedCoins = true;

        itemSpawner.deactiveBoosts = true;
    }

    void Update()
    {
        if (spawnCoinLine)
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).position.y < (Camera.main.ScreenToWorldPoint(Vector2.zero).y - lineLength))
                    Destroy(transform.GetChild(i).gameObject);

        if (transform.childCount == 0)
            spawnCoinLine = false;

    }

    public void SpawnRandomCoinLine(int amount, int startSpace, float yPos)
    {
        spawnCoinLine = true;

        for (int i = 0; i < amount; i++)
        {
            if (i == 0)
            {
                lastIndex = Random.Range(0, xPos.Count - 1);
            }
            else
            {
                int start, end;
                start = (lastIndex - 1 <= 0) ? 0 : lastIndex - 1;
                end = (lastIndex + 1 >= xPos.Count - 1) ? xPos.Count : lastIndex + 2;
                lastIndex = Random.Range(start, end);
            }

            x = xPos[lastIndex];
            y = lineLength * i + startSpace + yPos;

            var clone = Instantiate(coinLine, new Vector2(x, y), Quaternion.identity);
            clone.transform.SetParent(transform, true);
        }
    }

    public void SpawnLinearCoinLine(int amount, int startSpace, float yPos)
    {
        spawnCoinLine = true;

        for (int _x = 0; _x < 3; _x++)
        {
            for (int _y = 0; _y < amount; _y++)
            {
                x = xPos[2 * _x];
                y = lineLength * _y + startSpace + yPos;

                GameObject clone;
                if (_y < amount - 1)
                {
                    clone = Instantiate(coinLine.gameObject, new Vector2(x, y), Quaternion.identity);
                }
                else
                {
                    y += 7;
                    clone = Instantiate(itemSpawner.gameObject, new Vector2(x, y), Quaternion.identity);
                }
                clone.transform.SetParent(transform, true);
            }
        }
    }
}
