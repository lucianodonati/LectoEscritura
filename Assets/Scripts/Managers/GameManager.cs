using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject particleEffects;

    List<ParticleSystem> particles;

    public Text resultText;

    public SlideManager slideManager;
    Slide currentSlide;
    public int currentSlideFinishedCount;

    public enum GameState
    {
        NONE, INIT, MENU, NEXT_SLIDE, PLAYING, RIGHT, NEXT_LEVEL,
    }

    public GameState currentState;


    /* Slides */
    int slidesToNextLevel = 5;

    float nextLevelTimer = 0, checkSlideEvery = .01f, slideTimer, incorrectTimer;

    // Use this for initialization
    void Start()
    {
        currentState = GameState.INIT;
        particles = new List<ParticleSystem>(particleEffects.GetComponentsInChildren<ParticleSystem>());
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.NONE:
                break;
            case GameState.INIT:
                Init();
                break;
            case GameState.MENU:
                // TEMPORARY
                currentState = GameState.NEXT_SLIDE;
                break;
            case GameState.NEXT_SLIDE:
                NextSlide();
                break;
            case GameState.PLAYING:
                Playing();
                break;
            case GameState.RIGHT:
                Right();
                break;
            case GameState.NEXT_LEVEL:
                break;
        }

    }

    void Init()
    {
        slideManager.ReadXML();
        currentState = GameState.MENU;
    }

    void NextSlide()
    {
        if (currentSlide)
            currentSlide.DestroyVisuals();

        currentSlide = slideManager.getNextSlide();

        if (currentSlide)
            currentState = GameState.PLAYING;
        else
            currentState = GameState.MENU;
        nextLevelTimer = 3;
    }

    void Playing()
    {
        slideTimer -= Time.deltaTime;
        incorrectTimer -= Time.deltaTime;

        if (slideTimer < 0 && currentSlide.IsCorrect())
        {
            slideTimer = checkSlideEvery;
            currentState = GameState.RIGHT;
            CorrectSlideFeedback();
        }
        else if(incorrectTimer<0)
        {
            resultText.gameObject.SetActive(false);
        }
    }

    void CorrectSlideFeedback()
    {
        foreach (ParticleSystem ps in particles)
            ps.Emit(300);
        resultText.gameObject.SetActive(true);
        resultText.text = "Correcto !";
        resultText.color = Color.green;
    }

    void IncorrectSlideFeedback()
    {
        incorrectTimer = 1;
        foreach (ParticleSystem ps in particles)
            ps.Emit(300);
        resultText.gameObject.SetActive(true);
        resultText.text = "Intentalo de nuevo...";
        resultText.color = Color.red;
    }

    void Right()
    {
        nextLevelTimer -= Time.deltaTime;
        slidesToNextLevel--;

        if (nextLevelTimer <= 0)
            currentState = GameState.NEXT_SLIDE;
    }
}
