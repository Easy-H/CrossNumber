using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NewText))]
public class NewTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUI.changed)
            EditorUtility.SetDirty(target);
        base.OnInspectorGUI();
    }
}
