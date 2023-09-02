using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Map;
using UnityEngine;

namespace Managers
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private Location[] locations;

        private Location _currentLocation;

        public string CurrentLocation { get; private set; }

        public List<string> Locations => locations.Select(l => l.name).ToList();

        public void LoadLocation(string locationId, LocationSave save)
        {
            UnloadLocation();

            _currentLocation = locations.First(location => string.Equals(location.name, locationId, StringComparison.CurrentCultureIgnoreCase));
            _currentLocation.Enable(save);
            CurrentLocation = locationId;
        }

        public void UnloadLocation()
        {
            if (_currentLocation != null)
            {
                _currentLocation.Disable();
            }
        }

        public (string id, LocationSave progress) GetProgress()
        {
            return (_currentLocation.name, _currentLocation.GetProgress());
        }

        public async Task Throw(string bombId, float strength)
        {
            await _currentLocation.Throw(bombId, strength);
        }
    }
}