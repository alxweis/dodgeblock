using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { set; get; }

    public ParticleSystem coinParticleSystem;
    public float rotSpeed;
    public float angleDeviation = 20f;

    void Awake()
    {
        Instance = this;
    }
}
