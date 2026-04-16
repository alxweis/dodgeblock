using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    public static AspectRatio Instance { set; get; }

    public float orthoSize = 5f;
    Transform leftPlane, rightPlane;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        float targetRatio = 1080f / 1920f;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float scale = targetRatio / screenRatio;

        Camera camera = GetComponent<Camera>();

        if (scale > 1f)
        {
            // Heightbox
            orthoSize = 5f * scale;
            Camera.main.orthographicSize = orthoSize;
        }
        else
        {
            // Widthbox
            Rect rect = camera.rect;

            rect.width = scale;
            rect.height = 1f;
            rect.x = (1f - scale) / 2f;
            rect.y = 0f;

            camera.rect = rect;
        }

        // if scaled height is less than current height, add letterbox
        /*
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
        */
    }

    void Update()
    {
        float xScreenBorder = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        leftPlane = transform.GetChild(0).GetChild(0);
        leftPlane.position = new Vector2(-xScreenBorder, leftPlane.position.y);
        rightPlane = transform.GetChild(0).GetChild(1);
        rightPlane.position = new Vector2(xScreenBorder, rightPlane.position.y);
    }
}
