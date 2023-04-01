using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using UnityEngine.Android;
using System.IO;

public class SampleSpeechToText : MonoBehaviour
{
    public bool isShowPopupAndroid = true;
    public GameObject loading;
    public Toggle toggleShowPopupAndroid;
    public InputField inputLocale;
    public InputField inputText;
    public float pitch;
    public float rate;


    public Text txtLocale;
    public Text txtPitch;
    public Text txtRate;


    void Start()
    {

        Setting("en-US");
        loading.SetActive(false);
        SpeechToText.Instance.onResultCallback += OnResultSpeech;
#if UNITY_ANDROID
        SpeechToText.Instance.isShowPopupAndroid = isShowPopupAndroid;
        toggleShowPopupAndroid.isOn = isShowPopupAndroid;
        toggleShowPopupAndroid.gameObject.SetActive(true);
        Permission.RequestUserPermission(Permission.Microphone);


#else
        toggleShowPopupAndroid.gameObject.SetActive(false);
#endif

    }


    public void StartRecording()
    {
#if UNITY_EDITOR
#else
        SpeechToText.Instance.StartRecording("Speak any");
#endif
    }

    public void StopRecording()
    {
#if UNITY_EDITOR
        OnResultSpeech("Not support in editor.");
#else
        SpeechToText.Instance.StopRecording();
#endif
#if UNITY_IOS
        loading.SetActive(true);
#endif
    }
    void OnResultSpeech(string _data)
    {
        //inputText.GetComponent<Text>().text = _data;
        //inputText.textComponent.text = _data;
        inputText.text = _data;

        string filePath = Path.Combine(Application.persistentDataPath,"VoiceRespoinces");
        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        filePath = Path.Combine(filePath, "VoiceOutput.wav");

        if(!File.Exists(filePath))
            File.Create(filePath);


        TextToSpeech.Instance.StartSpeakFile(_data, filePath);
#if UNITY_IOS
        loading.SetActive(false);
#endif
    }
    public void OnClickSpeak()
    {
        TextToSpeech.Instance.StartSpeak(inputText.text);
    }

    /// <summary>
    /// </summary>
    public void  OnClickStopSpeak()
    {
        TextToSpeech.Instance.StopSpeak();
    }

    /// <summary>
    /// </summary>
    /// <param name="code"></param>
    public void Setting(string code)
    {
        txtLocale.text = "Locale: " + code;
        txtPitch.text = "Pitch: " + pitch;
        txtRate.text = "Rate: " + rate;
        SpeechToText.Instance.Setting(code);
        TextToSpeech.Instance.Setting(code, pitch, rate);
    }

    /// <summary>
    /// Button Click
    /// </summary>
    public void OnClickApply()
    {
        Setting(inputLocale.text);
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public void OnToggleShowAndroidPopupChanged(bool value)
    {
        isShowPopupAndroid = value;
        SpeechToText.Instance.isShowPopupAndroid = isShowPopupAndroid;
    }

}
