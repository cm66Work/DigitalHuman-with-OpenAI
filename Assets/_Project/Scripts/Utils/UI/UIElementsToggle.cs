using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utils.UI
{   
    [RequireComponent(typeof(CanvasGroup))]
    public class UIElementsToggle : MonoBehaviour
    {
        [SerializeField] private bool _offByDefault = false;

        private CanvasGroup _canvasGroup;
        private bool _currentActiveState;

        private void Start()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();
            _currentActiveState = _offByDefault;
            /*
             * Crew a canvas group for the game object this script is attached to.
             * 
             * if _offByDefault then hid the canvas group, which will hide the UI elements.
             * else do nothing.
             */

            if(_offByDefault == false)
            {
                SetCanvasGroupState(true);
                return;
            }
            SetCanvasGroupState(false);
        }
        private void SetCanvasGroupState(bool active)
        {
            if(active == _currentActiveState) return;

            _canvasGroup.blocksRaycasts = active;
            _canvasGroup.interactable = active;
            _canvasGroup.alpha = active ? 1 : 0;
            _currentActiveState = active;
        }

        public void ToggleUI() => SetCanvasGroupState(!_currentActiveState);
        public void ShowUI() => SetCanvasGroupState(true);
        public void HideUI() => SetCanvasGroupState(false);
    }
}