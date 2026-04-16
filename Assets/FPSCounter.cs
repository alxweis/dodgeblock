using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    Text fpsText;

    void Start()
    {
        fpsText = GetComponent<Text>();
    }

    void Update()
    {
        float fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = fps.ToString("0");
    }
}
