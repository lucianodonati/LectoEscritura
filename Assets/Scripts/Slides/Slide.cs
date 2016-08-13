using UnityEngine;
using System.Collections.Generic;

public class Slide : MonoBehaviour
{
    public Sprite slideImage;
    List<string> syllables;

    SpriteRenderer slideSpriteRenderer;
    public readonly float slideSize = 0.6f;

    readonly char[] separators = { ' ', ',', '.', ';', '-', '_' };

    void Awake()
    {
        slideSpriteRenderer = GetComponent<SpriteRenderer>();
        if (!slideSpriteRenderer)
            slideSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetSlideSizeAndLocation();
    }

    void SetSlideSizeAndLocation()
    {
        Bounds bounds = slideSpriteRenderer.sprite.bounds;
        float factor = 6 / bounds.size.y;
        transform.localScale = Vector3.one * factor;

        float x = (Screen.width / 2) - (slideSpriteRenderer.bounds.size.x / 2);
        float y = (Screen.height / 2) - (slideSpriteRenderer.bounds.size.y / 2);

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10));
    }

    // Use this for initialization
    public void Init(string _name, string _syllables, Sprite _image)
    {
        name = _name.ToLower();
        _syllables = _syllables.ToLower();
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
