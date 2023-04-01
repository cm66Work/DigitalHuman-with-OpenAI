using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Utils.Events;
using TextSpeech;
using UnityEngine.Android;

public class SpeechToTextController : MonoBehaviour
{



    #region Components
    [field: Space(5), Header("Components")]
    [SerializeField] private TextMeshProUGUI _voiceInputTextIU;
    #endregion

    #region Events
    [field: Space(5), Header("Events")]
    [SerializeField] GameEventSO _userStartedSpeakingEventSO;
    [SerializeField] GameEventSO _userStoppedSpeakingEventSO;
    #endregion

    private bool _messageSent = false; // is true when user finished talking, is used to prevent sending more then one prompt.

    //private List<string> _resultMessagesList = new List<string>();
    private string _lastInputString = "";

    public bool isShowPopupAndroid { get; internal set; }

    #region SetUp

    private void Start()
    {
        isShowPopupAndroid = false;
        SetUp();
    }

    private void SetUp()
    {
        Setting("en-US");
        SpeechToText.Instance.onResultCallback += OnFinalSpeechResult;
#if UNITY_ANDROID
        SpeechToText.Instance.isShowPopupAndroid = isShowPopupAndroid;
        Permission.RequestUserPermission(Permission.Microphone);


#else
        toggleShowPopupAndroid.gameObject.SetActive(false);
#endif
    }

    private void OnEnable()
    {
        SpeechToText.Instance.onResultCallback += OnFinalSpeechResult;
    }

    private void OnDisable()
    {
        SpeechToText.Instance.onResultCallback -= OnFinalSpeechResult;
    }

    /// <summary>
    /// </summary>
    /// <param name="code"></param>
    public void Setting(string code)
    {
        SpeechToText.Instance.Setting(code);
        TextToSpeech.Instance.Setting(code, 1, 1);
    }

    #endregion

    public void OnFinalSpeechResult(string result)
    {
        if(_messageSent) return;

        _voiceInputTextIU.text = result;

        // send prompt to chatGPT
        _userStoppedSpeakingEventSO.Raise(this, result);

        _lastInputString = result;
    }

    // Gets called when Button is healed down .
    public void StartRecording()
    {
#if UNITY_ANDROID
        _voiceInputTextIU.text = "";
        _userStartedSpeakingEventSO.Raise(this, null, true);
        _messageSent = false;
        SpeechToText.Instance.StartRecording("Speak any");
#endif
    }

    // Gets called when finger stops holding button.
    public void StopRecording()
    {
#if UNITY_EDITOR
        OnFinalSpeechResult("Not support in editor.");
#else
        SpeechToText.Instance.StopRecording();
#endif
#if UNITY_IOS
        //loading.SetActive(true);
#endif
    }
}
