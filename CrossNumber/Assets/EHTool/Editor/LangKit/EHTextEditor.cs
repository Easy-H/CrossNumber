using UnityEditor;
using UnityEngine;

namespace EHTool.LangKit {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(EHText))]
    public class EHTextEditor : UnityEditor.UI.TextEditor {

        SerializedProperty _key;
        protected override void OnEnable()
        {
            _key = serializedObject.FindProperty("_key");
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_key);
            serializedObject.ApplyModifiedProperties();


            EHText t = target as EHText;
            if (GUILayout.Button("Å° Ãß°¡"))
            {
                t.AddKey();
            }

            base.OnInspectorGUI(); 

        }
    }
}