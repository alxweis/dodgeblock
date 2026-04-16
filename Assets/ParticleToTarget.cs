using System.Collections.Generic;
using UnityEngine;

public class ParticleToTarget : MonoBehaviour
{
    public static ParticleToTarget Instance { set; get; }

    public Transform Target;

    ParticleSystem ps;
    static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
    int count;

    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    public int lastCoins;
    public bool collected;
    public bool played;
    public float fCoins;
    public int coinReward;
    float debugTimer = 2f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = ps.transform.TransformPoint(particle.position);
            Vector3 v2 = Target.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime); 
            particle.position = ps.transform.InverseTransformPoint(v2 - tarPosi);
            particles[i] = particle;
        }

        ps.SetParticles(particles, count);

        if (ps.particleCount > 0)
            played = true;

        if (played && ps.particleCount == 0)
        {
            collected = true;
            if(GameManager.Instance.scoreCoins)
                coinReward = 0;
            played = false;
        }
    }

    void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        float adder = GameManager.Instance.adder;
        for (int i = 0; i < numEnter; i++)
        {
            fCoins += adder;
            ScoreManager.Instance.coins = (int)fCoins;
            if (GameManager.Instance.scoreCoins)
            {
                if (coinReward > 0)
                    coinReward -= (int)adder;
                else
                    coinReward = 0;
            }
        }
        ScoreManager.Instance.coins = (int)fCoins;

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
