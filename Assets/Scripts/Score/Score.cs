using System.Collections;
using UnityEngine;

public class Score : MonoBehaviour
{
    SpriteRenderer scoreImage;
    Animator scoreWaveAnim;
    float alpha, parentStartX;

    void Start()
    {
        scoreImage = transform.GetChild(0).GetComponent<SpriteRenderer>();
        scoreWaveAnim = transform.GetChild(1).GetComponent<Animator>();
        parentStartX = transform.GetComponentInParent<Obstacle>().transform.position.x;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (transform.GetComponentInParent<Obstacle>().enableScoreAnim)
            {
                StartCoroutine(Animation(scoreImage, 240f, 0.4f));
                scoreWaveAnim.SetTrigger("Score");
            }
        }
    }

    IEnumerator Animation(SpriteRenderer s, float fadeStart, float duration)
    {
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float t = counter / duration;
            alpha = Mathf.Lerp(fadeStart / 255f, 0f, t);
            s.color = new Color(s.color.r, s.color.g, s.color.b, alpha);
            yield return null;
        }
    }
}
