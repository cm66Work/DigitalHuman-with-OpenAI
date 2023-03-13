using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utils.Events;

namespace Utils
{

    public class Mouse3D : MonoBehaviour
    {

        public static Mouse3D Instance { get; private set; }


        [SerializeField] private LayerMask _mouseColliderLayerMask;


        [Header("Events")]
        [SerializeField]
        private GameEventSO _onMouseClickedOnTileEventl;

        [Header("Debugging")]
        [SerializeField] private bool _debugMode = false;
        [SerializeField] private Transform _3DMouseInWorld;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            HandelMouseInput();

            DebugMode();
        }

        private void HandelMouseInput()
        {
            if(!Input.GetMouseButtonDown(0)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseColliderLayerMask))
            {
                //transform.position = raycastHit.point;

                _onMouseClickedOnTileEventl.Raise(this, raycastHit.point);
            }
        }

        private void DebugMode()
        {
            if(!_debugMode) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseColliderLayerMask))
            {
                _3DMouseInWorld.position = raycastHit.point;
            }

        }

        public static Vector3 GetMouseWorldPostion() => Instance.GetMouseWorldPostion_Instance();

        private Vector3 GetMouseWorldPostion_Instance()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseColliderLayerMask))
            {
                return raycastHit.point;
            }
            return Vector3.zero;
        }
    }
}