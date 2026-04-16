using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public bool flip;
    public bool[] availableCoins;

    [Space(10)]
    public float chanceToSpawnCoins = 0.5f;
    public bool spawnAllSelectedCoins;

    GameObject[] childs;
    GameObject[] coins;
    Vector2[] originCoinPos;
    List<GameObject> activeCoins = new List<GameObject>();

    void Awake()
    {
        childs = new GameObject[transform.childCount];
        coins = new GameObject[transform.childCount - 1];
        originCoinPos = new Vector2[transform.childCount - 1];
        for (int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            coins[i] = transform.GetChild(i).gameObject;
            originCoinPos[i] = transform.GetChild(i).localPosition;
        }

        OnDisable();
    }

    void OnEnable()
    {
        if (spawnAllSelectedCoins)
        {
            for (int i = 0; i < availableCoins.Length; i++)
            {
                if (availableCoins[i])
                {
                    coins[i].SetActive(true);
                    coins[i].transform.localPosition = originCoinPos[i];
                    transform.GetChild(i).GetChild(0).rotation = Quaternion.Euler(0f, CoinManager.Instance.angleDeviation * i, 0f);
                }
            }
        }
        else
        {
            if (Random.value <= chanceToSpawnCoins)
            {
                for (int i = 0; i < availableCoins.Length; i++)
                {
                    if (availableCoins[i])
                    {
                        activeCoins.Add(coins[i]);
                        coins[i].transform.localPosition = originCoinPos[i];
                        transform.GetChild(i).GetChild(0).rotation = Quaternion.Euler(0f, CoinManager.Instance.angleDeviation * i, 0f);
                    }
                }
                int r = Random.Range(2, activeCoins.Count);
                for (int i = 0; i < r; i++)
                {
                    activeCoins[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < availableCoins.Length; i++)
                {
                    coins[i].SetActive(false);
                }
                transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
            }
        }
    }

    void OnDisable()
    {
        foreach (GameObject go in childs)
            go.SetActive(false);
    }

    void OnValidate()
    {
        Flip();
    }
    void Update()
    {
        Flip();
    }

    void Flip()
    {
        float flippedX;
        if (flip)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform tr = transform.GetChild(i);
                if (tr.localPosition.x < 0f)
                    flippedX = tr.localPosition.x;
                else
                    flippedX = -tr.localPosition.x;
                tr.localPosition = new Vector2(flippedX, tr.localPosition.y);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform tr = transform.GetChild(i);
                if(tr.localPosition.x < 0f)
                    flippedX = -tr.localPosition.x;
                else
                    flippedX = tr.localPosition.x;
                tr.localPosition = new Vector2(flippedX, tr.localPosition.y);
            }
        }
    }
}
