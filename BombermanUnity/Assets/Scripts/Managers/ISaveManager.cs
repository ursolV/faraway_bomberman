namespace Managers
{
    public interface ISaveManager<T>
    {
        public void SaveProgress(string key, T progress);
        public T GetProgress(string key);
        public void DeleteProgress(string key);
        public bool HasProgress(string key);
    }
}