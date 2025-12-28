using UnityEngine;

public class BGMChangeManager : MonoBehaviour
{
    public AudioSource audioManager;
    public AudioClip BGM1;
    public AudioClip BGM2;
    public float changeTime = 90;
    public CountdownTimer currenttime;
    bool ischange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GetComponent<AudioSource>();
        audioManager.clip = BGM1;
        audioManager.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (currenttime == null || audioManager == null)
        {
            return;
        }
        if (!ischange && currenttime.currentTime <= changeTime)
        {
            audioManager.clip = BGM2;
            audioManager.Play();
            ischange = true;
        }

    }
}
