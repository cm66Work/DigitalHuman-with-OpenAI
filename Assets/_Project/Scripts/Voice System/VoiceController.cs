using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;



namespace GPT3DigitalHuman.Voice
{
    public class VoiceController : MonoBehaviour
    {
        #region Settings
        private const string LANG_CODE = "en-US";

        [SerializeField] private bool _debugMode = false;
        #endregion

        #region Events
        [SerializeField] private Utils.Events.GameEventSO _userStopedSpeaking;
        #endregion

        #region Components
        [SerializeField] private TextMeshProUGUI _buttonTextUI;
        [SerializeField] private TextMeshProUGUI _prmptTextUI;
        [SerializeField] private TextMeshProUGUI _responceTextUI;

        [SerializeField] private GameObject _uiResponceTextGameObject;
        [SerializeField] private GameObject _uiPromptTextGameObject;


        [SerializeField] private TextMeshProUGUI _log;
        #endregion

        #region SetUp

        private void Start()
        {
            SetUp(LANG_CODE);
        }

        private void SetUp(string code)
        {

            _uiResponceTextGameObject.SetActive(false);
            _uiPromptTextGameObject.SetActive(false);
        }


        #endregion 

        #region Text to Speech

        public void StartSpeaking(Component sender, object data)
        {
            if(!(data is string)) return;

            string message = data as string;

            //_log.text += message;
            _responceTextUI.text = message;

            EnableResponceUI();

            //SpeechRecognitionBridge.unmuteSpeakers();
            //SpeechRecognitionBridge.textToSpeech(message, 1);
        }

        #endregion

        #region Speech To Text


        private List<string> _resultMessagesList = new List<string>();
        private void OnFinalSpeechResult(string result)
        {
            _prmptTextUI.text = result;
            _resultMessagesList.Add(result);

            if(!result.Equals("SpeechRecognitionFinished")) return;
            if(!result.Equals("SpeechRecognitionStarted")) return;
            if(!result.Equals("SpeechRecognitionCompleted")) return;

            string prompt = _resultMessagesList[_resultMessagesList.Count ^ 2];

            _userStopedSpeaking.Raise(this, prompt);
            _prmptTextUI.text = prompt;
            StopListening();
        }

        public void StartListening()
        {
            _resultMessagesList.Clear();
            //SpeechRecognitionBridge.setUnityGameObjectNameAndMethodName(this.name, "OnFinalSpeechResult");
            //SpeechRecognitionBridge.speechToTextInHidenModeWithBeepSound();

            //SpeechToText.Instance.StartRecording();
            EnablePromptUI();
            _buttonTextUI.text = "listening...";
            DebugMessage.LogSuccess(this, "Started listening", _debugMode);
        }

        private void StopListening()
        {
            _buttonTextUI.text = "Tap to start listening";
            DebugMessage.LogSuccess(this, "Stopped listening", _debugMode);
        }
        #endregion


        private void EnableResponceUI()
        {
            _uiPromptTextGameObject.SetActive(false);
            _uiResponceTextGameObject.SetActive(true);
        }

        private void EnablePromptUI()
        {
            _uiPromptTextGameObject.SetActive(true);
            _uiResponceTextGameObject.SetActive(false);
        }
    }

    static class DebugMessage
    {
        public static void LogSuccess(Object objectReference, object messsage, bool debug)
        {
            if(!debug) return;

            Utils.DrowsyLogger.LogSuccess(objectReference, messsage);
        }
    }
}

