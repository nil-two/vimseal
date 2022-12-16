using System;
using UnityEngine;

[Serializable]
public class PersistentConfig
{
    private static readonly string PersistentConfigKey = "config";
    private static readonly int VolumeDefault = 80;
    private static readonly int VolumeMax = 100;
    private static readonly int VolumeUnit = 10;

    public int bgmVolume;
    public int seVolume;

    private PersistentConfig(int bgmVolume, int seVolume)
    {
        this.bgmVolume = bgmVolume;
        this.seVolume = seVolume;
    }

    private static PersistentConfig BuildDefaultPersistentConfig()
    {
        return new PersistentConfig(VolumeDefault, VolumeDefault);
    }

    public static PersistentConfig LoadPersistentConfigFromPlayerPrefs()
    {
        var playerConfigJson = PlayerPrefs.GetString(PersistentConfigKey, "{}");
        if (playerConfigJson == "{}")
        {
            return BuildDefaultPersistentConfig();
        }
        else
        {
            return JsonUtility.FromJson<PersistentConfig>(playerConfigJson);
        }
    }
    
    public static void SavePersistentConfigToPlayerPrefs(PersistentConfig persistentConfig)
    {
        PlayerPrefs.SetString(PersistentConfigKey, JsonUtility.ToJson(persistentConfig));
    }

    public void IncreaseBGMVolume()
    {
        bgmVolume = Math.Clamp(bgmVolume + VolumeUnit, 0, VolumeMax);
    }

    public void DecreaseBGMVolume()
    {
        bgmVolume = Math.Clamp(bgmVolume - VolumeUnit, 0, VolumeMax);
    }

    public void IncreaseSEVolume()
    {
        seVolume = Math.Clamp(seVolume + VolumeUnit, 0, VolumeMax);
    }

    public void DecreaseSEVolume()
    {
        seVolume = Math.Clamp(seVolume - VolumeUnit, 0, VolumeMax);
    }
}