using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PrefabsManager : MonoBehaviour
{
    // Parents
    public Transform imagesPanel;
    public Transform inputFieldsPanel;
    public Transform slidesParent;

    // Prefabs
    public Slide slidePrefab;
    public Image imagePrefab;

    public Syllable syllablePrefab;
    public InputField textPrefab;

    public Slide InstantiateSlide()
    {
        Slide newSlide = Instantiate(slidePrefab);
        newSlide.transform.SetParent(slidesParent);

        return newSlide;
    }
    public Image InstantiateImage(Sprite theSprite)
    {
        Image newImage = Instantiate(imagePrefab);
        newImage.transform.SetParent(imagesPanel, false);

        newImage.sprite = theSprite;

        return newImage;
    }
    public InputField InstantiateInputField(string _name)
    {
        InputField newInputField = Instantiate(textPrefab);
        newInputField.name = _name;
        newInputField.transform.SetParent(inputFieldsPanel, false);

        return newInputField;
    }
    public Syllable InstantiateSyllable(Transform slideParent)
    {
        Syllable newSyllable = Instantiate(syllablePrefab);
        newSyllable.transform.SetParent(slideParent);

        return newSyllable;
    }

}
