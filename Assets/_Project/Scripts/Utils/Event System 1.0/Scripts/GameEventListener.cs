using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utils.Events
{
    [System.Serializable]
    public class CustomeGameEvent : UnityEvent<Component, object> { }

    public class GameEventListener : MonoBehaviour
    {
        public GameEventSO GameEvent;

        public CustomeGameEvent Response;

        public void OnEnable()
        {
            GameEvent.RegisterListener(this, true);
        }

        private void OnDisable()
        {
            GameEvent.UnregisterListener(this, true);
        }

        public void OnEventRaised(Component sender, object data)
        {
            Response?.Invoke(sender, data);
        }
    }
}
