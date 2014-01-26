using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
    public AudioClip ballroomMusicClip;

    public static Audio Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void PlayBallromMusic()
    {
    }

    public void PlayBossMusic()
    {
    }

    public void PlayBossHitSFX()
    {
    }


}