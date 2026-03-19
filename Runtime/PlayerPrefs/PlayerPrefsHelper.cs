using UnityEngine;

public static class PlayerPrefsHelper
{
    /// <summary>
    /// bool값을 PlayerPrefs에 저장한다 (true = 1, false = 0)
    /// </summary>
    /// <param name="key">저장할 키값</param>
    /// <param name="value">저장할 값</param>
    public static void SetBool(string key, bool value)
    {
        var intValue = value ? 1 : 0;
        PlayerPrefs.SetInt(key, intValue);
    }

    /// <summary>
    /// PlayerPrefs에서 bool 값을 불러온다 (1 = true, 0 = false)
    /// </summary>
    /// <param name="defaultValue">저장된 값이 없을 때 기본값</param>
    /// <returns></returns>
    public static bool GetBool(string key, bool defaultValue = false)
    {
        // 키에 값이 존재하지 않을 경우를 대비해 기본값을 int로 변환
        int defaultIntValue = defaultValue ? 1 : 0;

        int intValue = PlayerPrefs.GetInt(key, defaultIntValue);
        
        // 1이면 true, 그 외의 값(0...)이면 false를 반환
        return intValue == 1;
    }
}