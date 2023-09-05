using Map;
using Services;
using UI;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Main manager
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private WindowManager _windowManager;
        [SerializeField] private LocationManager _locationManager;

        private ISaveService<LocationSave> _saveService;
        private static GameManager _instance;

        public static GameManager Instance => _instance;
        public LocationManager LocationManager => _locationManager;
        public ISaveService<LocationSave> SaveService => _saveService;

        private void Start()
        {
            _instance = this;
            _saveService = new SaveService<LocationSave>();
            _windowManager.OpenWindow("start");
        }

        public void FinishGame(bool result)
        {
            _saveService.DeleteProgress(_locationManager.CurrentLocationId);
            _windowManager.CloseWindow("battle");
            _windowManager.OpenWindow("gameOver", result);
        }

        public void OpenStartScreen()
        {
            _locationManager.UnloadCurrentLocation();
            _windowManager.OpenWindow("start");
        }
        
        public void LoadLocation(string locationId)
        {
            var save = _saveService.GetProgress(locationId);
            _locationManager.LoadLocation(locationId, save);
            _windowManager.OpenWindow("battle");
        }

        public void RestartLocation()
        {
            LoadLocation(_locationManager.CurrentLocationId);
        }

        public void SaveProgress()
        {
            var progress = _locationManager.GetProgress();
            _saveService.SaveProgress(progress.id, progress.progress);
        }
    }
}
