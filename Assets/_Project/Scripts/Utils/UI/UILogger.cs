using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UILogger : MonoBehaviour
{
    #region Components
    [SerializeField] private TextMeshProUGUI _logText;
    #endregion

    #region Settings
    [field: Space(5), Header("settings")]
    [SerializeField] private bool _clearLogsOnLoad = true;
    #endregion


    private void Awake()
    {
        if(_clearLogsOnLoad)
            _logText.text = "";
    }

    public void AddToLog(string text)
    {
        _logText.text += text;
    }

    public void AddToLog(string text, bool newLineAfter)
    {
        if(newLineAfter)
            AddToLog(text + "\n");
        else
            AddToLog(text);
    }

    public void AddToLogNewLine(string text)
    {
        AddToLog(text, true);
    }

    public void AddToLog(Component sender, object data)
    {
        if(!(data is string)) return;

        string logText = data as string;

        AddToLogNewLine(logText);
    }
}
