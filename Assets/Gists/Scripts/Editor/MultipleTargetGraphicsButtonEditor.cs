#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

namespace Gists.Editor
{
    [CustomEditor(typeof(MultipleTargetGraphicsButton))]
    public class MultipleTargetGraphicsButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SerializedObject serializedObject = new SerializedObject(target);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_additionalTargetGraphics"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
