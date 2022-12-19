using System;
using UnityEngine;

namespace Persistent
{
    [Serializable]
    public class PersistentArchive
    {
        private static readonly string PersistentArchiveKey = "archive";

        public bool basicCoursePassed;

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

        public static void RecordBasicCoursePassed()
        {
            var a = LoadPersistentArchiveFromPlayerPrefs();
            a.basicCoursePassed = true;
            SavePersistentArchiveToPlayerPrefs(a);
        }
    }
}
