using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayPlayTime : MonoBehaviour
{
    public Text myText;
    int[] time = new int[3];

    void Update()
    {
        time[1] = (int)Time.realtimeSinceStartup / 60;
        time[0] = (int)Time.realtimeSinceStartup - time[1] * 60;

        string playedFor = "Has jugado por ";

        if (time[1] < 10)
            playedFor += '0';
        playedFor += time[1];

        playedFor += ':';

        if (time[0] < 10)
            playedFor += '0';
        playedFor += time[0];

        myText.text = playedFor;
    }
}
