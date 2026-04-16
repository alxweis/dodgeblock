using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    Rigidbody2D rb;
    float deltaX;
    float speed = 10f;
    float mapWidth = 2.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    break;

                case TouchPhase.Moved:
                    float x = touch.deltaPosition.x * Time.fixedDeltaTime * speed;
                    Vector2 newPosition = rb.position + Vector2.right * x;
                    newPosition.x = Mathf.Clamp(touchPos.x - deltaX, -mapWidth, mapWidth);
                    rb.MovePosition(newPosition);
                    break;

                case TouchPhase.Ended:
                    rb.linearVelocity = Vector2.zero;
                    break;
            }
        }
    }
}
