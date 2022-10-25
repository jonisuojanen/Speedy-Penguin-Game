using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_AudioManager : MonoBehaviour
{
    public static F_AudioManager instance;

    private FMOD.Studio.EventInstance Level1Music;
    private FMOD.Studio.EventInstance SlideSFX;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        FMODUnity.RuntimeManager.LoadBank("Main");
        FMODUnity.RuntimeManager.LoadBank("Main.strings");

        Level1Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Level_1_Music");

        SlideSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Gameplay/Slide");
    }

    public void StartLevel1Music()
    {
        Level1Music.start();
    }

    public void StopLevelMusic()
    {
        Level1Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Level1Music.release();
    }

    public void StartSlideSFX()
    {
        SlideSFX.start();
    }

    public void StopSlideSFX()
    {
        SlideSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SlideSFX.release();
    }
}
