using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxSpeed;
    float length, startPos, cameraStartPos;

    void Awake()
    {
        startPos = transform.position.y;
        cameraStartPos = Camera.main.transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        float temp = Camera.main.transform.position.y * (1 - parallaxSpeed);
        float dist = (Camera.main.transform.position.y - cameraStartPos) * parallaxSpeed;

        transform.position = new Vector2(transform.position.x, startPos + dist);

        if (temp > (startPos + length))
            startPos += length;
        else if (temp < (startPos - length))
            startPos -= length;
    }
}
