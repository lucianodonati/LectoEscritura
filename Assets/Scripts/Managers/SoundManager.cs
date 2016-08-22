﻿using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public bool muted = true;
    float masterVolume = .5f;
    public AudioClip[] musicClips;
    public List<AudioSource> audioSources;
    public Dictionary<string, AudioClip> alphabetClips;

    public List<StringClipPair> FXClips;
    //public Dictionary<string, AudioClip> FXClips;

    [System.Serializable]
    public struct StringClipPair
    {
        public string theClipName;
        public AudioClip theClip;
    }


    public enum AudioSourcesTypes
    {
        MusicSource, FXSource, TypeWriterSource
    }
    float[] volumes = { .5f, .5f, .2f };

    readonly string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
        "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    // Use this for initialization
    void Start()
    {
        alphabetClips = new Dictionary<string, AudioClip>(26);
        for (int audioSourcesIndex = 0; audioSourcesIndex < 3; audioSourcesIndex++)
            audioSources.Add(gameObject.AddComponent<AudioSource>());

        Init();
    }

    void Init()
    {
        for (int alphabetKey = 0; alphabetKey < alphabet.Length; alphabetKey++)
        {
            string letter = alphabet[alphabetKey];
            alphabetClips.Add(letter, Resources.Load<AudioClip>("Sounds/Typewriter/" + letter));
        }

        SetMuted();
        ToggleMusic();
    }

    AudioClip getClipFromString(string _string)
    {
        foreach (StringClipPair pair in FXClips)
            if (pair.theClipName == _string)
                return pair.theClip;
        return null;
    }

    public void ToggleMusic()
    {
        AudioSource musicSource = audioSources[(int)AudioSourcesTypes.MusicSource];
        if (musicSource.isPlaying)
        {
            musicSource.clip = musicClips[Random.Range(0, musicClips.Length)];
            musicSource.Play();
        }
        else
            musicSource.Stop();

    }

    public void SetMuted()
    {
        foreach (AudioSource source in audioSources)
            source.mute = muted;
        UpdateMasterVolume(1);
    }

    public void UpdateMasterVolume(float _newVolume)
    {
        masterVolume = Mathf.Clamp(_newVolume, 0, 1);
        UpdateVolume(AudioSourcesTypes.FXSource, volumes[(int)AudioSourcesTypes.FXSource]);
        UpdateVolume(AudioSourcesTypes.MusicSource, volumes[(int)AudioSourcesTypes.MusicSource]);
        UpdateVolume(AudioSourcesTypes.TypeWriterSource, volumes[(int)AudioSourcesTypes.TypeWriterSource]);
    }

    public void UpdateVolume(AudioSourcesTypes _whichOne, float _newVolume)
    {
        volumes[(int)_whichOne] = masterVolume * Mathf.Clamp(_newVolume, 0, 1);
        audioSources[(int)_whichOne].volume = volumes[(int)_whichOne];
    }

    // Update is called once per frame
    void Update()
    {
        foreach (string key in alphabet)
            if (Input.GetKeyDown(key))
                audioSources[(int)AudioSourcesTypes.TypeWriterSource].PlayOneShot(alphabetClips[key]);
    }

    public void playOneShot(string _clipName)
    {
        AudioClip theClip = getClipFromString(_clipName);
        if (theClip)
            audioSources[(int)AudioSourcesTypes.FXSource].PlayOneShot(theClip, volumes[(int)AudioSourcesTypes.FXSource]);
    }
}
