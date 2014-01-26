using UnityEngine;
using System.Collections;
using System;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource ballroomMusicSrc;
    [SerializeField] private AudioSource bossMusicSrc;
    [SerializeField] private AudioSource hitSFX;
    
    [SerializeField]
    private AudioClip knifeHitSound;

    [SerializeField]
    private AudioClip knifeMissSound;

    [SerializeField]
    private AudioClip mayorDeathSound;

    public static Audio Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }

    void Start()
    {
        PlayBallromMusic();
    }

    public void PlayBallromMusic()
    {
        ballroomMusicSrc.volume = 1f;
        ballroomMusicSrc.Play();
    }

    public void FadeOutBallroomMusic(float time)
    {
        StartCoroutine(FadeOutAudio(ballroomMusicSrc, time));
    }

    public void PlayBossMusic()
    {
        bossMusicSrc.Play();
    }

    public void PlayKnifeHit()
    {
        hitSFX.PlayOneShot(knifeHitSound);
    }

    public void PlayKnifeMiss()
    {
        hitSFX.PlayOneShot(knifeMissSound);
    }

    public void PlayMayorDeath()
    {
        hitSFX.PlayOneShot(mayorDeathSound);
    }

    public void FadeOutBossMusic(float time)
    {
        FadeOutAudio(bossMusicSrc, time);
    }

    private IEnumerator FadeOutAudio(AudioSource src, float time)
    {
        float currentDB = LinearToDecibel(src.volume);
        float decRate = (currentDB - (-200)) / time;

        //Debug.LogError("decrate is " + decRate + "   currentDB is " + currentDB);

        while (currentDB > -200) {
            currentDB -= decRate * Time.deltaTime;
            src.volume = DecibelToLinear(currentDB);
            yield return null;
        }
        src.volume = 0f;
        src.Stop();
    }

    public static float DecibelToLinear(float db, float maxDB = 200)
    {
        return (float)Math.Pow(10, (db / (maxDB / 10)));
    }

    public static float LinearToDecibel(float s, float maxDB = 200)
    {
        return (float)((maxDB / 10) * Math.Log10(s));
    }
}