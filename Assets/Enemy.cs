using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Transform")]
    public Transform player;
    public float yDeviation;
    public bool move;

    [Header("Sensor")]
    public SpriteRenderer sprite;
    float xSize, ySize;
    public float sensorLength;
    public float sensorAngle;
    
    bool avoiding;
    float avoidMultiplier;
    Vector2 pos;
    Vector2 clampedXPos;

    void Awake()
    {
        transform.rotation = Quaternion.identity;
        ySize = sprite.bounds.size.y;
        xSize = sprite.bounds.size.x;
    }

    void Update()
    {
        if (move)
        {
            pos = player.transform.position;
            pos.y = player.transform.position.y - yDeviation;

            transform.position = pos;

            if (avoiding)
            {
                clampedXPos = transform.position;
                //clampedXPos.x = 0f;

                transform.position = clampedXPos;
            }
        }
    }

    void FixedUpdate()
    {
        Sensors();
    }

    void Sensors()
    {
        RaycastHit2D hit;
        Vector2 sensorStartPos = transform.position;
        avoidMultiplier = 0f;
        avoiding = false;

        // Right Sensor
        sensorStartPos += (Vector2)(transform.right * xSize * 0.5f);
        sensorStartPos -= (Vector2)(transform.up * ySize * 0.5f);
        hit = Physics2D.Raycast(sensorStartPos, transform.up, sensorLength);
        if (hit)
        {
            if (hit.collider.CompareTag("Death"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }

        // Right Angle Sensor
        hit = Physics2D.Raycast(sensorStartPos, Quaternion.AngleAxis(-sensorAngle, transform.forward) * transform.up, sensorLength);
        if (hit)
        {
            if (hit.collider.CompareTag("Death"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        // Left Sensor
        sensorStartPos -= (Vector2)(transform.right * xSize);
        hit = Physics2D.Raycast(sensorStartPos, transform.up, sensorLength);
        if (hit)
        {
            if (hit.collider.CompareTag("Death"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }

        // Left Angle Sensor
        hit = Physics2D.Raycast(sensorStartPos, Quaternion.AngleAxis(sensorAngle, transform.forward) * transform.up, sensorLength);
        if (hit)
        {
            if (hit.collider.CompareTag("Death"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
            }
        }

        // Center Sensor
        sensorStartPos += (Vector2)(transform.up * ySize);
        sensorStartPos += (Vector2)(transform.right * xSize * 0.5f);
        if (avoidMultiplier == 0f)
        {
            hit = Physics2D.Raycast(sensorStartPos, transform.up, sensorLength);
            if (hit)
            {
                if (hit.collider.CompareTag("Death"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if (hit.normal.x < 0f)
                        avoidMultiplier = -1f;
                    else
                        avoidMultiplier = 1f;
                }
            }
        }
    }
}
