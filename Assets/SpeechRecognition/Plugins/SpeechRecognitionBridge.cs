using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


namespace BrainCheck {

	public class SpeechRecognitionBridge {

		static AndroidJavaClass _class;
		static AndroidJavaObject instance 
		{ 
			get 
			{
				return _class.GetStatic<AndroidJavaObject>("instance");
			} 
		}

		public	 static void SetupPlugin () {
			if (_class == null) {
				_class = new AndroidJavaClass ("mayankgupta.com.textToSpeech.TextToSpeechPlugin");
				_class.CallStatic ("_initiateFragment");
			}
		}

		public static void requestMicPermission(){
			SetupPlugin ();
		   	instance.Call("requestMicrophonePermission");
		}

		public static void checkMicPermission(){
			SetupPlugin ();
		   	instance.Call("checkMicrophonePermission");
		}

		public static void textToSpeech(string text, int locale){
			SetupPlugin ();
		   	instance.Call("convertTextToSpeech", text, locale);
		}

		public static void speechToText(){
			SetupPlugin ();
		   	instance.Call("startSpeechToTextConversion");
		}

		public static void speechToTextInHidenModeWithBeepSound(){
			SetupPlugin ();
		   	instance.Call("startHiddenSpeechToTextConversion");
		}

		public static void speechToTextInSilentMode(){
			SetupPlugin ();
		   	instance.Call("startSilentSpeechToTextConversion");
		}

		public static void unmuteSpeakers(){
			SetupPlugin ();
		   	instance.Call("unMuteAudioManager");
		}

		public static void setUnityGameObjectNameAndMethodName(string gameObjectName, string statusMethodName){
			SetupPlugin ();
			Debug.Log("callback");
		   	instance.Call("_setUnityGameObjectNameAndMethodName", gameObjectName, statusMethodName);
		}

	}
}