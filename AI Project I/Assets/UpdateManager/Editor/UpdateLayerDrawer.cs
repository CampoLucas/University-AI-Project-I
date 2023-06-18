using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(UpdateLayer))]
public class UpdateLayerDrawer : PropertyDrawer
{
    private const float PROPERTY_HEIGHT = 18f;
    private const float SPACING = 3f;

    private SerializedProperty _order;
    private SerializedProperty _fixedFrameRate;
    private SerializedProperty _frameRate;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        _order = property.FindPropertyRelative("order");
        _fixedFrameRate = property.FindPropertyRelative("fixedFrameRate");
        _frameRate = property.FindPropertyRelative("frameRate");

        var height = PROPERTY_HEIGHT;

        if (property.isExpanded)
        {
            height += EditorGUI.GetPropertyHeight(_order) + SPACING;
            height += EditorGUI.GetPropertyHeight(_fixedFrameRate) + SPACING;

            //height += PROPERTY_HEIGHT + SPACING;
            if (_fixedFrameRate.boolValue)
            {
                //height += PROPERTY_HEIGHT + SPACING;
                height += EditorGUI.GetPropertyHeight(_frameRate) + SPACING;
            }

            
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var foldoutRect = new Rect(position.x, position.y, position.width, PROPERTY_HEIGHT);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            var rect = new Rect(position.x, position.y + PROPERTY_HEIGHT, position.width, PROPERTY_HEIGHT);

            
            EditorGUI.PropertyField(rect, _order);

            rect.y += PROPERTY_HEIGHT + SPACING;
            EditorGUI.PropertyField(rect, _fixedFrameRate);

            if (_fixedFrameRate.boolValue)
            {
                rect.y += PROPERTY_HEIGHT + SPACING;
                EditorGUI.PropertyField(rect, _frameRate);
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}
