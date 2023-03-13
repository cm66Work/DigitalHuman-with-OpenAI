using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using BrainCheck;

public class TextToSpeechController : MonoBehaviour
{
    #region Settings
    private const int LANG_CODE = 0;
    #endregion

    #region Components
    [field: Space(5), Header("Components")]
    [SerializeField] private TextMeshProUGUI _resultTextUI;
    #endregion

    #region SetUp

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        CheckPermisions();
    }

    private void CheckPermisions()
    {
#if UNITY_ANDROID
        SpeechRecognitionBridge.SetupPlugin();
        SpeechRecognitionBridge.checkMicPermission();
        SpeechRecognitionBridge.requestMicPermission();
#endif
    }

    #endregion

    public void ConvertStringToVoice(Component comp, object data)
    {
        if(!(data is string)) return;
        string screiptToRead = data as string;

        _resultTextUI.text = screiptToRead;

        SpeechRecognitionBridge.textToSpeech(screiptToRead, LANG_CODE);
    }
}
