using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }
    public bool showCollider = true;
    public bool deactivateSegments;
    float deactivateRange;

    [HideInInspector]
    public float chanceOfMoving;

    // Segment Spawning
    const float DistanceBeforeSpawn = 15f;
    const int InitialSegments = 1;
    const int MaxSegmentsOnScreen = 5;
    Transform cameraContainer;
    int amountOfActiveSegments;
    int continiousSegments;
    int currentSpawnY;
    float yScreenBottom;

    [Header ("List of Designs")]
    public List<Design> obstacles = new List<Design>();
    public List<Design> tripleMidObs = new List<Design>();
    [HideInInspector]
    public List<Design> designs = new List<Design>();

    [Header("List of Segments")]
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    [HideInInspector]
    public List<Segment> segments = new List<Segment>();

    // Gameplay
    bool isMoving;

    void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
    }

    void Start()
    {
        for (int i = 0; i < InitialSegments; i++)
            GenerateSegment();
    }

    void Update()
    {
        yScreenBottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;

        if ((currentSpawnY - cameraContainer.position.y) < DistanceBeforeSpawn)
            GenerateSegment();

        if (amountOfActiveSegments >= MaxSegmentsOnScreen)
            DespawnSegment();

        for (int i = 0; i < amountOfActiveSegments; i++)
            if ((segments[i].transform.position.y + segments[i].length) < yScreenBottom)
                DespawnSegment();

        if (GameManager.Instance.isGameStarted)
            if (chanceOfMoving <= 1f)
                chanceOfMoving += Time.deltaTime / Player.Instance.timeToLerp;

        deactivateSegments = (ItemManager.Instance.Item("Bigboost").active || ItemManager.Instance.Item("Smallboost").active || ItemManager.Instance.Item("Jitterboost").active) ? true : false;
    }

    void GenerateSegment()
    {
        SpawnSegment();

        if(Random.value < (continiousSegments * 0.25f) && !deactivateSegments)
        {
            continiousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continiousSegments++;
        }
    }

    void SpawnSegment()
    {
        int id = Random.Range(0, availableSegments.Count);

        Segment s = GetSegment(id, false);

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.up * currentSpawnY;

        currentSpawnY += s.length;
        amountOfActiveSegments++;

        if (deactivateSegments)
        {
            foreach (Transform child in s.transform)
            {
                if (child.position.y < deactivateRange)
                {
                    if (child.GetComponent<Obstacle>() != null)
                        child.GetComponent<Obstacle>().ObstacleClear();
                    else
                        child.gameObject.SetActive(false);
                }
            }
        }
        s.Spawn();
    }

    void DespawnSegment()
    {
        segments[amountOfActiveSegments - 1].Despawn();
        amountOfActiveSegments--;
    }

    void SpawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions;
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.up * currentSpawnY;

        currentSpawnY += s.length;
        amountOfActiveSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if(s == null)
        {
            GameObject go = Instantiate(transition ? availableTransitions[id].gameObject : availableSegments[id].gameObject) as GameObject;
            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;

            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
            for (int i = 0; i < s.transform.childCount; i++)
                s.transform.GetChild(i).gameObject.SetActive(true);
        }

        return s;
    }

    public Design GetDesign(DesignType dt, int visualIndex)
    {
        Design d = designs.Find(x => x.type == dt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if (d == null)
        {
            Design clone = null;
            if (dt == DesignType.obstacle)
                clone = obstacles[visualIndex];
            else if (dt == DesignType.tripleObstacle)
                clone = tripleMidObs[visualIndex];

            d = clone;
            designs.Add(d);
        }
        return d;
    }

    public void DeactivateAll(float range)
    {
        deactivateSegments = true;
        deactivateRange = Player.Instance.transform.position.y + range;

        for (int i = 0; i < amountOfActiveSegments; i++)
        {
            foreach (Transform child in segments[i].transform)
            {
                if (child.position.y < (yScreenBottom + range) && child.position.y > yScreenBottom)
                {
                    if (child.GetComponent<Obstacle>() != null)
                        child.GetComponent<Obstacle>().ObstacleClear();
                    else
                        child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void DeactiveObstaclesOnly(float range)
    {
        for (int i = 0; i < amountOfActiveSegments; i++)
        {
            foreach (Transform child in segments[i].transform)
            {
                if (child.GetComponent<Obstacle>() != null)
                {
                    if (child.transform.position.y < (yScreenBottom + range) && child.transform.position.y > yScreenBottom)
                            child.gameObject.SetActive(false);
                }
            }
        }
    }
}
