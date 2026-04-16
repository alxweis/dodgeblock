using System.Collections.Generic;
using UnityEngine;

public class RedLine : MonoBehaviour
{
    LineRenderer lr;
    EdgeCollider2D col;

    [Range(0f, 1f)]
    public float load;
    int coinReward;
    const int MaxCoinReward = 100;

    public List<Vector2> pos = new List<Vector2>();
    float t, length;
    bool triggered;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();
        length = pos[pos.Count - 1].y;
    }

    void Update()
    {
        if (load > 1f)
            load = 1f;

        if (triggered)
        {
            t += Time.deltaTime;
            load = (Player.Instance.speed * t) / length;
        }
        else
        {
            if (t > 0.8f)
            {
                coinReward = (int)(load * MaxCoinReward);
                Debug.Log(coinReward);
                GameManager.Instance.CollectCoins(coinReward);
            }
            coinReward = 0;
            t = load = 0f;
        }
    }

    void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();

        CheckPos();
    }

    void CheckPos()
    {
        col.points = pos.ToArray();

        for (int i = 0; i < pos.Count; i++)
        {
            if(i == lr.positionCount)
            {
                lr.positionCount++;
            }
            lr.SetPosition(i, pos[i]);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            if(!triggered)
                triggered = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(triggered)
            triggered = false;
    }
}
