using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SlideManager : MonoBehaviour
{
    public const string xmlPath = "/Resources/slides.xml";
    public const string imagesPath = "Images/";

    public bool randomOrder = true;

    SlideContainer sc;

    public PrefabsManager prefabsManager;
    public SoundManager soundManager;

    void Start()
    {

    }

    public void ReadXML()
    {
        string url = Application.dataPath + xmlPath;

        if (Application.isWebPlayer)
        {
            WWW request = new WWW(url);
            while (!request.isDone) ;
            url = request.text;
        }
        sc = SlideContainer.Load(url);
    }

    void WriteXML()
    {
        sc.Save(Application.persistentDataPath + xmlPath);
    }

    public Slide getNextSlide()
    {
        int slideIndex = 0;

        if (!randomOrder)
            slideIndex = Random.Range(0, sc.slides.Count);

        SlideData data = sc.slides[slideIndex];
        Sprite theSprite = Resources.Load<Sprite>(imagesPath + data.slideName);

        Image theImage = prefabsManager.InstantiateImage(theSprite);

        Slide newSlide = prefabsManager.InstantiateSlide();
        newSlide.Init(data.slideName, data.syllables, data.alreadyFilled, theImage, soundManager);

        return newSlide;
    }
}
