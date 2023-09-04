using UnityEngine;

public static class DataManager
{
    // private static ItemsData _itemsData;
    // private static BodiesData _bodiesData;
    // private static SoundData _soundData;
    private static LocalizationData _localizationData;

    // public static ItemsData ItemsData => _itemsData ??= Load<ItemsData>();
    // public static BodiesData BodiesData => _bodiesData ??= Load<BodiesData>();
    // public static SoundData SoundData => _soundData ??= Load<SoundData>();
    public static LocalizationData LocalizationData => _localizationData ??= Load<LocalizationData>();

    private static T Load<T>() where T : ScriptableObject =>
        Resources.Load<T>(typeof(T).FullName);
}
