using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Syllable : MonoBehaviour
{
    enum ColorState
    {
        NotSelected, Selected, Wrong, Correct
    }

    ColorState oldState, newState;
    static readonly Color[] colors = { Color.white, Color.cyan, Color.red, Color.green };
    static readonly float duration = .8f;
    float lerpTime = 0;

    SoundManager soundManager;
    Transform completedSlidesTransform;

    public InputField text;

    public bool completed, correct;
    string correctText, typedText;

    void Awake()
    {
        oldState = newState = ColorState.NotSelected;
        completed = false;
    }

    void Update()
    {
        if (text && !oldState.Equals(newState))
        {
            text.image.color = Color.Lerp(colors[(int)oldState], colors[(int)newState], lerpTime);
            if (lerpTime < 1)
                lerpTime += Time.deltaTime / duration;
            else if (correct && completed)
            {
                text.interactable = false;
            }
        }
    }

    public void Init(bool _alreadyFilled, string _correctSyllable, InputField _inputField, SoundManager _soundManager, Transform _completedSlidesTransform)
    {
        soundManager = _soundManager;

        completedSlidesTransform = _completedSlidesTransform;

        text = _inputField;
        text.image.color = colors[(int)newState];

        // Set the name of the gameobject and save it.
        name = correctText = _correctSyllable;
        // If the syllable is already filled set it's text
        if (_alreadyFilled)
        {
            ChangeColorState(ColorState.Correct);
            text.text = correctText;
            text.readOnly = correct = completed = true;
        }
        else
        {
            text.onValueChanged.AddListener(SyllableValueChanged);
            text.onEndEdit.AddListener(SyllableFinishedWriting);
        }
    }

    public void CorrectBlockSyllable()
    {
        ChangeColorState(ColorState.Correct);
        text.readOnly = correct = completed = true;

        soundManager.playOneShot("CorrectSyllable");
    }

    bool CheckIsCorrect()
    {
        if (text.text.Length == 0)
            return false;

        if (typedText == correctText)
        {
            correct = true;
            CorrectBlockSyllable();
            return true;
        }
        return false;
    }

    void ChangeColorState(ColorState _newState)
    {
        oldState = newState;
        newState = _newState;
    }

    public void SyllableValueChanged(string _currentValue)
    {
        soundManager.playRepeat("Typing");
        ChangeColorState(ColorState.Selected);
        typedText = text.text = _currentValue.ToUpper();
    }

    public void SyllableFinishedWriting(string _currentValue)
    {
        if (!CheckIsCorrect() && text.text.Length > 0)
        {
            ChangeColorState(ColorState.Wrong);
            soundManager.playOneShot("InCorrectSyllable");
        }
        else
            lerpTime = 0;
    }

    public void DestroyVisuals()
    {
        Destroy(text.gameObject);
        name = "[C]" + name;
        //transform.SetParent(completedSlidesTransform);
        gameObject.SetActive(false);
    }
}
