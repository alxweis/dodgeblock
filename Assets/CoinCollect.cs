using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinCollect : MonoBehaviour
{
    public RectTransform target;
    public RectTransform prefab;
    public bool collect;
    int coinAmount = 5;

    float radius = 5f;
    float power = 50f;
    float t;

    void Update()
    {
        if (collect)
        {
            for (int i = 0; i < coinAmount; i++)
            {
                var clone = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
                clone.SetParent(transform, false);
            }
            collect = false;
        }

        if(transform.childCount == coinAmount)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                float duration = Random.Range(1f, 3f);
                t += Time.deltaTime / duration;
                transform.GetChild(i).position = Vector2.Lerp(transform.GetChild(i).position, target.position, t);

                if (transform.GetChild(i).position == target.position)
                    Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
