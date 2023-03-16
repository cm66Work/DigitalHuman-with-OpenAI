using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame.GameState
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game State")]
    public class GameStateSO : ScriptableObject
    {
        private bool _isActive = false;

        private List<GameObject> ManagedGameObjects = new List<GameObject>();


        public void UpdateActive(bool isActive)
        {
            if(ManagedGameObjects == null) return;

            this._isActive = isActive;

            for(int i = 0; i < ManagedGameObjects.Count; i++)
            {
                ManagedGameObjects[i].SetActive(_isActive);
            }
        }

        public void RegisterGameObject(GameObject gameObjectToRegister)
        {
            if(ManagedGameObjects.Contains(gameObjectToRegister))
            {
                Utils.DrowsyLogger.LogWarning(this, "Warning:: Game Object already registered to manager");
                return;
            }

            ManagedGameObjects.Add(gameObjectToRegister);
        }

        public void UnregisterGameObject(GameObject gameObject)
        {
            if(ManagedGameObjects.Contains(gameObject))
            {
                ManagedGameObjects.Remove(gameObject);
                return;
            }

            Utils.DrowsyLogger.LogWarning(this, "Warning:: attempting to unregister a gameobject that has not been registered");
        }
    }
}