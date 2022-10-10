using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_MusicPlayer : MonoBehaviour
{
    public static F_MusicPlayer instance;

    private FMOD.Studio.EventInstance LevelMusic;

    public string LevelEvent;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        LevelMusic = FMODUnity.RuntimeManager.CreateInstance(LevelEvent);
    }

    public void StopLevelMusic()
    {
        LevelMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        LevelMusic.release();
    }

}
