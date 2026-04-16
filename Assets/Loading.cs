using UnityEngine;

public class Loading : MonoBehaviour
{
    public static Loading Instance { set; get; }

    RectTransform rt;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rt = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }

    void Update()
    {
        rt.Rotate(Vector3.back * 4f, Space.Self);
    }

    public void Close()
    {
        UIManager.Instance.OnPopUpClose(transform.GetComponent<RectTransform>(), 0.2f);
    }
}
