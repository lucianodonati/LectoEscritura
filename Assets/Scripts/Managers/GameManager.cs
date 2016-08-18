using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public SlideManager slideManager;
    Slide currentSlide;
    public int currentSlideFinishedCount;

    public enum GameState
    {
        NONE, INIT, MENU, NEXT_SLIDE, PLAYING, RIGHT, WRONG, NEXT_LEVEL,
    }

    public GameState currentState;


    /* Slides */
    int slidesToNextLevel = 5;

    float nextLevelTimer = 0;

    // Use this for initialization
    void Start()
    {
        currentState = GameState.INIT;
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
            case GameState.WRONG:
                Wrong();
                break;
            case GameState.NEXT_LEVEL:

                break;
            default:
                throw new UnityException("Non-handled state triggered. [GameManager State Machine]");
        }
    }

    void Init()
    {
        slideManager.ReadXML();
        currentState = GameState.MENU;
    }

    void NextSlide()
    {
        currentSlide = slideManager.getNextSlide();
        currentState = GameState.PLAYING;
        nextLevelTimer = 3;
    }

    void Playing()
    {
        if (currentSlide.IsCorrect())
            currentState = GameState.RIGHT;
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

    void Wrong()
    {
    }
}
