using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[System.Serializable]
public class TreeNode
{
    [HideInInspector]
    public bool question;
}

[System.Serializable]
public class TreeQuestion : TreeNode
{
    public Func<bool> Question;
    public TreeNode tNode;
    public TreeNode fNode;
}

[System.Serializable]
public class TreeAction : TreeNode
{
    public ActionWrapper Action;
}

[System.Serializable]
public class ActionWrapper : ScriptableObject
{
    public Action Action;
}

[CustomEditor(typeof(TreeAction))]
public class MyScriptEditor : Editor
{
    private string[] _methodNames;
    private int _selectedMethodIndex;

    private void OnEnable()
    {
        // Get all methods in the target script that return void and take no parameters
        MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.ReturnType == typeof(void) && m.GetParameters().Length == 0).ToArray();

        // Convert the method info into method names
        _methodNames = methods.Select(m => m.Name).ToArray();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Get the variable of type Action from the target script
        SerializedProperty actionProperty = serializedObject.FindProperty("Action");

        // Draw the dropdown list of method names
        _selectedMethodIndex = EditorGUILayout.Popup("Action Method", _selectedMethodIndex, _methodNames);

        // Get the selected method by name
        MethodInfo selectedMethod = target.GetType().GetMethod(_methodNames[_selectedMethodIndex], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // Create a delegate from the selected method and assign it to the Action variable
        if (GUILayout.Button("Assign Action"))
        {
            if (selectedMethod != null)
            {
                ActionWrapper actionWrapper = ScriptableObject.CreateInstance<ActionWrapper>();
                Action actionDelegate = (Action)Delegate.CreateDelegate(typeof(Action), target, selectedMethod);
                actionWrapper.Action = actionDelegate;
                actionProperty.objectReferenceValue = actionWrapper;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

public class Enemy : MonoBehaviour
{
    private TreeNode _nodeSo;
    public TreeQuestion qNode;
    public TreeAction aNode;

    public void A()
    {
        
    }
}
