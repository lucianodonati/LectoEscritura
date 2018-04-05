using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SlideManager : MonoBehaviour
{
    [SerializeField]
    Toggle randomOrderToggle = null, accentedMarksToggle = null;

    public const string imagesPath = "Images/";

    private bool randomOrder;

    SlideContainer sc;

    public PrefabsManager prefabsManager;
    public SoundManager soundManager;
    public Transform completedSlidesTransform;

    int currentSlideIndex = 0;

    void Start()
    {
        randomOrderToggle.isOn = randomOrder = PlayerPrefs.GetInt("RandomOrder", 1) == 1 ? true : false;
        accentedMarksToggle.isOn = Syllable.accentedMarksCount = PlayerPrefs.GetInt("accentedMarksCount", 0) == 1 ? true : false;
    }

    public void ToggleRandomOrder(bool newRandomOrder)
    {
        PlayerPrefs.SetInt("RandomOrder", (randomOrder = newRandomOrder) ? 1 : 0);

    }

    public void ToggleAccentedMarksCheck(bool newAccentedMarksCheck)
    {
        PlayerPrefs.SetInt("accentedMarksCount", (Syllable.accentedMarksCount = newAccentedMarksCheck) ? 1 : 0);
    }

    public void ReadXML()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("slides");

        sc = SlideContainer.LoadFromText(textAsset.text);
    }

    void WriteXML()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("slides");
        sc.Save(textAsset.text);
    }

    public Slide getNextSlide()
    {
        if (sc.slides.Count > 0)
        {
            if (randomOrder)
                currentSlideIndex = Random.Range(0, sc.slides.Count);

            SlideData data = sc.slides[currentSlideIndex];
            Sprite theSprite = Resources.Load<Sprite>(imagesPath + data.slideName);

            Image theImage = prefabsManager.InstantiateImage(theSprite);

            Slide newSlide = prefabsManager.InstantiateSlide();
            newSlide.Init(data.slideName, data.syllables, theImage, soundManager, completedSlidesTransform);

            //sc.slides.Remove(data);

            currentSlideIndex++;
            if (currentSlideIndex == sc.slides.Count)
                currentSlideIndex = 0;

            return newSlide;
        }
        return null;
    }
}
