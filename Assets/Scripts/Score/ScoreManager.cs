using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { set; get; }

    int scoreMultiplier;
    int coinscoreMultiplier;

    // In Game
    public int score;
    public int coinScore;

    public Text scoreText;
    public Image scoreTextBackground;
    public Text coinScoreText;
    public Image coinScoreTextBackground;

    public Image scoreboost;
    public Image coinboost;

    public ParticleSystem highscorePS;
    public Animator highscoreAnim;
    bool playedHSPS;

    // Out of Game
    public int coins;
    public int shields;
    int lastCoins;

    public Text highScoreText;
    public Text lastScoreText;
    public Text coinText;
    public Text shieldText;

    bool c, s;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        highScoreText.text = "Best: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
        lastScoreText.text = "Last: " + PlayerPrefs.GetInt("LastScore", 0).ToString();

        coinText.text = PlayerPrefs.GetInt("Coins", coins).ToString();
        shieldText.text = PlayerPrefs.GetInt("Shields", shields).ToString();

        coins = PlayerPrefs.GetInt("Coins", coins);
        shields = PlayerPrefs.GetInt("Shields", shields);
        coinText.text = ShortNumStr(coins);
        shieldText.text = ShortNumStr(shields);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LastScore", -1);
    }

    void Update()
    {
        float coinImageWidth = 190f + 30f * (coinScore.ToString().Length - 1);
        coinScoreTextBackground.rectTransform.sizeDelta = new Vector2(coinImageWidth, coinScoreTextBackground.rectTransform.sizeDelta.y);

        float scoreImageWidth = 120f + 40f * (score.ToString().Length - 1);
        scoreTextBackground.rectTransform.sizeDelta = new Vector2(scoreImageWidth, scoreTextBackground.rectTransform.sizeDelta.y);

        if (ItemManager.Instance.Item("Scoreboost").active)
        {
            scoreMultiplier = 2 * Player.Instance.scoreFactor;

            scoreboost.gameObject.SetActive(true);
            scoreboost.rectTransform.GetChild(0).GetComponent<Text>().text = "x" + scoreMultiplier.ToString("0");
        }
        else
            scoreboost.gameObject.SetActive(false);

        if (ItemManager.Instance.Item("Coinboost").active)
        {
            coinscoreMultiplier = 2 * Player.Instance.coinFactor;

            coinboost.gameObject.SetActive(true);
            coinboost.rectTransform.GetChild(0).GetComponent<Text>().text = "x" + coinscoreMultiplier.ToString("0");
        }
        else
            coinboost.gameObject.SetActive(false);

        if (ParticleToTarget.Instance.collected)
        {
            coins = ParticleToTarget.Instance.lastCoins + GameManager.Instance.collectCoins;
            PlayerPrefs.SetInt("Coins", coins);
            GameManager.Instance.scoreCoins = false;
            ParticleToTarget.Instance.collected = false;
        }

        if(coins != lastCoins)
        {
            lastCoins = coins;
            PlayerPrefs.SetInt("Coins", coins);
        }
        lastCoins = coins;

        PlayerPrefs.SetInt("Shields", shields);
        PlayerPrefs.SetInt("LastScore", score);

        if (s)
            shieldText.text = NumStr(shields);
        else
            shieldText.text = ShortNumStr(shields);

        if (c)
            coinText.text = NumStr(coins);
        else
            coinText.text = ShortNumStr(coins);
    }

    public string ShortNumStr(int i)
    {
        string s;
        if (i >= 1000 && i < 1000000)
            s = (i / 1000f).ToString("F1") + "K";
        else if (i >= 1000000 && i < 1000000000)
            s = (i / 1000000f).ToString("F2") + "M";
        else if (i >= 1000000000)
            s = (i / 1000000000f).ToString("F2") + "B";
        else
            s = i.ToString("0");
        return s;
    }
    public string NumStr(int i)
    {
        string s;
        s = i.ToString("#,#");
        return s;
    }

    public void CoinToggle(Toggle t)
    {
        if (t.isOn)
            c = true;
        else
            c = false;
    }

    public void ShieldToggle(Toggle t)
    {
        if (t.isOn)
            s = true;
        else
            s = false;
    }

    public void ScoreUp()
    {
        if (!ItemManager.Instance.Item("Scoreboost").active)
            score++;
        else
            score = score + scoreMultiplier;

        scoreText.text = score.ToString();

        if (score > PlayerPrefs.GetInt("Highscore", 0))
        {
            if (!playedHSPS)
            {
                highscoreAnim.SetTrigger("NewHighscore");
                highscorePS.Play();
                AudioManager.Instance.Play("Pop");
                AudioManager.Instance.Play("Rescue");
                playedHSPS = true;
            }
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    public void CoinScoreUp()
    {
        if (!ItemManager.Instance.Item("Coinboost").active) 
            coinScore++;
        else
            coinScore = coinScore + coinscoreMultiplier;
            
        coinScoreText.text = coinScore.ToString();
    }
}
