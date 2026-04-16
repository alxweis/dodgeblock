using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float smoothSpeed = 0.1f;

    public GameObject dodge, block, the;

    Quaternion targetRotation;
    Vector2 targetPosition;

    public float RotZ = 2.5f;
    public float PosX = 0.1f;
    public float PosY = 0.1f;

    public float randomRotZ, randomPosX, randomPosY;

    void Start()
    {
        InvokeRepeating("CalcValues", 0.5f, 1f);
    }

    void CalcValues()
    {
        randomRotZ = Random.Range(-RotZ, RotZ);
        randomPosX = Random.Range(-PosX, PosX);
        randomPosY = Random.Range(-PosY, PosY);
    }
    void Update()
    {
        targetPosition = new Vector2(transform.position.x + randomPosX, transform.position.y + randomPosY);
        transform.position = Vector2.Lerp(transform.position, targetPosition, smoothSpeed * Time.smoothDeltaTime);

        targetRotation = Quaternion.Euler(0, 0, randomRotZ);
        dodge.transform.rotation = Quaternion.Slerp(dodge.transform.rotation, targetRotation, smoothSpeed * Time.smoothDeltaTime);
        block.transform.rotation = Quaternion.Slerp(block.transform.rotation, targetRotation, smoothSpeed * Time.smoothDeltaTime);
        the.transform.rotation = Quaternion.Slerp(the.transform.rotation, targetRotation, smoothSpeed * Time.smoothDeltaTime);
    }
}
