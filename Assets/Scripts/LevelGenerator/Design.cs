using UnityEngine;

public enum DesignType
{
    none = -1,
    obstacle = 0,
    tripleObstacle = 1,
}

public class Design : MonoBehaviour
{
    public DesignType type;
    public int visualIndex;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (ColorLerper.Instance)
        {
            sr.color = ColorLerper.Instance.dodge.color;
            for (int i = 0; i < transform.childCount; i++)
            {
                SpriteRenderer sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (sr)
                    sr.color = ColorLerper.Instance.dodgeBackColor;
            }
        }
    }
}
