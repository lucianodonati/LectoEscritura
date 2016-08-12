using UnityEngine;
using System.Collections.Generic;

public class Slide : MonoBehaviour
{
    public Sprite slideImage;
    List<string> syllables;

    SpriteRenderer slideSpriteRenderer;

    readonly char[] separators = { ' ', ',', '.', ';', '-', '_' };

    void Awake()
    {
        slideSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    // Use this for initialization
    public void Init(string _name, string _syllables, Sprite _image)
    {
        name = _name;
        slideImage = _image;

        slideSpriteRenderer = GetComponent<SpriteRenderer>();
        slideSpriteRenderer.sprite = slideImage;

        syllables = new List<string>(_syllables.Split(separators));

        //print(this);
    }

    public override string ToString()
    {
        string toString = name + ": ";
        foreach (string syllable in syllables)
            toString += syllable + ",";

        return toString.TrimEnd(',');
    }

    // Update is called once per frame
    void Update()
    {

    }
}
