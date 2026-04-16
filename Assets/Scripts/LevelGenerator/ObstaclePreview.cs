using UnityEngine;

public class ObstaclePreview : MonoBehaviour
{
    void Start()
    {
        Design obstacleDesign = LevelManager.Instance.GetDesign(DesignType.obstacle, Random.Range(0, LevelManager.Instance.obstacles.Count));
        var left = Instantiate(obstacleDesign, transform.GetChild(0).position, Quaternion.identity);
        left.transform.SetParent(transform.GetChild(0), true);
        left.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
        var right = Instantiate(obstacleDesign, transform.GetChild(1).position, Quaternion.identity);
        right.transform.SetParent(transform.GetChild(1), true);
    }
}
