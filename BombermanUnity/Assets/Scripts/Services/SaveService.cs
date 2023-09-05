using UnityEngine;

namespace Services
{
    /// <summary>
    /// Save game data by key to <see cref="PlayerPrefs"/>
    /// </summary>
    /// <typeparam name="T">type of data that will be stored</typeparam>
    public class SaveService<T> : ISaveService<T>
    {
        public void SaveProgress(string key, T progress)
        {
            var json = JsonUtility.ToJson(progress);
            PlayerPrefs.SetString(key, json);
        }

        public T GetProgress(string key)
        {
            if (!HasProgress(key))
                return default;

            var json = PlayerPrefs.GetString(key);
            var save = JsonUtility.FromJson<T>(json);
            return save;
        }

        public void DeleteProgress(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public bool HasProgress(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}