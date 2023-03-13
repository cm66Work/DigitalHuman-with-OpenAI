using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Utils.Events;

namespace GPT3DigitalHuman.ChatBot
{
    public class ChatGPTController : MonoBehaviour
    {
        private string _apiKey = "";

        private bool _thinking = false; // is true when chat bot is processing question.

        #region Components

        [SerializeField] private TextAsset _apiKeyFileTextAsset;

        [SerializeField] private GameEventSO _textToSpeachTrigger;
        #endregion


        private ChatGPT3 _chatGPT3;

        private void Awake()
        {
            _apiKey = _apiKeyFileTextAsset.text;
            SetupChatAIP();
        }

        private void SetupChatAIP()
        {
            _chatGPT3 = new ChatGPT3(_apiKey);
        }

        public async void GenerateResponce(Component sender, object data)
        {
            if(_thinking) return;
            _thinking = true;
            if(!(data is string)) return;

            _textToSpeachTrigger.Raise(this, "Sure, let me think.");

            string prompt = data as string;

            string result = "I could not find anything.";

            try
            {
                result = await _chatGPT3.GenerateResponceFromPromot(prompt + " in as few words as possible.");
            }
            catch
            {
                result = "I'm sorry, I cant access the Internet right now. Please try again later.";
            }

            _textToSpeachTrigger.Raise(this, result);
            _thinking = false;
        }

        public void ModelUpdated(Component setnder, object data)
        {
            if(!(data is OpenAI_API.Models.Model))
                return;

            _chatGPT3.CurrentModel = (OpenAI_API.Models.Model)data;
        }

        public void UpdateTemptrature(Component sender, object data)
        {
            if(!(data is double))
                return;

            double newTempVal = (double)data;

            _chatGPT3.CurrentTemprature = newTempVal > 2 ?
                2 : newTempVal < 0 ? 0 : newTempVal;
        }

        public void UpdateTop_p(Component sender, object data)
        {
            if(!(data is double))
                return;

            double newtop_pVal = (double)data;

            _chatGPT3.CurrentTop_P = newtop_pVal > 2 ?
                2 : newtop_pVal < 0 ? 0 : newtop_pVal;
        }
    }
}
