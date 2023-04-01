using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioClipPlayer : MonoBehaviour
{

    [SerializeField] AudioSource _audioSource;

    private void Start()
    {
        TextSpeech.TextToSpeech.Instance.onDoneCallback += DoneCallback;
    }

    private void DoneCallback()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "VoiceRespoinces", "VoiceOutput.wav");

        StartCoroutine(LoadAudio(filePath));

    }
    IEnumerator LoadAudio(string filePath)
    {
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.WAV);
        yield return req.SendWebRequest();
        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(req);
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
    