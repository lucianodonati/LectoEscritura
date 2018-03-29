using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetVowelColor : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().color = getVowelColor(name[0]);
    }

    public static Color getVowelColor(char vowel)
    {
        switch (vowel.ToString().ToLower()[0])
        {
            case 'a':
                return normalizeColor(24, 140, 179);
            case 'e':
                return normalizeColor(227, 66, 122);
            case 'i':
                return normalizeColor(237, 148, 31);
            case 'o':
                return normalizeColor(10, 201, 133);
            case 'u':
                return normalizeColor(131, 103, 230);
        }
        return Color.magenta;
    }

    public static Color normalizeColor(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

}
