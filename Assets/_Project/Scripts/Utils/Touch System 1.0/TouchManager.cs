using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class TouchManager : MonoBehaviour
{
    #region Events
    [SerializeField] Utils.Events.GameEventSO _oneFigerTouchScreenPointEvent;
    #endregion

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }


    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if finger is over a UI element
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
            
            _oneFigerTouchScreenPointEvent.Raise(this, finger.screenPosition);
        }

    }
}
