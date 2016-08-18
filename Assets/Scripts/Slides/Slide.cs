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
    Syllable currentSyllable;

    bool finishedWriting;

    public readonly float slideSize = 0.6f;

    readonly char[] separators = { ' ', ',', '.', ';', '-', '_' };

    SoundManager soundManager;

    void Start()
    {
    }

    void Awake()
    {
        prefabsManager = FindObjectOfType<PrefabsManager>();

        syllablesFilledIndex = 0;

        finishedWriting = false;
        syllables = new List<Syllable>();

    }

    public void Init(string _name, string _syllables, string _alreadyFilled, Image _image, SoundManager _soundManager)
    {
        soundManager = _soundManager;

        name = _name;
        _syllables = _syllables.ToUpper();

        slideImage = _image;
        slideImage.name = name + " Image";

        // Parse the strings and create lists for the syllables and the filled ones
        syllablesStrings = new List<string>(_syllables.Split(separators));
        List<string> filledSyllablesStrings = new List<string>(_alreadyFilled.Split(separators));

        // Input data .xml mistakes should be handled here

        syllablesFilled = StringListToBooleanList(filledSyllablesStrings);

        for (int syllableIndex = 0; syllableIndex < syllablesStrings.Count; syllableIndex++)
        {
            InputField inputField = prefabsManager.InstantiateInputField(syllablesStrings[syllableIndex].ToUpper());
            Syllable currentSyll = prefabsManager.InstantiateSyllable(transform);
            currentSyll.Init(syllablesFilled[syllableIndex], syllablesStrings[syllableIndex].ToUpper(), inputField, soundManager);
            syllables.Add(currentSyll);
        }


        currentSyllable = getNextSyllableToComplete();
    }

    private List<bool> StringListToBooleanList(List<string> filledSyllablesStrings)
    {
        List<bool> booleanList = new List<bool>();

        foreach (string theString in filledSyllablesStrings)
        {
            if (theString.Contains("1") || theString.Contains("+") || theString.Contains("si") || theString.Contains("yes") || theString.Contains("true"))
                booleanList.Add(true);
            else
                booleanList.Add(false);
        }

        return booleanList;
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

        EventSystem.current.SetSelectedGameObject(nextSyll.text.gameObject, null);
        nextSyll.text.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    void Update()
    {
        // Finished writing the syllable
        if (currentSyllable.completed)
        {
            // Typed it right
            if (currentSyllable.correct)
            {
                currentSyllable.CorrectBlockSyllable();
            }
            // Mistakes were made =(
            else
            {

            }
        }
    }

    public bool IsCorrect()
    {
        if (!finishedWriting)
            return false;

        bool correct = true;

        for (int syllableIndex = 0; syllableIndex < syllablesStrings.Count && correct; syllableIndex++)
            correct = syllables[syllableIndex].correct;

        return correct;
    }
}
