using UnityEngine;

public class Coin : MonoBehaviour
{
    GameObject model;
    float timeStamp;
    bool lerpToPlayer;
    ParticleSystem ps;

    void Awake()
    {
        if (gameObject.tag == "Coin")
        {
            model = transform.GetChild(0).gameObject;
            ps = CoinManager.Instance.coinParticleSystem;
        }
        else if (gameObject.tag == "Scoreboost")
            ps = ItemManager.Instance.scoreBoost;
        else if (gameObject.tag == "Coinboost")
            ps = ItemManager.Instance.coinBoost;
        else if (gameObject.tag == "Smallboost")
            ps = ItemManager.Instance.smallBoost;
        else if (gameObject.tag == "Bigboost")
            ps = ItemManager.Instance.bigBoost;
        else if (gameObject.tag == "Jitterboost")
            ps = ItemManager.Instance.jitterBoost;
    }

    void Update()
    {
        if (gameObject.tag == "Coin")
            model.transform.Rotate(Vector2.up * CoinManager.Instance.rotSpeed * Time.timeScale, Space.Self);

        if (lerpToPlayer)
        {
            transform.position = Vector2.Lerp(transform.position, Player.Instance.transform.position, (Time.time - timeStamp) * 4f);
            //transform.localScale = Vector2.Lerp(Vector2.one, new Vector2(0.8f, 0.8f), (Time.time - timeStamp) / 0.25f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Magnet")
        {
            timeStamp = Time.time;
            lerpToPlayer = true;
        }

        if (collision.gameObject.tag == "Player")
        {
            lerpToPlayer = false;
            var clone = Instantiate(ps, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Magnet")
            lerpToPlayer = false;
    }
}
