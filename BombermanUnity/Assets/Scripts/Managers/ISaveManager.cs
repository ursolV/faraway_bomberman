namespace Managers
{
    /// <summary>
    /// Save game data by key
    /// </summary>
    /// <typeparam name="T">type of data that will be stored</typeparam>
    public interface ISaveManager<T>
    {
        public void SaveProgress(string key, T progress);
        public T GetProgress(string key);
        public void DeleteProgress(string key);
        public bool HasProgress(string key);
    }
}