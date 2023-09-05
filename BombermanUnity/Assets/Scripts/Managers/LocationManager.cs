using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manage locations
    /// </summary>
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private Location[] _locations;

        private Location _currentLocation;

        public string CurrentLocationId { get; private set; }
        public Location CurrentLocation => _currentLocation;

        /// <summary>
        /// Id of all available locations
        /// </summary>
        public List<string> LocationIds => _locations.Select(l => l.name).ToList();

        /// <summary>
        /// Load the location and apply the save
        /// </summary>
        public void LoadLocation(string locationId, LocationSave save)
        {
            UnloadCurrentLocation();

            _currentLocation = _locations.First(location => string.Equals(location.name, locationId, StringComparison.CurrentCultureIgnoreCase));
            _currentLocation.Enable(save);
            CurrentLocationId = locationId;
        }

        public void UnloadCurrentLocation()
        {
            if (_currentLocation != null)
            {
                _currentLocation.Disable();
            }
        }

        /// <summary>
        /// Get location saves that can be saved for further apply
        /// </summary>
        public (string id, LocationSave progress) GetProgress()
        {
            return (_currentLocation.name, _currentLocation.GetProgress());
        }
    }
}