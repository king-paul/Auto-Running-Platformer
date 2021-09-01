using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]

public class NPCInspector : Editor
{
    SerializedProperty velocity;
    bool open = false;

    private void OnEnable()
    {
        velocity = serializedObject.FindProperty("m_MoveVelocity");
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(velocity);
        serializedObject.ApplyModifiedProperties();

        //Vector3 vel = ((Player)target).m_MoveVelocity;

        //open = EditorGUILayout.DropdownButton("Menu", GUILayout.foc)
    

        //EditorGUILayout.LabelField("x");
        //vel.x = EditorGUILayout.FloatField(vel.x);
        //EditorGUILayout.LabelField("x");
        //vel.y = EditorGUILayout.FloatField(vel.y);
        //GUILayoutUtility.GetRect(200, 200);
        //vel.x = EditorGUI.FloatField(new Rect(0, 25, 100, 20), vel.x)

        //((Player)target).m_MoveVelocity = vel;
        
        // UI SYSTEMS
        //GUI - Old manual system
        //GUILayout

        //EditorGUI
        //EditorGUILayout

        
    }
}
