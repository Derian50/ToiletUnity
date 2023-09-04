using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationDataLoader
{
    private static LocalizationData _localizationData;

    public static LocalizationData LocalizationData => _localizationData ??= Load<LocalizationData>();
        
    private static T Load<T>() where T : ScriptableObject =>
        Resources.Load<T>(typeof(T).FullName);
}
