using UnityEngine;

public class PlayerAirStripes : MonoBehaviour
{
    public Transform player;
    public TrailRenderer left, right;

    public float aRange;
    float accelerationX, oldXPos;

    void Update()
    {
        accelerationX = (player.transform.position.x - oldXPos) / Time.deltaTime / Time.deltaTime;
        oldXPos = player.transform.position.x;

        if (accelerationX < aRange && accelerationX > -aRange)
        {
            left.emitting = false;
            right.emitting = false;
        }
        else if (accelerationX > aRange)
        {
            left.emitting = true;
            right.emitting = false;
        }
        else if (accelerationX < -aRange)
        {
            left.emitting = false;
            right.emitting = true;
        }

        if(accelerationX > 200f)
        {
            PlayTransition();
        }
        else if(accelerationX < -200f)
        {
            PlayTransition();
        }
    }

    void PlayTransition()
    {
        if (AudioManager.Instance.TransitionNotPlaying())
        {
            if (Random.value > 0.5f)
                AudioManager.Instance.Play("Transition1");
            else
                AudioManager.Instance.Play("Transition2");
        }
    }
}
