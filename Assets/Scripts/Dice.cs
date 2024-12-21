using UnityEngine;

public class Dice : MonoBehaviour
{
    AudioSource[] audioSources;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void PlayRollingSound()
    {
        if (GetComponent<AudioSource>().isPlaying)
        {
            return;
        }

        audioSources[0].loop = false;
        audioSources[0].Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "floor")
        {
            StopRollingSound();
        }
    }

    private void Update()
    {
        if (transform.position.y > 1.0f && GetComponent<Rigidbody>().angularVelocity.magnitude > 1.0f)
        {
            PlayRollingSound();
        }
    }

    public void StopRollingSound()
    {
        audioSources[0].Stop();
        PlayThrowingSound();
    }

    void PlayThrowingSound()
    {
        GameObject.Find("Main").GetComponent<Main>().PlayThrowingSound();
    }

    //void Update()
    //{
    //    Vector3 pos = transform.localPosition;
    //    bool collisionWithWall = false;
    //    if (pos.x > 2.0f)
    //    {
    //        collisionWithWall = true;
    //        pos.x = 2.0f;
    //    }
    //    else if (pos.x < -2.0f)
    //    {
    //        collisionWithWall = true;
    //        pos.x = -2.0f;
    //    }

    //    if (pos.z > 6.0f)
    //    {
    //        collisionWithWall = true;
    //        pos.z = 6.0f;
    //    }
    //    else if (pos.z < -3.0f)
    //    {
    //        collisionWithWall = true;
    //        pos.z = -3.0f;
    //    }

    //    transform.localPosition = pos;
    //    if (collisionWithWall)
    //    {
    //        TryStopRolling();
    //    }
    //}

    //void TryStopRolling()
    //{
    //    if (transform.position.y < 1.0f)        // floor?
    //    {
    //        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    //        Vector3 velocity = GetComponent<Rigidbody>().linearVelocity;
    //        velocity.y = 0.0f;
    //        float mag = velocity.magnitude;
    //        GetComponent<Rigidbody>().linearVelocity = -transform.position.normalized * mag * 1.2f;
    //    }
    //}
}
