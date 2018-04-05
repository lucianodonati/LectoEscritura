using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public enum AudioSourcesTypes
    {
        Music, FX
    }

    [SerializeField]
    AudioSource musicSource = null, FXSource = null;
    [SerializeField]
    Slider FXSlider = null, musicSlider = null;

    float[] volumes = { .1f, .4f };

    public AudioClip[] musicClips;

    public AudioClip[] alphabetClips;

    public List<StringClipPair> FXClips;

    [System.Serializable]
    public struct StringClipPair
    {
        public string theClipName;
        public AudioClip theClip;
    }

    private void Start()
    {
        musicSlider.value = volumes[(int)AudioSourcesTypes.Music] = musicSource.volume =
            PlayerPrefs.GetFloat(AudioSourcesTypes.Music.ToString(), volumes[(int)AudioSourcesTypes.Music]);

        FXSlider.value = volumes[(int)AudioSourcesTypes.FX] = FXSource.volume =
            PlayerPrefs.GetFloat(AudioSourcesTypes.FX.ToString(), volumes[(int)AudioSourcesTypes.FX]);

        ToggleMusic();
    }

    AudioClip getClipFromString(string _string)
    {
        foreach (StringClipPair pair in FXClips)
            if (pair.theClipName == _string)
                return pair.theClip;
        return null;
    }

    void ToggleMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = musicClips[Random.Range(0, musicClips.Length)];
            musicSource.Play();
        }
        else
            musicSource.Stop();

    }

    public void UpdateFXVolume(float _newVolume)
    {
        FXSource.volume = _newVolume;
        volumes[(int)AudioSourcesTypes.FX] = _newVolume;
        PlayerPrefs.SetFloat(AudioSourcesTypes.FX.ToString(), _newVolume);
        PlayerPrefs.Save();
    }
    public void UpdateMusicVolume(float _newVolume)
    {
        musicSource.volume = _newVolume;
        volumes[(int)AudioSourcesTypes.Music] = _newVolume;
        PlayerPrefs.SetFloat(AudioSourcesTypes.Music.ToString(), _newVolume);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            FXSource.PlayOneShot(
                alphabetClips[Random.Range(0, alphabetClips.Length)], volumes[(int)AudioSourcesTypes.FX]);
        }
    }

    public void playOneShot(string _clipName)
    {
        AudioClip theClip = getClipFromString(_clipName);
        if (theClip)
            FXSource.PlayOneShot(theClip, volumes[(int)AudioSourcesTypes.FX]);
    }
}
