using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using System.Threading.Tasks;

namespace GPT3DigitalHuman.ChatBot
{
    public class ChatGPT3
    {
        #region Settings
        private Model _currentModel = Model.CurieText;
        [Range(0,2)] private double _currentTemprature = 0.1;
        [Range(0,1)] private double _top_P = 0.1;

        public Model CurrentModel { get { return _currentModel; } set { _currentModel = value; }  }
        public double CurrentTemprature { get { return _currentTemprature; } set { _currentTemprature = value; }  }
        public double CurrentTop_P { get { return _top_P; } set { _top_P = value; } }
        #endregion

        private OpenAIAPI _aip;

        public ChatGPT3(string aipKey)
        {
            if(aipKey.Length <= 0)
            {
                Debug.LogError("ERROR:: -- API key is empty");
                return;
            }
            _aip = new OpenAIAPI(aipKey);
        }

        public ChatGPT3(string aipKey, string organisationName)
        {
            if(aipKey.Length <= 0)
            {
                Debug.LogError("ERROR:: -- API key is empty");
                return;
            }
            _aip = new OpenAIAPI(new APIAuthentication(aipKey, organisationName));
        }

        /// <summary>
        /// Will return a response from the input prompt given
        /// Will use the current set CurrentModel for the response model.
        /// </summary>
        /// <param name="textPrompt"></param>
        /// <returns></returns>
        public async Task<string> GenerateResponceFromPromot(string textPrompt)
        {
            CompletionRequest request = new CompletionRequest(textPrompt,
                model: CurrentModel,
                max_tokens: 26,
                temperature: CurrentTemprature,
                presencePenalty: 0.1,
                frequencyPenalty: 0.1);

            return (await _aip.Completions.CreateCompletionAsync(request)).ToString();
        }
    }
}
