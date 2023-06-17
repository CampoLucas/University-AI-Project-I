using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumDataContainer<,>))]
public class EnumDataContainerDrawer : PropertyDrawer
{
    private Rect _rect;
    private const float FOLDOUT_HEIGHT = 16f;
    private const float SPACING = 8f;

    #region Serialized Properties

    private SerializedProperty _content;
    private SerializedProperty _enumType;

    #endregion

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        GetSerializedProperties(property);
        
        var height = FOLDOUT_HEIGHT;
        
        if (!property.isExpanded) return height + SPACING;
        
        if (_content.arraySize != _enumType.enumNames.Length)
            _content.arraySize = _enumType.enumNames.Length;

        for (var i = 0; i < _content.arraySize; i++)
        {
            height += EditorGUI.GetPropertyHeight(_content.GetArrayElementAtIndex(i)) + SPACING;
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        _rect = position;
        _rect.height = FOLDOUT_HEIGHT;
        
        property.isExpanded = EditorGUI.Foldout(_rect, property.isExpanded, label);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            var height = FOLDOUT_HEIGHT;
            
            for (var i = 0; i < _content.arraySize; i++)
            {
                var rect = new Rect(position.x, position.y + height, position.width,
                    EditorGUI.GetPropertyHeight(_content.GetArrayElementAtIndex(i)) + SPACING);
                height += rect.height;
                EditorGUI.PropertyField(rect, _content.GetArrayElementAtIndex(i),
                    new GUIContent(_enumType.enumNames[i]), true);
            }

            EditorGUI.indentLevel--;
        }
        
        EditorGUI.EndProperty();
    }
    
    private void GetSerializedProperties(SerializedProperty property)
    {
        if (_content == null)
            _content = property.FindPropertyRelative("content");

        if (_enumType == null)
            _enumType = property.FindPropertyRelative("enumType");
    }
}




