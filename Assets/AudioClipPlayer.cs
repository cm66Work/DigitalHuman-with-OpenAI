using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioClipPlayer : MonoBehaviour
{
    private void Start()
    {
        TextSpeech.TextToSpeech.Instance.onDoneCallback += DoneCallback;
    }

    private void DoneCallback()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "VoiceOutput.wav");

        StartCoroutine(LoadAudio(filePath));

    }
    IEnumerator LoadAudio(string filePath)
    {
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.WAV);
        yield return req.SendWebRequest();
        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(req);
        AudioSource aus = GetComponent<AudioSource>();
        aus.clip = audioClip;
        aus.Play();
    }
}
    