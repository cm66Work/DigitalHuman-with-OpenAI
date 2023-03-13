using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

using Utils.Events;

namespace GPT3DigitalHuman.AR
{
    [RequireComponent(typeof(ARRaycastManager), 
        typeof(ARPlaneManager))]
    public class PlaceObjectOnPlane : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;

        #region Events
        [field: Space(5), Header("Events"), SerializeField] private GameEventSO _logGameEventOnNewLine;
        #endregion

        private ARRaycastManager _arRaycastManager;
        private ARPlaneManager _arPlaneManager;

        private GameObject _gameObject = null;

        private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

        private bool _placed = false;

        private void Awake()
        {
            _arRaycastManager = GetComponent<ARRaycastManager>();
            _arPlaneManager = GetComponent<ARPlaneManager>();

            _gameObject = Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            _gameObject.SetActive(false);
        }

        public void FingerDown(Component sender, object data)
        {
            if(!(data is Vector2))
                return;
            
            _logGameEventOnNewLine.Raise(this, $"Finger Touch Screen at {(Vector2)data}, in screen space");

            CreateRaycast((Vector2)data);

        }

        public void ResetModelPlacment()
        {
            _placed = false;
            _gameObject.SetActive(false);
        }


        private void CreateRaycast(Vector2 touchPointOnScreen)
        {
            if(_placed) return;

            if(_arRaycastManager.Raycast(touchPointOnScreen, _hits, TrackableType.PlaneWithinPolygon))
            {
                foreach(ARRaycastHit hit in _hits)
                {
                    Pose pos = hit.pose;
                    //GameObject obj = Instantiate(_prefab, pos.position, pos.rotation);

                    //if(_gameObject != null)
                    //{
                    //    Destroy(_gameObject);
                    //    _gameObject = obj;
                    //}

                    if(_arPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        _gameObject.transform.position = pos.position;
                        Vector3 position = _gameObject.transform.position;
                        Vector3 cameraPosition = Camera.main.transform.position;
                        Vector3 directiopn = cameraPosition - position;
                        Vector3 targetRotationEuler = Quaternion.LookRotation(directiopn).eulerAngles;
                        Vector3 scaleEuler = Vector3.Scale(targetRotationEuler, _gameObject.transform.up.normalized);
                        Quaternion targetRotation = Quaternion.Euler(scaleEuler);
                        _gameObject.transform.rotation =  targetRotation;

                        _gameObject.SetActive(true);
                        _placed = true;
                        break;
                    }
                }
            }
        }
    }
}
