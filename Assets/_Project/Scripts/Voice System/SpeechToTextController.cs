using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Utils.Events;
using BrainCheck;

public class SpeechToTextController : MonoBehaviour
{



    #region Components
    [field: Space(5), Header("Components")]
    [SerializeField] private TextMeshProUGUI _voiceInputTextIU;
    [SerializeField] private TextMeshProUGUI _buttonTextUI;
    #endregion

    #region Events
    [field: Space(5), Header("Events")]
    [SerializeField] GameEventSO _userStartedSpeakingEventSO;
    [SerializeField] GameEventSO _userStoppedSpeakingEventSO;
    #endregion

    private bool _messageSent = false; // is true when user finished talking, is used to prevent sending more then one prompt.

    //private List<string> _resultMessagesList = new List<string>();
    private string _lastInputString = "";

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


    public void OnFinalSpeechResult(string result)
    {
        if(_messageSent) return;

        _voiceInputTextIU.text = result;
        //_resultMessagesList.Add(result);

        if(result.Equals("SpeechRecognitionFinished"))
        {
            StopListening();
            string prompt = _lastInputString; //_resultMessagesList[_resultMessagesList.Count ^ 2];

            _voiceInputTextIU.text = prompt;

            _messageSent = true;
            _userStoppedSpeakingEventSO.Raise(this, prompt);


            return;
        }
        _lastInputString = result;
        //if(result.Equals("SpeechRecognitionStarted")) return;
        //if(result.Equals("SpeechRecognitionCompleted")) return;

    }



    public void StartListening()
    {
        CheckPermisions();
        _voiceInputTextIU.text = "";
        _userStartedSpeakingEventSO.Raise(this, null);
        _messageSent = false;
        //_resultMessagesList.Clear();
        SpeechRecognitionBridge.setUnityGameObjectNameAndMethodName(this.name, "OnFinalSpeechResult");
        SpeechRecognitionBridge.speechToTextInHidenModeWithBeepSound();

        _buttonTextUI.text = "listening...";
    }

    private void StopListening()
    {
#if UNITY_EDITOR
        _buttonTextUI.text = "Not support in editor.";
#else
        _buttonTextUI.text = "Tap to start listening";
#endif
    }
}
