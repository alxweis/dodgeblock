using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections;
using EZCameraShake;
using Kino;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public Transform player;
    public GameObject playerCharacter;
    public Transform magnet;

    [Header("Game Booleans")]
    public bool isGameStarted;
    public bool isPaused;
    public bool isDead;

    [Header("Game Values")]
    public int startCoins;
    public int startShields;
    public float chanceForFreeButton = 0.9f;
    float chanceForRandomAd;
    public float maxTimeForRandomAd;
    public float startLerpDuration = 1f;
    int alreadyRevived;
    public bool inCrashedMenu;
    public int collectCoins;

    [Header("Animators")]
    public Animator mainMenuAnim;
    public Animator bankAnim;
    public GameObject inGameCanvas;
    public RectTransform bank;

    [Header("Revive")]
    public float timeToSkip = 4f;
    public Animator reviveAnim;
    public Image reviveTimer;
    float reviveT;
    bool showReviveMenu;
    bool crashed;
    public Button skipButton;
    public GameObject freeButton;
    public GameObject adButton;
    public GameObject coinButton;
    public Text reviveCoinPriceText;
    bool usedAdRevive;

    int reviveCoinPrice;
    public int startReviveCoinPrice;

    [Header("Crashed")]
    public Animator crashedAnim;

    public Animator pauseAnim;
    public Animator deathAnim;
    public Animator scoreHolderAnim;
    public Animator crossFadeAnim;

    public Text coinRewardText;

    public GameObject coinPlusButton, shieldPlusButton, keyPlusButton;

    [Header("Particle Systems")]
    public Transform xYPlayerFollow;
    public Transform yPlayerFollow;
    public ParticleSystem uICoinCollect;
    public ParticleSystem deathPS;
    public ParticleSystem shieldPS;
    public ParticleSystem airStripes;

    public Button playButton;
    public GameObject pauseButton;
    
    [Header("Item Timer")]
    public RectTransform carrier;
    bool displayed;

    [Header("Booster")]
    public GameObject smallBoost;
    public GameObject[] bigBoosts;
    public Image backgroundFade;

    const int smallBoostDebugRange = 10;
    const int bigBoostdebugRange = 20;

    public float adder;
    public int particleAmount;
    public const int MaxParticles = 40;
    public bool scoreCoins;

    public int counter;
    const float JitterTime = 4f;
    float t;
    public bool jitter, jitterBoost;
    const float BigBoostRange = 140f;
    const int MaxCount = 50;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("LastScore", 0) != -1)
        {
            ScoreManager.Instance.lastScoreText.gameObject.SetActive(true);
            ScoreManager.Instance.highScoreText.rectTransform.anchoredPosition = new Vector2(250f, ScoreManager.Instance.highScoreText.rectTransform.anchoredPosition.y);
        }

        if (!PlayerPrefs.HasKey("FirstGameStart"))
        {
            UIManager.Instance.WelcomePopUp("WELCOME", "Thank you for\n downloading the game! Get now <color=#FFD900>" + startCoins + " Coins</color> and <color=#00A0FF>" + startShields + " Shields</color> for free!");
            PlayerPrefs.SetInt("FirstGameStart", 1);
        }

        mainMenuAnim.SetTrigger("GameStart");
        bankAnim.SetTrigger("On");
        coinPlusButton.SetActive(true);
        shieldPlusButton.SetActive(true);
        keyPlusButton.SetActive(true);

        AdvertismentManager.Instance.ShowRandomAd("bigbanner");
    }

    void Update()
    {
        xYPlayerFollow.position = new Vector3(player.transform.position.x, player.transform.position.y, -5f);
        yPlayerFollow.position = new Vector3(0f, player.transform.position.y, -5f);

        magnet.position = player.transform.position;

        if (showReviveMenu)
        {
            if (reviveTimer.fillAmount > 0f)
            {
                reviveT += Time.deltaTime / timeToSkip;
                reviveTimer.fillAmount = Mathf.Lerp(1f, 0f, reviveT);
                skipButton.interactable = false;
                skipButton.transform.GetChild(0).GetComponent<Text>().text = (-timeToSkip * reviveT + timeToSkip).ToString("0.0");
            }
            else
            {
                reviveT = 0f;
                skipButton.interactable = true;
                skipButton.transform.GetChild(0).GetComponent<Text>().text = "Skip";
                showReviveMenu = false;
            }
        }

        if (crashed)
        {
            int coinReward = ParticleToTarget.Instance.coinReward;
            if (coinReward >= 1000)
                coinRewardText.text = (coinReward / 1000f).ToString("F1") + "K";
            else if (coinReward >= 1000000)
                coinRewardText.text = (coinReward / 1000000f).ToString("F2") + "M";
            else
                coinRewardText.text = coinReward.ToString("#,0");
        }

        float playerSpeed = Player.Instance.speed;
        float a = playerSpeed / Time.deltaTime;
        var emission = airStripes.emission;
        if(!isDead && !isPaused && (a > 500f || playerSpeed > 11f))
        {
            emission.enabled = true;
            var main = airStripes.main;
            main.simulationSpeed = playerSpeed;
        }
        else
            emission.enabled = false;

        if (jitter)
        {
            if (t > 0f)
            {
                t -= Time.deltaTime / Time.timeScale;
                Time.timeScale = 0.1f;
                if (Input.GetMouseButtonDown(0))
                    counter++;
            }
            else
            {
                jitterBoost = true;
                jitter = false;
            }
        }
        else
        {
            Time.timeScale = 1f;
            if(!jitterBoost)
                counter = 0;
            t = JitterTime;
        }

        if (jitterBoost)
        {
            if (counter > MaxCount)
                counter = MaxCount;
            Debug.Log(counter);

            float constant = ItemManager.Instance.Item("Bigboost").duration / BigBoostRange * (MaxCount + BigBoostRange);
            ItemManager.Instance.Item("Jitterboost").duration = (counter + BigBoostRange) / (MaxCount + BigBoostRange) * constant;

            //DeactivateSegments(counter + BigBoostRange);

            Vector2 pos = new Vector2(0f, playerCharacter.transform.position.y);

            //BoostCoinManager.Instance.SpawnCoinLine(25, 20, pos.y);

            //int i = Random.Range(0, bigBoosts.Length);
            //GameObject clone = Instantiate(bigBoosts[i], pos, Quaternion.identity);
            //Destroy(clone, 15f);

            StartCoroutine(BigCameraLerp("Jitterboost", startLerpDuration, 0.5f, 1.2f, -1f));

            counter = 0;
            jitterBoost = false;
        }

        if (!isGameStarted && !isDead && !HangarManager.Instance.hangarOpen && !ShopManager.Instance.shopOpen)
        {
            if (chanceForRandomAd > 0.9f)
            {
                AdvertismentManager.Instance.ShowRandomAd("bigbanner");
                chanceForRandomAd = 0f;
            }
            else
                chanceForRandomAd += Time.deltaTime / maxTimeForRandomAd;
        }

        if (isGameStarted && !isPaused)
        {
            if (!ItemManager.Instance.Item("Smallboost").active && !ItemManager.Instance.Item("Bigboost").active && !ItemManager.Instance.Item("Jitterboost").active)
                Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency = Mathf.Lerp(Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency, 5010f, Time.deltaTime * 10f);
            else
                Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency = Mathf.Lerp(Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency, 3000f, Time.deltaTime * 10f);
        }
        else
        {
            Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency = Mathf.Lerp(Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency, 1000f, Time.deltaTime * 10f);
        }

        if (!isPaused)
        {
            if (ItemManager.Instance.Item("Smallboost").active || ItemManager.Instance.Item("Bigboost").active || ItemManager.Instance.Item("Jitterboost").active)
                pauseButton.SetActive(false);
            else
                pauseButton.SetActive(true);
        }
        else
        {
            pauseButton.SetActive(false);
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (pause && !isPaused && isGameStarted)
                Pause();
    }

    // Start Game
    public void OnPlay()
    {
        isGameStarted = true;
        isDead = false;

        inGameCanvas.SetActive(true);
        mainMenuAnim.SetTrigger("StartGame");
        scoreHolderAnim.SetTrigger("StartGame");
        bankAnim.SetTrigger("Off");
    }

    #region Pause
    public void Pause()
    {
        if (!isDead)
        {
            if (isDead)
                reviveAnim.SetTrigger("MenuExit");

            pauseAnim.SetTrigger("Menu");
            Time.timeScale = 0f;

            isPaused = true;
        }
    }

    public void OnResume()
    {
        isGameStarted = true;
        isDead = false;
        pauseAnim.SetTrigger("MenuExit");
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnSettings()
    {
        UIManager.Instance.SettingsPopUp();
    }

    public void OnRoundLeave()
    {
        UIManager.Instance.LeavePopUp();
    }
    #endregion

    #region Death
    public void OnDeath()
    {
        if(isGameStarted)
        {
            isDead = true;
            isGameStarted = false;

            CameraShaker.Instance.ShakeOnce(3f, 2f, 0.1f, 1f);
            deathPS.Play();
            playerCharacter.gameObject.SetActive(false);

            if (ScoreManager.Instance.coinScore > 0)
            {
                StartCoroutine(TimeToShowRevive());
            }
            else
            {
                OnLeave(0.5f);
                showReviveMenu = false;
            }
        }
    }

    IEnumerator TimeToShowRevive()
    {
        yield return new WaitForSeconds(0.5f);
        showReviveMenu = true;
        reviveTimer.fillAmount = 1f;
        reviveAnim.SetTrigger("Menu");

        if (Random.value > chanceForFreeButton)
        {
            freeButton.SetActive(true);
            adButton.SetActive(false);
            coinButton.SetActive(false);
        }
        else
        {
            freeButton.SetActive(false);

            if (usedAdRevive)
            {
                adButton.SetActive(false);
                if (ScoreManager.Instance.coins < reviveCoinPrice)
                    reviveTimer.fillAmount = 0f;
                coinButton.GetComponent<RectTransform>().anchoredPosition = freeButton.GetComponent<RectTransform>().anchoredPosition;
                coinButton.GetComponent<RectTransform>().sizeDelta = freeButton.GetComponent<RectTransform>().sizeDelta;
            }
            else
            {
                adButton.SetActive(true);
            }

            coinButton.SetActive(true);

            reviveCoinPrice = startReviveCoinPrice * (int)Mathf.Pow(2, alreadyRevived);

            if (ScoreManager.Instance.coins < reviveCoinPrice)
                coinButton.GetComponent<Button>().interactable = false;
            else
                coinButton.GetComponent<Button>().interactable = true;
            reviveCoinPriceText.text = reviveCoinPrice.ToString("0");

            alreadyRevived++;
        }
    }

    public void OnAd()
    {
        //AdvertismentManager.Instance.ShowAd("rewardedVideo", new ShowOptions { resultCallback = HandleAdResult });
        //usedAdRevive = true;
    }

    void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Revive();
                AdvertismentManager.Instance.adPanel.SetActive(false);
                break;
            case ShowResult.Skipped:
                UIManager.Instance.InfoPopUp("Error", "Playing the ad failed!");
                break;
            case ShowResult.Failed:
                UIManager.Instance.InfoPopUp("Error", "Playing the ad failed!");
                break;
        }
    }

    public void OnCoinRevive()
    {
        ScoreManager.Instance.coins -= reviveCoinPrice;
        Revive();
    }

    public void Revive()
    {
        deathAnim.SetTrigger("MenuExit");
        isDead = false;
        isGameStarted = true;
        playerCharacter.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        playerCharacter.SetActive(true);
        LevelManager.Instance.DeactiveObstaclesOnly(15f);
        Pause();
    }

    public void OnSkip()
    {
        inCrashedMenu = true;
        crashed = true;
        ParticleToTarget.Instance.coinReward = ScoreManager.Instance.coinScore;

        inGameCanvas.SetActive(false);
        reviveAnim.SetTrigger("MenuExit");
        crashedAnim.SetTrigger("Menu");

        bankAnim.SetTrigger("On");
    }

    public void OnCollect(Button b)
    {
        scoreCoins = true;
        CollectCoins(ScoreManager.Instance.coinScore);
        b.interactable = false;
        b.transform.parent.GetChild(2).GetComponent<Button>().interactable = false;

        float timeToWait;
        if (ScoreManager.Instance.coinScore > 0)
            timeToWait = 1.8f;
        else
            timeToWait = 0f;

        OnLeave(timeToWait);
    }

    public void CollectCoins(int coinsToCollect)
    {
        collectCoins = coinsToCollect;
        if (coinsToCollect > MaxParticles)
        {
            adder = coinsToCollect / MaxParticles;
            particleAmount = MaxParticles;
        }
        else
        {
            adder = 1f;
            particleAmount = coinsToCollect;
        }
        var emission = uICoinCollect.emission;
        emission.enabled = true;
        emission.SetBurst(0, new ParticleSystem.Burst(0f, particleAmount));
        ParticleToTarget.Instance.fCoins = ParticleToTarget.Instance.lastCoins = ScoreManager.Instance.coins;
        uICoinCollect.Play();
    }

    public void OnLeave(float timeToWait)
    {
        inCrashedMenu = false;
        StartCoroutine(LoadScene(timeToWait));
    }

    IEnumerator LoadScene(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        crossFadeAnim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(0);
    }

    #endregion

    public void SmallBoost()
    {
        float smallBoostRange = Player.Instance.SmallBoostSpeed * ItemManager.Instance.Item("Smallboost").duration;
        LevelManager.Instance.DeactivateAll(smallBoostRange - 0.5f * smallBoostDebugRange);

        int coinLineAmount = Mathf.RoundToInt((smallBoostRange - 2.75f * smallBoostDebugRange) / BoostCoinManager.Instance.lineLength);
        BoostCoinManager.Instance.SpawnLinearCoinLine(coinLineAmount, smallBoostDebugRange, playerCharacter.transform.position.y);

        StartCoroutine(SmallCameraLerp("Smallboost", 1.5f, -3f));
    }

    public void BigBoost()
    {
        float bigBoostRange = Player.Instance.BigBoostSpeed * ItemManager.Instance.Item("Bigboost").duration;
        LevelManager.Instance.DeactivateAll(bigBoostRange - 0.25f * bigBoostdebugRange);

        int coinLineAmount = Mathf.RoundToInt((bigBoostRange - 2f * bigBoostdebugRange) / BoostCoinManager.Instance.lineLength);
        BoostCoinManager.Instance.SpawnRandomCoinLine(coinLineAmount, bigBoostdebugRange, playerCharacter.transform.position.y);

        StartCoroutine(BigCameraLerp("Bigboost", startLerpDuration, 0.5f, 1.2f, -1f));
    }

    public void JitterBoost()
    {
        Debug.Log("Jitterboost");

        jitter = true;
    }

    IEnumerator SmallCameraLerp(string name, float max, float yDeviation)
    {
        float duration = ItemManager.Instance.Item(name).duration;
        float startSize = AspectRatio.Instance.orthoSize;
        float counter = 0f;
        bool playedBoostThunder = false;

        float endValue = Random.Range(0.2f, 0.5f);

        CameraShaker.Instance.ShakeOnce(4f, 2f, duration / 2f, duration / 2f);

        while (counter < duration / 2f)
        {
            counter += Time.deltaTime;
            float t = counter / duration;
            Camera.main.orthographicSize = Mathf.Lerp(startSize, startSize + max, t);
            CameraFollow.Instance.yDeviation = Mathf.SmoothStep(0f, yDeviation, t);

            Camera.main.transform.GetComponent<AnalogGlitch>().scanLineJitter = Mathf.Lerp(endValue, 0f, t);
            Camera.main.transform.GetComponent<AnalogGlitch>().colorDrift = Mathf.Lerp(endValue, 0f, t);
            yield return null;
        }
        if(counter >= duration / 2f)
        {
            if (!playedBoostThunder)
            {
                AudioManager.Instance.Play("BoostThunder");
                playedBoostThunder = true;
            }

            while (counter < duration)
            {
                counter += Time.deltaTime;
                float t = counter / duration;
                Camera.main.orthographicSize = Mathf.Lerp(startSize + max, startSize, t);
                CameraFollow.Instance.yDeviation = Mathf.SmoothStep(yDeviation, 0f, t);

                Camera.main.transform.GetComponent<AnalogGlitch>().scanLineJitter = Mathf.Lerp(endValue, 0f, t);
                Camera.main.transform.GetComponent<AnalogGlitch>().colorDrift = Mathf.Lerp(endValue, 0f, t);
                yield return null;
            }
        }
    }

    IEnumerator BigCameraLerp(string name, float startLerpDuration, float startMax, float middleMax, float yDeviation)
    {
        float duration = ItemManager.Instance.Item(name).duration;
        float startSize = AspectRatio.Instance.orthoSize;
        float dragSpeed = Player.Instance.dragSpeed;
        float counter = 0f;
        bool playedBoostThunder = false;

        float endValue = Random.Range(0.5f, 1f);

        CameraShakeInstance shake = null;

        shake = CameraShaker.Instance.StartShake(2f, 2f, startLerpDuration);

        // Start Phase
        while (counter < startLerpDuration)
        {
            counter += Time.deltaTime;
            float t = counter / startLerpDuration;
            Camera.main.orthographicSize = Mathf.Lerp(startSize, startSize + startMax, t);
            CameraFollow.Instance.yDeviation = Mathf.SmoothStep(0f, yDeviation, t);
            Player.Instance.dragSpeed = Mathf.SmoothStep(dragSpeed, dragSpeed / 1.1f, t);

            Camera.main.transform.GetComponent<AnalogGlitch>().scanLineJitter = Mathf.Lerp(endValue, 0f, t);
            Camera.main.transform.GetComponent<AnalogGlitch>().colorDrift = Mathf.Lerp(endValue, 0f, t);
            yield return null;
        }
        // Middle Phase
        if (counter >= startLerpDuration && counter < (duration - startLerpDuration))
        {
            // 1. Middle Phase
            while (counter < (duration / 2f))
            {
                counter += Time.deltaTime;
                float t = (counter - startLerpDuration) / (duration / 2f - startLerpDuration);
                Camera.main.orthographicSize = Mathf.Lerp(startSize + startMax, startSize + middleMax, t);
                yield return null;
            }
            // 2. Middle Phase
            if (counter >= (duration / 2f))
            {
                while (counter < (duration - startLerpDuration))
                {
                    counter += Time.deltaTime;
                    float t = (counter - startLerpDuration - (duration / 2f - startLerpDuration)) / (duration / 2f - startLerpDuration);
                    Camera.main.orthographicSize = Mathf.Lerp(startSize + middleMax, startSize + startMax, t);
                    yield return null;
                }
            }
        }
        // End Phase
        if (counter >= (duration - startLerpDuration))
        {
            if (!playedBoostThunder)
            {
                AudioManager.Instance.Play("BoostThunder");
                playedBoostThunder = true;
            }

            while (counter < duration)
            {
                counter += Time.deltaTime;
                float t = (counter - startLerpDuration - (2 * (duration / 2f - startLerpDuration))) / startLerpDuration;
                Camera.main.orthographicSize = Mathf.Lerp(startSize + startMax, startSize, t);
                CameraFollow.Instance.yDeviation = Mathf.SmoothStep(yDeviation, 0f, t);
                Player.Instance.dragSpeed = Mathf.SmoothStep(dragSpeed / 1.1f, dragSpeed, t);

                Camera.main.transform.GetComponent<AnalogGlitch>().scanLineJitter = Mathf.Lerp(endValue, 0f, t);
                Camera.main.transform.GetComponent<AnalogGlitch>().colorDrift = Mathf.Lerp(endValue, 0f, t);
                shake.StartFadeOut(startLerpDuration);
                yield return null;
            }
        }
    }
}
