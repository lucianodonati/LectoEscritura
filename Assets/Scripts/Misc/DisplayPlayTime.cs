using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayPlayTime : MonoBehaviour
{
    public Text myText;

    // Update is called once per frame
    void Update()
    {
        myText.text = "Tiempo de juego: " + Time.realtimeSinceStartup.ToString("F2");
    }
}
