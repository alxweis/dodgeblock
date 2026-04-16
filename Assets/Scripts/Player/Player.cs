using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { set; get; }

    Transform playerCharacter;
    ItemManager iM;

    [Header("Speed")]
    public float speed;
    public float speedStart = 5f;
    public float speedEnd = 8f;
    const float MinSpeed = 7f;
    const float MaxSpeed = 10f;
    [HideInInspector]
    public float BigBoostSpeed = 18f;
    [HideInInspector]
    public float JitterBoostSpeed = 18f;
    [HideInInspector]
    public float SmallBoostSpeed = 18f;
    float lerpT, backLerpT, speedStamp;
    bool didSpeedStamp;

    // Time
    float t, timeStamp;
    float currentLerpTime;

    // Time To Lerp
    public float timeToLerp = 10f;
    const float MinTimeToLerp = 2f;
    const float MaxTimeToLerp = 10f;

    // Time Interval
    public float timeInterval = 5f;
    const float MinTimeInterval = 5f;
    const float MaxTimeInterval = 10f;

    [Header("Horizontal Movement")]
    public float dragSpeed = 2f;
    public float smoothDrag = 5f;
    const float MaxAngle = 45f;
    const float MapWidth = 2.2f;
    float deltaX, xdesired;
    bool drag;

    //Input
    float deltaTapTime, lastTapTime, deltaDTapTime, lastDTapTime;
    float tapDelay = 250f;

    [Header("Shield")]
    public GameObject shield;
    public ParticleSystem shieldPS;
    bool enable;

    [Header("Scoreboost")]
    public int scoreFactor = 1;

    [Header("Coinboost")]
    public int coinFactor = 1;

    void Awake()
    {
        Instance = this;
        playerCharacter = transform.GetChild(0);
    }

    void Update()
    {
        iM = ItemManager.Instance;
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.isPaused && !GameManager.Instance.isDead)
        {
            #region Input
            float mousePosX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x * dragSpeed;

            if (Input.GetMouseButtonDown(0))
            {
                DoubleTap();
                deltaX = mousePosX - transform.position.x;
                drag = false;
            }
            else if (Input.GetMouseButton(0))
            {
                xdesired = mousePosX - deltaX;
                Vector2 desiredPos = new Vector2(xdesired, transform.position.y);
                desiredPos.x = Mathf.Clamp(desiredPos.x, -MapWidth, MapWidth);
                transform.position = Vector2.Lerp(transform.position, desiredPos, Time.deltaTime * smoothDrag);
                drag = true;
            }
            else
            {
                xdesired = Mathf.Lerp(xdesired, transform.position.x, Time.deltaTime * smoothDrag);
                Vector2 desiredPos = new Vector2(xdesired, transform.position.y);
                desiredPos.x = Mathf.Clamp(desiredPos.x, -MapWidth, MapWidth);
                transform.position = Vector2.Lerp(transform.position, desiredPos, Time.deltaTime * smoothDrag);
            }
            #endregion

            #region Rotation Manager
            if (drag)
            {
                float direction = Mathf.Clamp(xdesired, -MapWidth, MapWidth) - transform.position.x;
                float angle = Mathf.Atan2(speed, direction * smoothDrag) * Mathf.Rad2Deg - 90f;
                Quaternion rot = Quaternion.AngleAxis(Mathf.Clamp(angle, -MaxAngle, MaxAngle), Vector3.forward);
                playerCharacter.transform.rotation = rot;
            }
            #endregion

            #region Speed Manager
            if (!iM.Item("Smallboost").active && !iM.Item("Bigboost").active && !iM.Item("Jitterboost").active)
            {
                if(currentLerpTime < timeToLerp)
                    currentLerpTime += Time.deltaTime;
                t = -Mathf.Pow((currentLerpTime / timeToLerp) - 1f, 2f) + 1f;
                speed = Mathf.Lerp(speedStart, speedEnd, t);

                #region Old
                /*

                if(speed != speedEnd)
                {
                    speed = Mathf.Lerp(speedStart, speedEnd, t - timeStamp);
                }
                else
                {
                    timeInterval -= Time.deltaTime;
                    if(timeInterval <= 0f)
                    {
                        bool doTimeStamp = true;
                        if (doTimeStamp)
                        {
                            timeStamp = t;
                            doTimeStamp = false;
                        }

                        speedStart = speedEnd;
                        speedEnd = Random.Range(MinSpeed, MaxSpeed);
                        speed = speedStart;

                        timeToLerp = Random.Range(MinTimeToLerp, MaxTimeToLerp);
                        timeInterval = Random.Range(MinTimeInterval, MaxTimeInterval);
                    }
                }*/
                #endregion

                didSpeedStamp = false;
                
                speed = Mathf.Lerp(speedStart, speedEnd, t);

                AudioManager.Instance.HandleEngineSound(false);
            }
            else
            {
                if (iM.Item("Bigboost").active)
                {
                    if (!didSpeedStamp)
                    {
                        speedStamp = speed;
                        backLerpT = 0f;
                        didSpeedStamp = true;
                    }
                    speed = BigBoostSpeed;

                    if (iM.Item("Bigboost").timer <= GameManager.Instance.startLerpDuration)
                    {
                        backLerpT += Time.deltaTime / GameManager.Instance.startLerpDuration;
                        speed = Mathf.Lerp(BigBoostSpeed, speedStamp, backLerpT);
                    }
                }
                else if (iM.Item("Smallboost").active)
                {
                    if (!didSpeedStamp)
                    {
                        speedStamp = speed;
                        backLerpT = 0f;
                        didSpeedStamp = true;
                    }
                    speed = SmallBoostSpeed;

                    if (iM.Item("Smallboost").timer <= (iM.Item("Smallboost").duration / 2f))
                    {
                        backLerpT += Time.deltaTime;
                        speed = Mathf.Lerp(SmallBoostSpeed, speedStamp, backLerpT);
                    }
                }
                else if (iM.Item("Jitterboost").active)
                {
                    if (!didSpeedStamp)
                    {
                        speedStamp = speed;
                        backLerpT = 0f;
                        didSpeedStamp = true;
                    }
                    speed = JitterBoostSpeed;

                    if (iM.Item("Jitterboost").timer <= GameManager.Instance.startLerpDuration)
                    {
                        backLerpT += Time.deltaTime / GameManager.Instance.startLerpDuration;
                        speed = Mathf.Lerp(JitterBoostSpeed, speedStamp, backLerpT);
                    }
                }

                AudioManager.Instance.HandleEngineSound(true);
            }
            #endregion

            #region Shield Manager
            if (iM.Item("Shield").active)
            {
                if (!enable)
                {
                    playerCharacter.GetComponent<Animator>().SetTrigger("True");
                    StartCoroutine(ShieldIdleTimer());
                    shield.GetComponent<SpriteRenderer>().sprite = ShuttleManager.Instance.shuttleItems[PlayerPrefs.GetInt("Shuttle")].prim;
                    shield.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    AudioManager.Instance.Play("PowerUp");
                    enable = true;
                }
            }
            else
            {
                if (enable)
                {
                    playerCharacter.GetComponent<Animator>().SetTrigger("False");
                    enable = false;
                }
            }
            #endregion

            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else
        {
            xdesired = transform.position.x;
            AudioManager.Instance.EngineSoundOff();
        }
    }

    IEnumerator ShieldIdleTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if(iM.Item("Shield").active)
            playerCharacter.GetComponent<Animator>().SetTrigger("Idle");
    }

    void DoubleTap()
    {
        if (lastTapTime != 0 && !GameManager.Instance.jitter)
        {
            deltaTapTime = Time.time - lastTapTime;
            if (deltaTapTime < tapDelay / 1000f)
            {
                deltaDTapTime = Time.time - lastDTapTime;
                if (!iM.Item("Shield").displayed && !iM.Item("Shield").active)
                {
                    if (ScoreManager.Instance.shields > 0)
                    {
                        ScoreManager.Instance.shields--;
                        iM.Item("Shield").active = true;
                    }
                    else
                    {
                        if (deltaDTapTime > tapDelay / 1000f)
                            UIManager.Instance.Notification();
                    }
                }
                lastDTapTime = Time.time;
            }
        }
        lastTapTime = Time.time;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Death")
        {
            if (!iM.Item("Shield").active)
                GameManager.Instance.OnDeath();
            else
            {
                LevelManager.Instance.DeactiveObstaclesOnly(15f);
                shieldPS.Play();
                iM.Item("Shield").active = false;
                AudioManager.Instance.Play("Rescue");
            }
            Vibration.Vibrate(300);
            AudioManager.Instance.Play("Explosion");
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Score")
        {
            ScoreManager.Instance.ScoreUp();
            AudioManager.Instance.Play("Score");
            Vibration.Vibrate(50);
        }

        if (collider.gameObject.tag == "Coin")
        {
            ScoreManager.Instance.CoinScoreUp();
            AudioManager.Instance.Play("Coin");
        }

        if(collider.gameObject.tag == "Scoreboost")
        {
            AudioManager.Instance.Play("PowerUp");
            iM.Item("Scoreboost").collected = true;

            if (iM.Item("Scoreboost").active)
            {
                iM.Item("Scoreboost").timer = iM.Item("Scoreboost").duration;
                if(scoreFactor < 4)
                    scoreFactor++;
            }
            else
            {
                scoreFactor = 1;
                iM.Item("Scoreboost").active = true;
            }
        }

        if(collider.gameObject.tag == "Coinboost")
        {
            AudioManager.Instance.Play("PowerUp");
            iM.Item("Coinboost").collected = true;

            if (iM.Item("Coinboost").active)
            {
                iM.Item("Coinboost").timer = iM.Item("Coinboost").duration;
                if(coinFactor < 4)
                    coinFactor++;
            }
            else
            {
                coinFactor = 1;
                iM.Item("Coinboost").active = true;
            }
        }

        if(collider.gameObject.tag == "Smallboost")
        {
            AudioManager.Instance.Play("PowerUp");
            iM.Item("Smallboost").collected = true;
            GameManager.Instance.SmallBoost();
            iM.Item("Smallboost").active = true;
        }

        if(collider.gameObject.tag == "Bigboost")
        {
            AudioManager.Instance.Play("PowerUp");
            iM.Item("Bigboost").collected = true;
            GameManager.Instance.BigBoost();
            iM.Item("Bigboost").active = true;
        }

        if(collider.gameObject.tag == "Jitterboost")
        {
            AudioManager.Instance.Play("PowerUp");
            iM.Item("Jitterboost").collected = true;
            GameManager.Instance.JitterBoost();
            iM.Item("Jitterboost").active = true;
        }
    }
}
