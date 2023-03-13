using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Events
{

    [CreateAssetMenu(menuName ="Scriptable Objects/Game Event")]
    public class GameEventSO : ScriptableObject
    {
        public List<GameEventListener> Listeners = new List<GameEventListener>();

        // Raise event though different methods signatures.
        public void Raise(Component sender, object data)
        {
            for(int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].OnEventRaised(sender, data);
            }
        }
        // Manage Listeners

        public void RegisterListener(GameEventListener listener)
        {
            if(!Listeners.Contains(listener))
                Listeners.Add(listener);
        }
        public void UnregisterListener(GameEventListener listener)
        {
            if(Listeners.Contains(listener))
                Listeners.Remove(listener);
        }
    }
}