using UnityEngine;
using System.Collections;
using System.IO;

namespace BrainCheck {


	public class UnityReceiveMessages : MonoBehaviour {
		public static UnityReceiveMessages Instance;
		public TextMesh tMesh;
		public TextMesh tMesh1;

		void Awake(){
			Instance = this;
		}

		// Use this for initialization
		void Start () {
		}

		// Update is called once per frame
		void Update () {
		
		}
		public void CallbackMethod(string messages){
			if (messages.Equals("SpeechRecognitionFinished")) {
					tMesh1.text = messages;
				} else {
					tMesh.text = messages;
					tMesh1.text = "";
				}
			
		}
	}
}
