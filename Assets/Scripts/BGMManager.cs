using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource OverWorldMusic;
    public AudioSource DungeonMusic;
    public AudioSource BossMusic;
    [Range(0, 1)] public float BGMVolume = 0.5f;

    void Awake()
    {
        OverWorldMusic.Play();

    }

    // Start is called before the first frame update
    void Start()
    {
        OverWorldMusic.volume = BGMVolume;
        DungeonMusic.volume = BGMVolume;
        BossMusic.volume = BGMVolume;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOverWorld()
    {
        OverWorldMusic.Play();
        DungeonMusic.Stop();
        BossMusic.Stop();
        OverWorldMusic.loop = true;
    }

    public void PlayDungeon()
    {
        OverWorldMusic.Stop();
        DungeonMusic.Play();
        BossMusic.Stop();
        DungeonMusic.loop = true;
    }

    public void PlayBoss()
    {
        OverWorldMusic.Play();
        DungeonMusic.Stop();
        BossMusic.Play();
        BossMusic.loop = true;
    }
}
