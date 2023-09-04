#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class LocaleAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LocaleAttribute))]
public class LocaleAttributePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        var keys = LocalizationDataLoader.LocalizationData.Keys;
        var curInd = Array.IndexOf(keys, property.stringValue);
        if (curInd == -1) curInd = 0;
        
        var newInd = EditorGUI.Popup(position, label.text, curInd, keys);
        property.stringValue = keys[newInd];
    }
}
#endif
