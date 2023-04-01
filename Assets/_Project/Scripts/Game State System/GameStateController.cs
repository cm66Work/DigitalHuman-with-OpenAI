using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utils.Events;
using Utils;

namespace BaseGame.GameState
{
    [ExecuteInEditMode]
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private List<GameStateSO> _allGameStates;


        [SerializeField] private int _currentStateIndex = 0;

        public void SwitchGameState(GameStateSO newGameState)
        {
            if(!_allGameStates.Contains(newGameState))
            {
                DrowsyLogger.LogError(this, "ERROR:: GameStateSO is not a resisted GameStateSO, have you added it to All Games Stats?");
                return;
            }

            _allGameStates[_currentStateIndex].UpdateActive(false);
            _currentStateIndex = _allGameStates.IndexOf(newGameState);
            _allGameStates[_currentStateIndex].UpdateActive(true);
        }
        private void Start()
        {
            for(int i = 0; i < _allGameStates.Count; i++)
            {
                _allGameStates[i].UpdateActive(false);
            }


            _allGameStates[_currentStateIndex].UpdateActive(true);
        }
    }
}