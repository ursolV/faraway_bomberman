using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Map;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// It switches locations.
    /// Acts as a mediator between characters and bombs
    /// </summary>
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private Location[] locations;

        private Location _currentLocation;

        public string CurrentLocationId { get; private set; }

        /// <summary>
        /// id of all available locations
        /// </summary>
        public List<string> LocationIds => locations.Select(l => l.name).ToList();

        /// <summary>
        /// Load the location and apply the save
        /// </summary>
        public void LoadLocation(string locationId, LocationSave save)
        {
            UnloadCurrentLocation();

            _currentLocation = locations.First(location => string.Equals(location.name, locationId, StringComparison.CurrentCultureIgnoreCase));
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
        /// <returns></returns>
        public (string id, LocationSave progress) GetProgress()
        {
            return (_currentLocation.name, _currentLocation.GetProgress());
        }

        /// <summary>
        /// Throw a bomb at the current location
        /// </summary>
        /// <param name="strength">Throwing power. Between 0-1</param>
        public async Task Throw(string bombId, float strength)
        {
            await _currentLocation.Throw(bombId, strength);
        }
    }
}