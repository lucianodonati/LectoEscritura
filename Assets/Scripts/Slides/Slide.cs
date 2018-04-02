using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Slide : MonoBehaviour
{
    public PrefabsManager prefabsManager;

    Image slideImage;

    public List<bool> syllablesFilled;
    int syllablesFilledIndex;

    public List<string> syllablesStrings;
    List<Syllable> syllables;
    //Syllable currentSyllable;

    public readonly float slideSize = 0.6f;

    readonly char[] separators = { ' ', ',', '.', ';', '-', '_' };

    SoundManager soundManager;
    Transform completedSlidesTransform;

    public float timeSpentOnThis;

    void Start()
    {
        timeSpentOnThis = 0;
    }

    void Awake()
    {
        prefabsManager = FindObjectOfType<PrefabsManager>();

        syllablesFilledIndex = 0;

        syllables = new List<Syllable>();

    }

    public void Init(string _name, string _syllables, Image _image, SoundManager _soundManager, Transform _completedSlidesTransform)
    {
        bool toUpper = Random.value > .5f;

        soundManager = _soundManager;

        completedSlidesTransform = _completedSlidesTransform;

        name = _name;

        slideImage = _image;
        slideImage.name = name + " Image";

        // Parse the strings and create lists for the syllables and the filled ones
        syllablesStrings = new List<string>(_syllables.Split(separators));

        // Input data .xml mistakes should be handled here

        syllablesFilled = RandomizeFilledOnes(syllablesStrings.Count);

        bool firstOne = true;
        for (int syllableIndex = 0; syllableIndex < syllablesStrings.Count; syllableIndex++)
        {

            InputField inputField = prefabsManager.InstantiateInputField(syllablesStrings[syllableIndex]);
            Syllable currentSyll = prefabsManager.InstantiateSyllable(transform);
            currentSyll.Init(syllablesFilled[syllableIndex], syllablesStrings[syllableIndex], toUpper, inputField, soundManager);
            syllables.Add(currentSyll);

            // Focus on the first syllable to complete
            if (!syllablesFilled[syllableIndex] && firstOne)
            {
                if (!Application.isMobilePlatform)
                    FocusOnInputField(inputField);
                firstOne = false;
            }
        }

        //currentSyllable = getNextSyllableToComplete();
    }

    private List<bool> RandomizeFilledOnes(int syllableCount)
    {
        List<bool> booleanList = new List<bool>();

        int filledOnesCount = 0;
        List<int> indexesUsed = new List<int>();

        if (syllableCount == 2)
        {
            filledOnesCount = 1;
        }
        else if (syllableCount > 2)
        {
            filledOnesCount = (int)(syllableCount * .5f);
            // Randomize rounding
            if (Random.value > .5f)
                filledOnesCount++;
        }

        for (int i = 0; i < filledOnesCount; i++)
        {
            int randomIndex = -1;
            do
            {
                randomIndex = Random.Range(0, syllableCount);
            } while (IsIndexPicked(randomIndex, indexesUsed));
            indexesUsed.Add(randomIndex);
        }

        for (int i = 0; i < syllableCount; i++)
        {
            if (IsIndexPicked(i, indexesUsed))
                booleanList.Add(true);
            else
                booleanList.Add(false);
        }

        return booleanList;
    }

    bool IsIndexPicked(int index, List<int> pickedOnes)
    {
        for (int i = 0; i < pickedOnes.Count; i++)
        {
            if (pickedOnes[i] == index)
                return true;
        }
        return false;
    }

    Syllable getNextSyllableToComplete()
    {
        Syllable nextSyllable = null;
        for (int toCompleteIndex = syllablesFilledIndex; nextSyllable == null && toCompleteIndex < syllables.Count; toCompleteIndex++)
        {
            if (syllablesFilled[toCompleteIndex])
            {
                syllablesFilledIndex = toCompleteIndex;
                nextSyllable = syllables[toCompleteIndex];
            }
        }
        if (null != nextSyllable)
            nextSyllable.enabled = true;
        return nextSyllable;
    }

    public override string ToString()
    {
        string toString = name + ": ";
        foreach (string syllable in syllablesStrings)
            toString += syllable + ",";

        return toString.TrimEnd(',');
    }


    public void FocusNextSyllable()
    {
        Syllable nextSyll = getNextSyllableToComplete();
        if (null != nextSyll)
            FocusOnInputField(nextSyll.text);
    }

    void FocusOnInputField(InputField objectToFocusOn)
    {
        EventSystem.current.SetSelectedGameObject(objectToFocusOn.gameObject, null);
        objectToFocusOn.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    void Update()
    {
        // Finished writing the syllable
        if (IsCorrect())
        {

        }
        else
            timeSpentOnThis += Time.deltaTime;
    }

    public void DestroyVisuals()
    {
        foreach (Syllable syllable in syllables)
            syllable.DestroyVisuals();
        Destroy(slideImage.gameObject);
        name = "[C]" + name;
        transform.SetParent(completedSlidesTransform);
        gameObject.SetActive(false);
    }

    public bool IsCorrect()
    {
        bool correct = true;

        for (int syllableIndex = 0; syllableIndex < syllablesStrings.Count && correct; syllableIndex++)
            correct = syllables[syllableIndex].correct && syllables[syllableIndex].completed;

        return correct;
    }
}
