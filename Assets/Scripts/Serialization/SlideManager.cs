using UnityEngine;
using System.Collections.Generic;

public class SlideManager : MonoBehaviour
{
    public const string xmlPath = "/Resources/slides.xml";
    public const string imagesPath = "Images/";

    SlideContainer sc;

    List<Slide> slides = new List<Slide>();

    void Start()
    {
        //ReadXML();
    }

    // Use this for initialization
    void ReadXML()
    {

        // Recognize which platform we're running the application on. (Paths are different)
        //if (Application.platform == RuntimePlatform.WebGLPlayer)
        //    sc = SlideContainer.Load(Application.absoluteURL + xmlPath);
        //else
        //    sc = SlideContainer.Load(Application.dataPath + xmlPath);

        sc = SlideContainer.Load(Application.dataPath + xmlPath);

        foreach (SlideData slide in sc.slides)
        {
            Sprite theSprite = Resources.Load<Sprite>(imagesPath + slide.slideName);

            GameObject newGameObject = new GameObject();
            Slide newSlide = newGameObject.AddComponent<Slide>();
            newSlide.Init(slide.slideName, slide.syllables, theSprite);

            slides.Add(newSlide);
        }
    }

    void WriteXML()
    {
        sc.Save(Application.persistentDataPath + xmlPath);
    }
}
