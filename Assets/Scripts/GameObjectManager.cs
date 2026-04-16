using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public bool obstacle = false;
    public bool moving = false;
    float deathZone;
    float startX;
    public float xdir;
    float smoothSpeed;

    void Start()
    {
        startX = transform.position.x;
        smoothSpeed = Random.Range(1f, 2.5f);

        if (gameObject.tag == "Obstacle")
        {
            obstacle = true;
        }

        if (obstacle)
        {
            if (Random.value > GameObjectSpawner.Instance.probabilityOfMovingOb)
            {
                moving = true;
            }
        }
    }

    void Update()
    {
        if (moving)
        {
            xdir = Mathf.Sin(Time.time * smoothSpeed - startX) * 1.4f;
            transform.position = new Vector2(xdir, transform.position.y);
        }

        deathZone = Player.Instance.transform.position.y - 10f;
        if (transform.position.y < deathZone)
        {
            Destroy(gameObject);
        }
    }
}
