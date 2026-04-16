using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridMenuAutoFitter : MonoBehaviour
{
    public RectTransform viewPort;
    RectTransform rt;
    GridLayoutGroup gl;

    IEnumerator Start()
    {
        yield return null;
        rt = GetComponent<RectTransform>();
        gl = GetComponent<GridLayoutGroup>();
        float x = rt.rect.width / 3f + viewPort.sizeDelta.x / 2f;
        gl.cellSize = new Vector2(x, gl.cellSize.y);
    }
}
