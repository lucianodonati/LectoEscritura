using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Syllable : MonoBehaviour
{
    enum ColorState
    {
        NotSelected, Selected, MouseOver, Wrong, Correct
    }

    ColorState oldState, newState;
    static readonly Color[] colors = { Color.blue, Color.cyan, Color.grey, Color.red, Color.green };
    static readonly float duration = 1;
    float lerpTime = 0;

    SoundManager soundManager;

    public InputField text;

    public bool completed, correct;
    string correctText, typedText;

    void Start()
    {
        oldState = newState = ColorState.NotSelected;

        completed = false;
    }

    void Update()
    {
        if (text)
        {
            text.image.color = Color.Lerp(colors[(int)oldState], colors[(int)newState], lerpTime);
            if (lerpTime < 1)
                lerpTime += Time.deltaTime / duration;
        }
    }

    public void Init(bool _alreadyFilled, string _correctSyllable, InputField _inputField, SoundManager _soundManager)
    {
        soundManager = _soundManager;

        text = _inputField;
        text.image.color = colors[(int)newState];

        // Set the name of the gameobject and save it.
        name = correctText = _correctSyllable;
        // If the syllable is already filled set it's text
        if (_alreadyFilled)
        {
            text.text = correctText;
            CorrectBlockSyllable();
        }
        else
        {
            text.onValueChanged.AddListener(SyllableValueChanged);
            text.onEndEdit.AddListener(SyllableFinishedWriting);
        }
    }

    public void CorrectBlockSyllable()
    {
        text.readOnly = correct = completed = true;
        ChangeColorState(ColorState.Correct);

        soundManager.playOneShot("CorrectSyllable");
    }

    bool CheckIsCorrect()
    {
        if (text.text.Length == 0)
            return false;

        if (typedText == correctText)
            correct = true;
        return true;
    }

    void ChangeColorState(ColorState _newState)
    {
        if (!completed)
        {
            oldState = newState;
            newState = _newState;
        }
    }

    public void SyllableValueChanged(string _currentValue)
    {
        ChangeColorState(ColorState.Selected);

        typedText = text.text = _currentValue.ToUpper();

        CheckIsCorrect();
    }

    public void SyllableFinishedWriting(string _currentValue)
    {
        if (!CheckIsCorrect())
            ChangeColorState(ColorState.Wrong);

        // Gets checked every frame by the owner slide
        completed = true;
    }

    void OnMouseOver()
    {
        ChangeColorState(ColorState.MouseOver);
    }

    void OnMouseExit()
    {
        ChangeColorState(ColorState.NotSelected);
    }
}
