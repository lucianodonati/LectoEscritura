using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PrefabsManager : MonoBehaviour
{
    // Parents
    public Transform canvasParent;
    public Transform slidesParent;

    // Prefabs
    public Slide slidePrefab;
    public Image imagePrefab;

    public Syllable syllablePrefab;
    public InputField textPrefab;

    public Slide InstantiateSlide()
    {
        Slide newSlide = Instantiate<Slide>(slidePrefab);
        newSlide.transform.SetParent(slidesParent);

        return newSlide;
    }
    public Image InstantiateImage(Sprite theSprite)
    {
        Image newImage = Instantiate<Image>(imagePrefab);
        newImage.transform.SetParent(canvasParent, false);

        newImage.sprite = theSprite;

        float imageSize = Screen.height * .6f;
        newImage.rectTransform.anchoredPosition.

        return newImage;
    }
    public InputField InstantiateInputField(string _name)
    {
        InputField newInputField = Instantiate<InputField>(textPrefab);
        newInputField.name = _name;
        newInputField.transform.SetParent(canvasParent, false);

        return newInputField;
    }
    public Syllable InstantiateSyllable(Transform slideParent)
    {
        Syllable newSyllable = Instantiate<Syllable>(syllablePrefab);
        newSyllable.transform.SetParent(slideParent);

        return newSyllable;
    }

}
