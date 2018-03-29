using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    List<ParticleSystem> particles;

    Text resultText;

    public SlideManager slideManager;
    Slide currentSlide;
    public int currentSlideFinishedCount;

    public enum GameState
    {
        NONE, INIT, MENU, NEXT_SLIDE, PLAYING, RIGHT
    }

    public GameState currentState;

    /* Slides */
    int slidesPerLevel = 5;
    int slidesToNextLevel;

    float nextLevelTimer = 0, checkSlideEvery = .01f, slideTimer, incorrectTimer;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
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
                if (SceneManager.GetActiveScene().name == "Playing")
                {
                    FindReferences();
                    slideManager.ReadXML();
                    currentState = GameState.NEXT_SLIDE;
                }
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
        }

    }

    void FindReferences()
    {
        slideManager = GameObject.FindObjectOfType<SlideManager>();
        particles = new List<ParticleSystem>(
            GameObject.Find("ParticleEffects").GetComponentsInChildren<ParticleSystem>()
            );
        resultText = GameObject.Find("Result").GetComponent<Text>();
        resultText.gameObject.SetActive(false);

        GameObject.Find("Salir").GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    void Init()
    {
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
        else if (incorrectTimer < 0)
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

    public void StartPlaying()
    {
        slidesToNextLevel = slidesPerLevel;
        SceneManager.LoadScene("Playing");


    }

    public void OpenOptions()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
