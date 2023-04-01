using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Events
{

    [CreateAssetMenu(menuName ="Scriptable Objects/Game Event")]
    public class GameEventSO : ScriptableObject
    {
        public List<GameEventListener> Listeners = new List<GameEventListener>();

        [Header("Logging")]
        public GameEventSO LoggerEventSO;

        // Raise event though different methods signatures.
        public void Raise(Component sender, object data, bool generateLogEvent = false)
        {
            for(int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].OnEventRaised(sender, data);
                if(generateLogEvent && LoggerEventSO != null)
                    LoggerEventSO.Raise(null, $"{this.name} Raised to {Listeners[i].gameObject.name}");
            }
        }
        // Manage Listeners

        public void RegisterListener(GameEventListener listener, bool generateLogEvent = false)
        {
            if(!Listeners.Contains(listener))
                Listeners.Add(listener);

            if(generateLogEvent && LoggerEventSO != null)
            {
                LoggerEventSO.Raise(null, $"Registered: {listener.gameObject.name} from {this.name}");
            }
        }
        public void UnregisterListener(GameEventListener listener, bool generateLogEvent = false)
        {
            if(Listeners.Contains(listener))
                Listeners.Remove(listener);


            if(generateLogEvent && LoggerEventSO != null)
            {
                LoggerEventSO.Raise(null, $"Unregistered: {listener.gameObject.name} to {this.name}");
            }
        }
    }
}