using System;
using UnityEngine;

namespace Persistent
{
    [Serializable]
    public class PersistentArchive
    {
        private static readonly string PersistentArchiveKey = "archive";
        
        public bool firstCoursePassed;
        public bool secondCoursePassed;

        private static PersistentArchive BuildDefaultPersistentArchive()
        {
            return new PersistentArchive();
        }

        public static PersistentArchive LoadPersistentArchiveFromPlayerPrefs()
        {
            var playerArchiveJson = PlayerPrefs.GetString(PersistentArchiveKey, "{}");
            if (playerArchiveJson == "{}")
            {
                return BuildDefaultPersistentArchive();
            }
            else
            {
                return JsonUtility.FromJson<PersistentArchive>(playerArchiveJson);
            }
        }

        private static void SavePersistentArchiveToPlayerPrefs(PersistentArchive persistentArchive)
        {
            PlayerPrefs.SetString(PersistentArchiveKey, JsonUtility.ToJson(persistentArchive));
        }

        public static void RecordFirstCoursePassed()
        {
            var a = LoadPersistentArchiveFromPlayerPrefs();
            a.firstCoursePassed = true;
            SavePersistentArchiveToPlayerPrefs(a);
        }

        public static void RecordSecondCoursePassed()
        {
            var a = LoadPersistentArchiveFromPlayerPrefs();
            a.secondCoursePassed = true;
            SavePersistentArchiveToPlayerPrefs(a);
        }
    }
}