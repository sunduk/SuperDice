using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private void Start()
    {
        Main main = GameObject.Find("Main").GetComponent<Main>();
        bool soundOn = main.IsSoundOn();
        transform.Find("On").gameObject.SetActive(soundOn);
        transform.Find("Off").gameObject.SetActive(!soundOn);
    }

    public void OnClick()
    {
        Main main = GameObject.Find("Main").GetComponent<Main>();
        main.ToggleSound();

        bool soundOn = main.IsSoundOn();
        transform.Find("On").gameObject.SetActive(soundOn);
        transform.Find("Off").gameObject.SetActive(!soundOn);
    }
}
