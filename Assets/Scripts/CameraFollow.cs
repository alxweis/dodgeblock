using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { set; get; }

    public Transform target;
    public Vector3 offset;
    public float yDeviation;

    [Header("DeathMovement")]
    public float yPos = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!GameManager.Instance.isDead)
        {
            Vector3 desiredPos = new Vector3(offset.x, target.position.y + offset.y + yDeviation, offset.z);
            transform.position = desiredPos;
        }
        else
        {
            float smoothedSpeed = Player.Instance.speed / 200f;

            Vector3 desiredPos = new Vector3(offset.x, target.position.y + offset.y + yPos, offset.z);
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothedSpeed);
            transform.position = smoothedPos;
        }
    }

}
