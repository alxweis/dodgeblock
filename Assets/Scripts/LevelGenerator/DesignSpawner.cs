using UnityEngine;

public class DesignSpawner : MonoBehaviour
{
    public DesignType type;
    Design currentObDesign, currentMidDesign, left, middle, right;
    bool spawned;
    bool tripleOb;

    public void Spawn()
    {
        int amtObs = 0;
        int amtMids = 0;

        switch (type)
        {
            case DesignType.obstacle:
                amtObs = LevelManager.Instance.obstacles.Count;
                break;
            case DesignType.tripleObstacle:
                tripleOb = true;
                amtObs = LevelManager.Instance.obstacles.Count;
                amtMids = LevelManager.Instance.tripleMidObs.Count;
                break;
        }

        currentObDesign = LevelManager.Instance.GetDesign(DesignType.obstacle, Random.Range(0, amtObs));
        if(tripleOb)
            currentMidDesign = LevelManager.Instance.GetDesign(type, Random.Range(0, amtMids));

        if(!spawned)
        {
            left = Instantiate(currentObDesign, transform.GetChild(0).position, Quaternion.identity);
            left.transform.SetParent(transform.GetChild(0), true);
            left.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
            right = Instantiate(currentObDesign, transform.GetChild(1).position, Quaternion.identity);
            right.transform.SetParent(transform.GetChild(1), true);
            if (tripleOb)
            {
                middle = Instantiate(currentMidDesign, transform.GetChild(2).position, Quaternion.identity);
                middle.transform.SetParent(transform.GetChild(2), true);
            }
            spawned = true;
        }

        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        if (tripleOb)
            middle.gameObject.SetActive(true);
    }

    public void Despawn()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        if (tripleOb)
            middle.gameObject.SetActive(false);
    }
}
