#if UNITY_EDITOR
using System.Linq;
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable InconsistentNaming

public enum Locales
{
    en,
    ru,
    tr,
}

[CreateAssetMenu(order = 51)]
public class LocalizationData : ScriptableObject
{
    [SerializeField] private LocaleString[] _uiLocaleStrings;
    // [SerializeField] private LocaleString[] _meleeLocaleStrings;
    // [SerializeField] private LocaleString[] _rangedLocaleStrings;
    
    [System.Serializable]
    public struct LocaleString
    {
        public string LocalizeKey;
        public EnumDict<Locales, string> Locales;
    }

    public readonly Dictionary<string, EnumDict<Locales, string>> Key2Locales = new();
    // public Dictionary<MinionType, LocaleString[]> MinionLocaleStrings;

    public void Init()
    {
        foreach (var ls in _uiLocaleStrings)
        {
            Key2Locales[ls.LocalizeKey] = ls.Locales;
        }

        // MinionLocaleStrings = new Dictionary<MinionType, LocaleString[]>
        // {
        //     { MinionType.Melee, _meleeLocaleStrings},
        //     { MinionType.Ranged, _rangedLocaleStrings}
        // };
    }

#if UNITY_EDITOR
    public string[] Keys
    {
        get
        {
            var s = new List<string> { "-" };
            s.AddRange(_uiLocaleStrings.Select(ls => ls.LocalizeKey));
            return s.ToArray();
        }
    }
#endif
}
