using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject particleEffects;

    List<ParticleSystem> particles;

    public GameObject resultTextParent;
    public Text resultText;

    public SlideManager slideManager;
    Slide currentSlide;
    public int currentSlideFinishedCount;

    [SerializeField]
    Text correctText = null, incorrectText = null;
    public static int correct = 0, incorrect = 0;

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
        instance = this;

        currentState = GameState.INIT;
        particles = new List<ParticleSystem>(particleEffects.GetComponentsInChildren<ParticleSystem>());

        correctText.text = (correct = PlayerPrefs.GetInt("correct", 0)).ToString();
        incorrectText.text = (incorrect = PlayerPrefs.GetInt("incorrect", 0)).ToString();
    }

    public void AddCorrect()
    {
        correct++;
        PlayerPrefs.SetInt("correct", correct);
        correctText.text = correct.ToString();
    }

    public void AddIncorrect()
    {
        incorrect++;
        PlayerPrefs.SetInt("incorrect", incorrect);
        incorrectText.text = incorrect.ToString();
    }

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
        else if (incorrectTimer < 0)
        {
            resultTextParent.gameObject.SetActive(false);
        }
    }

    void CorrectSlideFeedback()
    {
        StartCoroutine(EmitDelayed());
        resultTextParent.gameObject.SetActive(true);
        resultText.text = "Correcto !";
        resultText.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    IEnumerator EmitDelayed()
    {
        List<int> indexesUsed = new List<int>();
        for (int i = 0; i < particles.Count; i++)
            indexesUsed.Add(i);

        int randomIndex = 0;
        for (int i = 0; i < particles.Count; i++)
        {
            randomIndex = Random.Range(0, indexesUsed.Count);

            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Random.Range(0, Screen.width),
                            Random.Range(0, Screen.height),
                            0));
            particles[randomIndex].gameObject.transform.position = screenPosition;

            ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = particles[randomIndex].colorOverLifetime;
            colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(
                Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f),
                Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f)
                );

            particles[randomIndex].Play();
            indexesUsed.Remove(randomIndex);
            yield return new WaitForSeconds(Random.Range(0, .15f));
        }
    }

    void IncorrectSlideFeedback()
    {
        incorrectTimer = 1;
        foreach (ParticleSystem ps in particles)
            ps.Emit(300);
        resultTextParent.gameObject.SetActive(true);
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
