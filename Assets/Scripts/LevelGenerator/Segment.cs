using UnityEngine;

public class Segment : MonoBehaviour
{
    public int SegId { set; get; }
    public bool transition;
    public int length;
    public bool standAlone;

    DesignSpawner[] designSpawner;

    void Awake()
    {
        designSpawner = gameObject.GetComponentsInChildren<DesignSpawner>();

        for (int i = 0; i < designSpawner.Length; i++)
            foreach (SpriteRenderer sr in designSpawner[i].GetComponentsInChildren<SpriteRenderer>())
            {
                if(sr.tag == "Death")
                    sr.enabled = LevelManager.Instance.showCollider;
            }

        if (standAlone)
            Spawn();
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < designSpawner.Length; i++)
            designSpawner[i].Spawn();
    }

    public void Despawn()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < designSpawner.Length; i++)
            designSpawner[i].Despawn();
    }
}