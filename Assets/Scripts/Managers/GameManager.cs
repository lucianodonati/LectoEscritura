using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject particleEffects;

    List<ParticleSystem> particles;

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

    float nextLevelTimer = 0, checkSlideEvery = .01f, slideTimer;

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
                print("Next_slide");
                NextSlide();
                break;
            case GameState.PLAYING:
                Playing();
                break;
            case GameState.RIGHT:
                Right();
                break;
            case GameState.NEXT_LEVEL:
                print("NEXT_LEVEL");
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

        if (slideTimer < 0 && currentSlide.IsCorrect())
        {
            slideTimer = checkSlideEvery;
            currentState = GameState.RIGHT;
            foreach (ParticleSystem ps in particles)
                ps.Emit(300);
        }
        else
        {

        }
    }

    void Right()
    {
        nextLevelTimer -= Time.deltaTime;
        slidesToNextLevel--;



        if (nextLevelTimer <= 0)
            currentState = GameState.NEXT_SLIDE;
    }
}
