using UnityEditor;

namespace Utils.Pooling
{

    [CustomEditor(typeof(PoolObject))]
    public class PoolObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Place this on a prefab that you want to be use the ObjectPooling Class", MessageType.Info);

            base.OnInspectorGUI();
        }
    }
}