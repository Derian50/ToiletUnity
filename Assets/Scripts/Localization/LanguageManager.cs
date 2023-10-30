using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private Locales _currentLocale;

    public static Locales CurrentLocale { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        DataManager.LocalizationData.Init();

        _currentLocale = System.Enum.Parse<Locales>(YaSDK.GetLanguage());
        CurrentLocale = _currentLocale;
        
        
    }
}
