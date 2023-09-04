#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
#endif

using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizeString : MonoBehaviour
{
    [Locale][SerializeField] private string _localizeKey;
    [SerializeField] private UnityEvent<string> _onLocalize = new UnityEvent<string>();

    private TextMeshProUGUI _textMeshPro;
    
    private void Awake()
    {
        Locales locale = LanguageManager.CurrentLocale;
        
        if (_onLocalize.GetPersistentEventCount() == 0)
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            _textMeshPro.text = DataManager.LocalizationData.Key2Locales[_localizeKey][locale];   
        }
        else
        {
            _onLocalize.Invoke(DataManager.LocalizationData.Key2Locales[_localizeKey][locale]);
        }
    }

#if UNITY_EDITOR
    private static FieldInfo _field;

    [MenuItem("CONTEXT/TextMeshProUGUI/Localize")]
    private static void Localize(MenuCommand menuCommand)
    {
        var t = (TMP_Text)menuCommand.context;
        if (!t.TryGetComponent<LocalizeString>(out var l))
            l = t.gameObject.AddComponent<LocalizeString>();

        _field ??= typeof(LocalizeString).GetField(nameof(_onLocalize), BindingFlags.NonPublic | BindingFlags.Instance);
        var onLocalize = (UnityEvent<string>)_field!.GetValue(l);

        for (var i = 0; i < onLocalize.GetPersistentEventCount(); i++)
            if (onLocalize.GetPersistentTarget(i).Equals(t))
                return;

        UnityEventTools.AddPersistentListener(onLocalize,
            Delegate.CreateDelegate(typeof(UnityAction<string>), t, UnityEventBase.GetValidMethodInfo(
                t, "set_text", new[] { typeof(string) }), false) as UnityAction<string>);
    }
#endif
}
