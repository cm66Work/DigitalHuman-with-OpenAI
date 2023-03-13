using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utils.Variables
{
    [CreateAssetMenu(menuName = "Utils/Variables/Float")]
    public class FloatVariableSO : ScriptableObject
    {
        public float Value;
    }

    [Serializable]
    public class ReferenceFloatSO
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariableSO Variable;

        public float Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }
    }
}
