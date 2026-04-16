using UnityEngine;
using UnityEngine.UI;

public class ColorLerper : MonoBehaviour
{
    public static ColorLerper Instance { set; get; }

    public float lerpTime;

    [Header("Objects")]
    public Text dodge;
    public Text block;
    public Text highScore, lastScore;
    public Image background;
    public Image leftSide;
    public Image rightSide;

    [Header("Background")]
    public Color bCurrentColor;
    public Color[] bColors;
    
    [Header("Sides")]
    public Color sCurrentColor;
    public Color[] sColors;

    int bColorIndex, sColorIndex, bLen, sLen;
    float t;

    public Color dodgeBackColor, blockBackColor;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bColorIndex = Random.Range(0, bColors.Length);
        bCurrentColor = bColors[bColorIndex];

        sColorIndex = Random.Range(0, sColors.Length);
        sCurrentColor = sColors[sColorIndex];

        bLen = bColors.Length;
        sLen = sColors.Length;
    }

    void Update()
    {
        #region ColorLerper
        bCurrentColor = Color.Lerp(bCurrentColor, bColors[bColorIndex], lerpTime * Time.deltaTime);
        sCurrentColor = Color.Lerp(sCurrentColor, sColors[sColorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

        if(t > 0.9f)
        {
            t = 0f;
            bColorIndex++;
            sColorIndex++;
            bColorIndex = (bColorIndex >= bLen) ? 0 : bColorIndex;
            sColorIndex = (sColorIndex >= sLen) ? 0 : sColorIndex;
        }
        #endregion

        background.color = bCurrentColor;
        rightSide.color = leftSide.color = sCurrentColor;

        Color custom = bCurrentColor + 0.5f * sCurrentColor;
        Color invertCustom = new Color(1f - custom.r, 1f - custom.g, 1f - custom.b, 1f);

        dodge.color = invertCustom;
        block.color = ColorMultiplier(invertCustom, 0.85f);

        dodgeBackColor = ColorMultiplier(dodge.color, 0.2f);
        blockBackColor = ColorMultiplier(block.color, 0.2f);
        
        highScore.color = lastScore.color = invertCustom;

        dodge.GetComponent<Outline>().effectColor = dodge.GetComponent<Shadow>().effectColor = dodgeBackColor;
        block.GetComponent<Outline>().effectColor = block.GetComponent<Shadow>().effectColor = blockBackColor;

        highScore.GetComponent<Outline>().effectColor = highScore.GetComponent<Shadow>().effectColor = dodgeBackColor;
        lastScore.GetComponent<Outline>().effectColor = lastScore.GetComponent<Shadow>().effectColor = dodgeBackColor;
    }

    Color ColorMultiplier(Color color, float multiplier)
    {
        return color = new Color(multiplier * color.r, multiplier * color.g, multiplier * color.b, 1f);
    }
}
