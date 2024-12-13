using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	private float curTimeScale;
    public AudioSource backgroundMusic;
    public float pauseVolume = .4f;
    private float backgroundMusicVolume;

    public void TogglePause()
	{
		if (Time.timeScale != 0)
		{
			curTimeScale = Time.timeScale;
			Time.timeScale = 0;
            backgroundMusicVolume = backgroundMusic.volume;
            backgroundMusic.volume = pauseVolume;
        }
        else
		{
			Time.timeScale = curTimeScale;
            backgroundMusic.volume = backgroundMusicVolume;

        }
	}
}
