using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrainCheck {


	public enum SpeechrecognitionOption 
	{
	  textToSpeech,
	  speechToText,
	  setUpPlugin,
	  speechToTextSilentMode,
	  unmuteSpeakers,
	  checkMicPermission,
	  requestMicPermission,
	  speechToTextInHidenModeWithBeepSound
	}

	public class DemoScript : MonoBehaviour
	{
		public SpeechrecognitionOption myOption;
		public string textToConvet;
		string gameObjectName = "UnityReceiveMessage";
		string statusMethodName = "CallbackMethod";

		void OnMouseUp() {
	    	StartCoroutine(BtnAnimation());
	 	}

	 	private IEnumerator BtnAnimation()
	    {
	    	Vector3 originalScale = gameObject.transform.localScale;
	        gameObject.transform.localScale = 0.9f * gameObject.transform.localScale;
	        yield return new WaitForSeconds(0.2f);
	        gameObject.transform.localScale = originalScale;
	        ButtonAction();
	    }

	    private void ButtonAction() {
	    	BrainCheck.SpeechRecognitionBridge.setUnityGameObjectNameAndMethodName(gameObjectName, statusMethodName);
			switch(myOption) 
			{
				case SpeechrecognitionOption.requestMicPermission:
			      BrainCheck.SpeechRecognitionBridge.requestMicPermission();
			      break;
			    case SpeechrecognitionOption.checkMicPermission:
			      BrainCheck.SpeechRecognitionBridge.checkMicPermission();
			      break;
				case SpeechrecognitionOption.setUpPlugin:
			      BrainCheck.SpeechRecognitionBridge.SetupPlugin();
			      break;
			    case SpeechrecognitionOption.textToSpeech:
			      BrainCheck.SpeechRecognitionBridge.textToSpeech(textToConvet, 0);  // 0 is for default locale.
			      break;
			    case SpeechrecognitionOption.speechToText:
			      BrainCheck.SpeechRecognitionBridge.speechToText();
			      break;
			    case SpeechrecognitionOption.speechToTextSilentMode:
			      BrainCheck.SpeechRecognitionBridge.speechToTextInSilentMode();
			      break;
			    case SpeechrecognitionOption.unmuteSpeakers:
			      BrainCheck.SpeechRecognitionBridge.unmuteSpeakers();
			      break;
			    case SpeechrecognitionOption.speechToTextInHidenModeWithBeepSound:
			      BrainCheck.SpeechRecognitionBridge.speechToTextInHidenModeWithBeepSound();
			      break;
			}
	    }
	}
}