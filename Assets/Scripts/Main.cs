using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject[] dices;

    [SerializeField]
    float force;

    [SerializeField]
    float torque;

    bool isRollingStarted = false;
    List<GameObject> activeDices = new List<GameObject>();
    bool isPlayingThrowingSound = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        Initialize();
        InitializeSound();
    }

    public void Initialize()
    {
        activeDices.Clear();

        for (int i = 0; i < 3; ++i)
        {
            dices[i].SetActive(false);
        }

        for (int i = 0; i < 1; ++i)
        {
            dices[i].SetActive(true);
            activeDices.Add(dices[i]);
        }
    }

    public void SetDice(int count)
    {
        activeDices.Clear();

        for (int i = 0; i < 3; ++i)
        {
            dices[i].SetActive(false);

            float x = Random.Range(-2, 2);
            float y = Random.Range(5, 10);
            dices[i].transform.localPosition = new Vector3(x, y, x);
            dices[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        for (int i = 0; i < count; ++i)
        {
            dices[i].SetActive(true);
            activeDices.Add(dices[i]);

            float rnd = Random.Range(-1.0f, 1.0f);
            dices[i].GetComponent<Rigidbody>().maxAngularVelocity = Mathf.Infinity;
            dices[i].GetComponent<Rigidbody>().AddTorque(new Vector3(rnd, rnd, rnd) * this.torque);
        }
    }


    public void Roll()
    {
        if (this.isRollingStarted)
        {
            return;
        }

        for (int i = 0; i < activeDices.Count; ++i)
        {
            float mag = activeDices[i].GetComponent<Rigidbody>().angularVelocity.sqrMagnitude;
            if (mag >= 1.0f)
            {
                return;
            }
        }

        StartCoroutine(PlayRolling());
    }


    IEnumerator PlayRolling()
    {
        this.isPlayingThrowingSound = false;
        this.isRollingStarted = true;
        for (int i = 0; i < activeDices.Count; ++i)
        {
            Vector3 forceDirection = Vector3.up;
            if (activeDices[i].transform.position.z > 0.0f)
            {
                forceDirection.z = -0.1f;
            }
            else if (activeDices[i].transform.position.z < 0.0f)
            {
                forceDirection.z = 0.1f;
            }

            if (activeDices[i].transform.position.x > 0.0f)
            {
                forceDirection.x = -0.1f;
            }
            else if (activeDices[i].transform.position.x < 0.0f)
            {
                forceDirection.x = 0.1f;
            }

            activeDices[i].GetComponent<Rigidbody>().AddForce(forceDirection * this.force);
        }

        while (true)
        {
            bool onAir = true;
            for (int i = 0; i < activeDices.Count; ++i)
            {
                if (activeDices[i].transform.position.y < 1.0f)
                {
                    onAir = false;
                }
            }

            if (onAir)
            {
                break;
            }

            yield return null;
        }

        for (int i = 0; i < activeDices.Count; ++i)
        {
            float rnd = Random.Range(1.0f, 2.0f);
            int dir = Random.Range(0, 100);
            if (dir < 50)
            {
                rnd *= -1.0f;
            }

            activeDices[i].GetComponent<Rigidbody>().maxAngularVelocity = Mathf.Infinity;
            activeDices[i].GetComponent<Rigidbody>().AddTorque(new Vector3(rnd, rnd, rnd) * this.torque);
        }

        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            bool isRollingFinished = true;
            for (int i = 0; i < activeDices.Count; ++i)
            {
                float mag = activeDices[i].GetComponent<Rigidbody>().angularVelocity.sqrMagnitude;
                if (mag >= 1.0f)
                {
                    isRollingFinished = false;
                }
            }

            if (isRollingFinished)
            {
                this.isRollingStarted = false;
                break;
            }

            yield return null;
        }
    }

    public void PlayThrowingSound()
    {
        if (isPlayingThrowingSound)
        {
            return;
        }

        //GetComponent<AudioSource>().mute = !IsSoundOn();
        GetComponent<AudioSource>().Play();
        isPlayingThrowingSound = true;
    }

    public void InitializeSound()
    {
        int sound = PlayerPrefs.GetInt("savedata.sound");
        if (sound == 0)
        {
            AudioListener.volume = 0.0f;
        }
        else
        {
            AudioListener.volume = 1.0f;
        }
    }

    public void ToggleSound()
    {
        int sound = PlayerPrefs.GetInt("savedata.sound");
        if (sound == 0)
        {
            // on
            PlayerPrefs.SetInt("savedata.sound", 1);
            AudioListener.volume = 1.0f;
        }
        else
        {
            // off
            PlayerPrefs.SetInt("savedata.sound", 0);
            AudioListener.volume = 0.0f;
        }
    }

    public bool IsSoundOn()
    {
        return PlayerPrefs.GetInt("savedata.sound") == 1;
    }

    void OnEnable()
    {
        ShakeDetector.OnShake += HandleShake;
    }

    void OnDisable()
    {
        ShakeDetector.OnShake -= HandleShake;
    }

    void HandleShake()
    {
        // 흔들림에 대한 추가 처리 로직
        Roll();
    }
}
