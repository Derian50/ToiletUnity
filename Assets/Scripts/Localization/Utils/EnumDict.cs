using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public sealed class EnumDict<TEnum, TValue> : IEnumerable<EnumDict<TEnum, TValue>.EnumDictElement>, ISerializationCallbackReceiver where TEnum : Enum
{
    [SerializeField] private EnumDictElement[] Dict = CreateNewDict();

    [Serializable]
    public struct EnumDictElement
    {
        [SerializeField] private string key;
        [SerializeField] private TValue value;

        public TEnum Key
        {
            get => Name2Enum.TryGetValue(key, out var res) ? res : default;
            set => key = value.ToString();
        }

        public TValue Value
        {
            get => value;
            set => this.value = value;
        }

        public override string ToString() =>
            $"[{Key.ToString()}, {Value.ToString()}]";
    }

    private static readonly Type enumType = typeof(TEnum);
    private static readonly TEnum[] EnumArray = (TEnum[])Enum.GetValues(enumType);
    private static readonly Dictionary<string, TEnum> Name2Enum = EnumArray.ToDictionary(e => Enum.GetName(enumType, e));

    private static EnumDictElement[] CreateNewDict() => EnumArray.Select(e => new EnumDictElement { Key = e }).ToArray();

    public TValue this[int enumInt]
    {
        get => Dict[enumInt].Value;
        set => Dict[enumInt].Value = value;
    }

    public TValue this[TEnum key]
    {
        get => this[Array.IndexOf(EnumArray, key)];
        set => this[Array.IndexOf(EnumArray, key)] = value;
    }

    public int Length => EnumArray.Length;
    public Type EnumType => enumType;

    public IReadOnlyCollection<TEnum> Keys => EnumArray;
    public ICollection<TValue> Values => Dict.Select(element => element.Value).ToArray();

    public void Clear() =>
        Dict = CreateNewDict();

    public bool Contains(TValue item) =>
        Dict.Any(e => e.Value.Equals(item));

    public EnumDict<TEnum, TValue> Clone()
    {
        var copy = new EnumDict<TEnum, TValue>();
        for (var i = 0; i < Dict.Length; i++)
            copy[i] = Dict[i].Value;
        return copy;
    }

    IEnumerator<EnumDictElement> IEnumerable<EnumDictElement>.GetEnumerator() => Dict.ToList().GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Dict.GetEnumerator();

    private void Check()
    {
        if (Dict.Length == EnumArray.Length && !Dict.Where((e, i) => !e.Key.Equals(EnumArray[i])).Any()) return;

        var d = CreateNewDict();
        for (var i = 0; i < d.Length; i++)
            d[i].Value = Dict.FirstOrDefault(e => e.Key.Equals(d[i].Key)).Value;
        Dict = d;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize() => Check();
    void ISerializationCallbackReceiver.OnBeforeSerialize() => Check();
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnumDict<,>), false)]
public class EnumDictDrawer : PropertyDrawer
{
    private const string DictName = "Dict";
    private const string keyName = "key";
    private const string valueName = "value";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            var dict = property.FindPropertyRelative(DictName);

            EditorGUI.indentLevel++;

            for (var i = 0; i < dict.arraySize; i++)
            {
                var element = dict.GetArrayElementAtIndex(i);

                var key = element.FindPropertyRelative(keyName);
                var value = element.FindPropertyRelative(valueName);

                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                position.height = EditorGUI.GetPropertyHeight(value);

                EditorGUI.PropertyField(position, value,
                    new GUIContent(ObjectNames.NicifyVariableName(key.stringValue)), true);
            }

            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        var dict = property.FindPropertyRelative(DictName);
        var vertSpace = EditorGUIUtility.standardVerticalSpacing;
        var height = EditorGUIUtility.singleLineHeight;

        for (var i = 0; i < dict.arraySize; i++)
            height += EditorGUI.GetPropertyHeight(dict.GetArrayElementAtIndex(i).FindPropertyRelative(valueName)) + vertSpace;

        return height;
    }
}
#endif
