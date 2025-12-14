using UnityEngine;

public static class ClassUnlockManager
{

    public static bool IsUnlocked(string characterID)
    {
        return PlayerPrefs.GetInt("char_unlocked_" + characterID, 0) == 1;
    }

    public static void Unlock(string characterID)
    {
        PlayerPrefs.SetInt("char_unlocked_" + characterID, 1);
        PlayerPrefs.Save();
    }
}
