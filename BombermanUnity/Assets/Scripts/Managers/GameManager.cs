using System.Threading.Tasks;
using Map;
using UI;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Main manager
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private WindowManager windowManager;
        [SerializeField] private LocationManager locationManager;

        private ISaveManager<LocationSave> _saveManager;
        private static GameManager _instance;

        public static GameManager Instance => _instance;
        public LocationManager LocationManager => locationManager;
        public ISaveManager<LocationSave> SaveManager => _saveManager;

        private void Start()
        {
            _instance = this;
            _saveManager = new SaveManager<LocationSave>();
            windowManager.OpenWindow("start");
        }

        public async void FinishGame(bool result)
        {
            _saveManager.DeleteProgress(locationManager.CurrentLocationId);
            windowManager.CloseWindow("battle");
            //delay to see the death animation before opening the window 
            await Task.Delay(1000);
            windowManager.OpenWindow("gameOver", result);
        }

        public void OpenStartScreen()
        {
            locationManager.UnloadCurrentLocation();
            windowManager.OpenWindow("start");
        }
        
        public void LoadLocation(string locationId)
        {
            var save = _saveManager.GetProgress(locationId);
            locationManager.LoadLocation(locationId, save);
            windowManager.OpenWindow("battle");
        }

        public void RestartLocation()
        {
            LoadLocation(locationManager.CurrentLocationId);
        }

        public void SaveProgress()
        {
            var progress = locationManager.GetProgress();
            _saveManager.SaveProgress(progress.id, progress.progress);
        }
    }
}
