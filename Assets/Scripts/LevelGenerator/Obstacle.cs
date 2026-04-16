using UnityEngine;

public enum Type
{
    DoubleObstacle,
    TripleObstacle
};

public class Obstacle : MonoBehaviour
{
    Transform player;

    public Type type;

    public bool moving;
    public float smoothSpeed;
    public float maxAmplitude = 1.5f;
    bool move, startMoving;
    float xdir, startX;

    [Space(10)]
    public bool enableScoreAnim;

    [HideInInspector]
    [Range(0f, 1f)]
    public float distance;
    bool obstacleClear;

    void Start()
    {
        player = GameManager.Instance.player;
        startX = transform.position.x;

        startMoving = moving;
        if (moving && smoothSpeed == 0)
            smoothSpeed = Random.Range(1f, 2f);
    }

    void OnEnable()
    {
        if (Random.value <= LevelManager.Instance.chanceOfMoving)
            move = true;
        else
            move = false;
    }

    void Update()
    {
        if (move)
        {
            if (moving)
            {
                xdir = Mathf.Sin(Time.time * smoothSpeed - startX) * maxAmplitude;
                if (type == Type.DoubleObstacle)
                    transform.position = new Vector2(xdir, transform.position.y);
                else
                {
                    transform.GetChild(2).position = new Vector2(xdir, transform.GetChild(2).position.y);

                    #region Adaption
                    float xPos = 0.5f * xdir;
                    transform.GetChild(3).position = new Vector2(xPos - maxAmplitude, transform.GetChild(3).position.y);
                    transform.GetChild(4).position = new Vector2(xPos + maxAmplitude, transform.GetChild(4).position.y);

                    transform.GetChild(3).GetComponent<BoxCollider2D>().size = new Vector2(xdir + 1.15f, transform.GetChild(3).GetComponent<BoxCollider2D>().size.y);
                    transform.GetChild(4).GetComponent<BoxCollider2D>().size = new Vector2(-1.15f / maxAmplitude * xdir + 1.15f, transform.GetChild(4).GetComponent<BoxCollider2D>().size.y);

                    float a = 6.25f;
                    transform.GetChild(3).GetChild(0).localScale = new Vector2(a * xdir + 12.375f, transform.GetChild(3).GetChild(0).localScale.y);
                    transform.GetChild(4).GetChild(0).localScale = new Vector2(-a * xdir + 12.375f, transform.GetChild(3).GetChild(0).localScale.y);
                    #endregion
                }
            }
        }

        if (transform.position.y < Camera.main.ScreenToWorldPoint(Vector2.zero).y - 1f)
        {
            if (obstacleClear)
            {
                if (type == Type.DoubleObstacle)
                {
                    // Left Collider
                    transform.GetChild(0).localPosition = new Vector2(-3.15f, transform.GetChild(0).localPosition.y);
                    // Right Collider
                    transform.GetChild(1).localPosition = new Vector2(3.15f, transform.GetChild(1).localPosition.y);

                    // Score Collider
                    transform.GetChild(2).localPosition = new Vector2(0f, transform.GetChild(2).localPosition.y);
                    transform.GetChild(2).GetComponent<BoxCollider2D>().size = new Vector2(1.15f, transform.GetChild(2).GetComponent<BoxCollider2D>().size.y);
                    // Score Image
                    transform.GetChild(2).GetChild(0).GetComponent<Transform>().localScale = new Vector2(12.375f, transform.GetChild(2).GetChild(0).GetComponent<Transform>().localScale.y);
                }
                else
                {
                    // Left Collider
                    transform.GetChild(0).localPosition = new Vector2(-4.55f, transform.GetChild(0).localPosition.y);
                    // Right Collider
                    transform.GetChild(1).localPosition = new Vector2(4.55f, transform.GetChild(1).localPosition.y);

                    // Right Score Collider
                    transform.GetChild(4).gameObject.SetActive(true);

                    // Mid
                    transform.GetChild(2).gameObject.SetActive(true);
                }

                obstacleClear = false;
            }

            moving = startMoving;
            if (type == Type.DoubleObstacle)
            {
                transform.position = new Vector2(startX, transform.position.y);
            }
            else
            {
                transform.GetChild(2).position = new Vector2(0f, transform.GetChild(2).position.y);

                #region Adaption
                // Left Score Collider
                transform.GetChild(3).position = new Vector2(-maxAmplitude, transform.GetChild(3).position.y);
                transform.GetChild(3).GetComponent<BoxCollider2D>().size = new Vector2(1.15f, transform.GetChild(3).GetComponent<BoxCollider2D>().size.y);
                transform.GetChild(3).GetChild(0).localScale = new Vector2(12.375f, transform.GetChild(3).GetChild(0).localScale.y);
                // Right Score Collider
                transform.GetChild(4).position = new Vector2(maxAmplitude, transform.GetChild(4).position.y);
                transform.GetChild(4).GetComponent<BoxCollider2D>().size = new Vector2(1.15f, transform.GetChild(4).GetComponent<BoxCollider2D>().size.y);
                transform.GetChild(4).GetChild(0).localScale = new Vector2(12.375f, transform.GetChild(3).GetChild(0).localScale.y);
                #endregion
            }
            enableScoreAnim = true;
        }
    }

    public void ObstacleClear()
    {
        obstacleClear = true;

        // 4.65f -> 3.15f + 1.5f
        // 4.2f -> 4.65f / 1.15f
        // 1.4f -> 4.55f - 3.15f

        // Moving
        if (moving)
            moving = false;

        if (type == Type.DoubleObstacle)
        {
            // Left
            transform.GetChild(0).localPosition = new Vector2(-4.65f - transform.position.x, transform.GetChild(0).localPosition.y);
            // Right
            transform.GetChild(1).localPosition = new Vector2(4.65f - transform.position.x, transform.GetChild(1).localPosition.y);

            transform.GetChild(2).localPosition = new Vector2(-transform.position.x, transform.GetChild(2).localPosition.y);
            transform.GetChild(2).GetComponent<BoxCollider2D>().size = new Vector2(4.2f, transform.GetChild(2).GetComponent<BoxCollider2D>().size.y);
            transform.GetChild(2).GetChild(0).GetComponent<Transform>().localScale = new Vector2(31.1f, transform.GetChild(2).GetChild(0).GetComponent<Transform>().localScale.y);
        }
        else
        {
            // Left
            transform.GetChild(0).localPosition = new Vector2(-4.65f - transform.position.x, transform.GetChild(0).localPosition.y);
            // Right
            transform.GetChild(1).localPosition = new Vector2(4.65f - transform.position.x, transform.GetChild(1).localPosition.y);

            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(3).localPosition = new Vector2(-transform.position.x, transform.GetChild(3).localPosition.y);
            transform.GetChild(3).GetComponent<BoxCollider2D>().size = new Vector2(4.2f, transform.GetChild(3).GetComponent<BoxCollider2D>().size.y);
            transform.GetChild(3).GetChild(0).GetComponent<Transform>().localScale = new Vector2(31.1f, transform.GetChild(3).GetChild(0).GetComponent<Transform>().localScale.y);

            // Mid
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
