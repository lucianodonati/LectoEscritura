﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Syllable : MonoBehaviour
{
    enum SyllableState
    {
        None, Init, Typing, Wrong, Shake, Correct, Transition, Done
    }

    public static bool accentedMarksCount = false;
    static readonly char[] vocals = { 'a', 'e', 'i', 'o', 'u' };
    static readonly char[] accentedMarks = { 'á', 'é', 'í', 'ó', 'ú' };

    SyllableState currentState, transitionTo;
    static readonly float colorLerpDuration = .5f;
    float colorLerpTimer = 0;

    Slide parentSlide;
    SoundManager soundManager;

    public InputField text;

    Color lerpTo;

    public bool completed, correct;
    string correctText, typedText;

    bool shouldReFocus = false;

    bool initialized = false;

    bool shakeToggle = true;
    float shakeSpeed = 300;

    bool toUpper = false;

    void Awake()
    {
        currentState = SyllableState.Init;
    }

    void Start()
    {
        parentSlide = GetComponentInParent<Slide>();

    }

    void Update()
    {
        if (initialized)
        {
            switch (currentState)
            {
                case SyllableState.Init:
                    text.transform.localEulerAngles = Vector3.zero;
                    text.interactable = true;
                    completed = correct = false;
                    colorLerpTimer = 0;
                    text.image.color = Color.white;
                    text.text = "";
                    currentState = SyllableState.None;

                    if(shouldReFocus)
                    {
                        parentSlide.ReFocusCurrent();
                        shouldReFocus = false;
                    }
                    break;
                case SyllableState.Typing:
                    Typing();
                    break;
                case SyllableState.Wrong:
                    Wrong();
                    break;
                case SyllableState.Shake:
                    Shaking();
                    break;
                case SyllableState.Correct:
                    Correct();
                    break;
                case SyllableState.Transition:
                    Transition();
                    break;
                case SyllableState.Done:
                    Done();
                    break;
            }
        }
    }

    public void Init(bool _alreadyFilled, string _correctSyllable, bool _toUpper, InputField _inputField, SoundManager _soundManager)
    {
        toUpper = _toUpper;

        soundManager = _soundManager;

        text = _inputField;
        text.image.color = lerpTo;

        // Set the name of the gameobject and save it.
        name = correctText = toUpper ? _correctSyllable.ToUpper() : _correctSyllable.ToLower();

        // If the syllable is already filled set it's text
        if (_alreadyFilled)
        {
            currentState = SyllableState.Done;
            text.text = correctText;
            correct = true;
        }
        else
        {
            text.onValueChanged.AddListener(SyllableValueChanged);
            text.onEndEdit.AddListener(SyllableFinishedWriting);
        }
        initialized = true;
    }

    void Done()
    {
        text.interactable = false;
        text.readOnly = completed = true;
        text.image.color = Color.green;
    }

    bool CheckIsCorrect()
    {
        if (text.text.Length == 0)
            return false;

        if (typedText == correctText)
        {
            correct = true;
            return true;
        }
        else if (!accentedMarksCount)
        {
            char[] correctWithoutAccents = correctText.ToLower().ToCharArray();
            for (int i = 0; i < correctWithoutAccents.Length; i++)
            {
                for (int j = 0; j < accentedMarks.Length; j++)
                {
                    if (correctWithoutAccents[i] == accentedMarks[j])
                    {
                        correctWithoutAccents[i] = vocals[j];
                        break;
                    }
                }
            }
            if (typedText.ToLower() == new string(correctWithoutAccents))
            {
                correct = true;
                return true;
            }
        }
        return false;
    }

    void Typing()
    {
    }

    void Transition()
    {
        text.interactable = false;

        text.image.color = Color.Lerp(text.image.color, lerpTo, colorLerpTimer);
        if (colorLerpTimer < 1)
            colorLerpTimer += Time.deltaTime / colorLerpDuration;
        else
            currentState = transitionTo;
    }

    void Correct()
    {
        soundManager.playOneShot("Correct");
        lerpTo = Color.green;
        transitionTo = SyllableState.Done;
        currentState = SyllableState.Transition;
        parentSlide.SyllableCorrect();
        parentSlide.FocusNextSyllable();
    }

    void Wrong()
    {
        lerpTo = Color.red;
        shouldReFocus = true;
        soundManager.playOneShot("Incorrect");
        transitionTo = SyllableState.Init;
        currentState = SyllableState.Shake;
        parentSlide.ReFocusCurrent();
    }

    void Shaking()
    {
        Transition();

        int toggleInt = 1;

        if (!shakeToggle)
            toggleInt = -1;

        text.transform.Rotate(Vector3.forward, shakeSpeed * toggleInt * Time.deltaTime);

        if (text.transform.eulerAngles.z < 180)
        {
            if (text.transform.eulerAngles.z > 5)
                shakeToggle = false;
        }
        else if (text.transform.eulerAngles.z - 360 < -5)
            shakeToggle = true;
    }

    public void SyllableValueChanged(string _currentValue)
    {
        typedText = text.text = toUpper ? _currentValue.ToUpper() : _currentValue.ToLower();
        currentState = SyllableState.Typing;
    }

    public void SyllableFinishedWriting(string _currentValue)
    {
        if (text.text.Length > 0)
        {
            if (CheckIsCorrect())
            {
                GameManager.instance.AddCorrect();
                currentState = SyllableState.Correct;
            }
            else
            {
                GameManager.instance.AddIncorrect();
                currentState = SyllableState.Wrong;
            }
        }
        else
            colorLerpTimer = 0;
    }

    public void DestroyVisuals()
    {
        Destroy(text.gameObject);
        name = "[C]" + name;
        //transform.SetParent(completedSlidesTransform);
        gameObject.SetActive(false);
    }
}
