using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using TextSpeech;

public class TextToSpeechController : MonoBehaviour
{
    #region Settings
    private const int LANG_CODE = 0;

    private const string FILENAME = "VoiceOutput.wav";
    #endregion

    #region Components
    [field: Space(5), Header("Components")]
    [SerializeField] private TextMeshProUGUI _resultTextUI;
    #endregion

    #region SetUp


    #endregion

    // Response back from ChatGPT server
    public void ConvertStringToVoice(Component comp, object data)
    {
        if(!(data is string)) return;
        string scriptToRead = data as string;

        _resultTextUI.text = scriptToRead;

        string filePath = Path.Combine(Application.persistentDataPath, "VoiceRespoinces");
        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        filePath = Path.Combine(filePath, "VoiceOutput.wav");

        if(!File.Exists(filePath))
            File.Create(filePath);


        TextToSpeech.Instance.StartSpeakFile(scriptToRead, filePath);
#if UNITY_IOS
        loading.SetActive(false);
#endif

    }
}
