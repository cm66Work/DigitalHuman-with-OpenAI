using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame.GameState
{
    /// <summary>
    /// Register Game Object that this script is attached to a specific game state.
    /// Scriptable object lists need to be generated at runtime.
    /// </summary>
    public class RegisterGameObjectGameState : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameStateSO;
        [SerializeField] private List<GameObject> _managedGameObjects; 

        private bool _isAlreadyRegistered = false;

        private void Awake()
        {
            RegisterToTargetGameStateSO();
        }

        private void OnDestroy()
        {
            for(int i = 0; i < _managedGameObjects.Count; i++)
            {
                _gameStateSO.UnregisterGameObject(_managedGameObjects[i]);
                _isAlreadyRegistered = false; 
            }
        }

        private void RegisterToTargetGameStateSO()
        {
            for(int i = 0; i < _managedGameObjects.Count; i++)
            {
                _gameStateSO.RegisterGameObject(_managedGameObjects[i]);
            }

            //if(gameObject.transform.childCount > 0)
            //{
            //    Utils.DrowsyLogger.LogWarning(this, "Warning:: Having Game Objects with children that are managed by Game State Controller may " +
            //        "lead to reduced performance when changing active game states." +
            //        "\n Would recommend merging all children into one Game Object");
            //}

            _isAlreadyRegistered = true; // prevent gameobject from being registered move then once.
        }
    }
}