using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    AudioSource source;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playOneShot(string _clipName)
    {

    }
}
