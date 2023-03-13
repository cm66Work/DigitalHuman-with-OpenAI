using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utils
{ 
    public static class UtilsClass
    {
        #region Mouse Position 2D
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        private static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        private static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        private static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPostion = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPostion;
        }


        #endregion
    }
}