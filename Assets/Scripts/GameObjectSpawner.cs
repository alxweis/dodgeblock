using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    public static GameObjectSpawner Instance { set; get; }

    public Transform player;
    public GameObject[] obstacles;
    public GameObject[] coins;

    public GameObject selectedBarrierSkin;

    Vector2 spawnPoint;

    Vector2 coinSpawnPoint;

    public float heightToSpawnObjects = 8f;

    [Header("Time Between Waves")]
    public float waveFrequency; // current time between an obstacle spawn
    public float waveFrequencyStart = 1.2f; // start time between an obstacle spawn  
    public float waveFrequencyEnd = 0.65f; // end time between an obstalce spawn
    public float timeToLerp = 90f; // time to lerp from waveFrequencyStart to waveFrequencyEnd (Lvl.1: 90s; Lvl.2: 5s)
    public float timeIntervall = 20f; // How long the current waveFrequency will be if waveFrequency == waveFrequencyEnd (Lvl.1: 20s; Lvl.2: 5s-15s)

    [Header("Random Time Between Waves")]
    public float randomWFStart = 0.4f;
    public float randomWFEnd = 1.3f;

    [Header("Random Speed")]
    public float randomSpeedStart = 5f;
    public float randomSpeedEnd = 9f;

    float timeToSpawn = 1f;
    float timeStamp = 0;
    bool doTimeStamp;

    [Header("Player Speed")]
    public float speed;
    public float speedStart = 5f;
    public float speedEnd = 7f;

    [HideInInspector]
    public float t = 0f;
    [HideInInspector]
    public bool nxtLvl = false;
    [HideInInspector]
    public float probabilityOfMovingOb = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {

        if(GameManager.Instance.isGameStarted == true)
        {
            t += Time.deltaTime / timeToLerp;

            if (waveFrequency != waveFrequencyEnd)
            {
                waveFrequency = Mathf.Lerp(waveFrequencyStart, waveFrequencyEnd, t - timeStamp);
                speed = Mathf.Lerp(speedStart, speedEnd, t - timeStamp);
            }
            else if (waveFrequency == waveFrequencyEnd)
            {
                timeIntervall -= Time.deltaTime;
                if (timeIntervall <= 0f)
                {
                    doTimeStamp = true;
                    if (doTimeStamp)
                    {
                        timeStamp = t;
                        doTimeStamp = false;
                    }
                    timeToLerp = 8f;
                    waveFrequencyStart = waveFrequencyEnd;
                    waveFrequencyEnd = Random.Range(randomWFEnd, randomWFStart);
                    speedStart = speedEnd;
                    speedEnd = Random.Range(randomSpeedStart, randomSpeedEnd);
                    timeIntervall = Random.Range(5f, 15f);
                    nxtLvl = true;
                }
            }

            if (Time.time >= timeToSpawn)
            {
                //SpawnObstacles();
                // SpawnCoins();
                timeToSpawn = Time.time + waveFrequency;
            }
            if (nxtLvl)
            {
                probabilityOfMovingOb = Mathf.Lerp(probabilityOfMovingOb, 0.2f, Time.deltaTime / 60f);
            }
        }
    }

    void SpawnObstacles()
    {
        int randomIndex = Random.Range(0, obstacles.Length);
        spawnPoint = new Vector2(obstacles[randomIndex].transform.position.x, player.position.y + heightToSpawnObjects);

        Instantiate(obstacles[randomIndex], spawnPoint, Quaternion.identity);
    }
    void SpawnCoins()
    {
        int randomIndex = Random.Range(0, coins.Length);
        coinSpawnPoint = new Vector2(coins[randomIndex].transform.position.x, player.position.y + heightToSpawnObjects + 1f);

        Instantiate(coins[randomIndex], coinSpawnPoint, Quaternion.identity);
    }
}
