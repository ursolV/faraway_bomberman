using System.Linq;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartWindow : AbstractWindow
    {
        [SerializeField] private TMP_Dropdown _locationDropdown;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;

        private void Awake()
        {
            _locationDropdown.onValueChanged.AddListener(OnLocationChanged);
            _continueButton.onClick.AddListener(OnContinueClick);
            _newGameButton.onClick.AddListener(OnNewGameClick);
        }

        private void OnNewGameClick()
        {
            var locationId = GameManager.Instance.LocationManager.LocationIds[_locationDropdown.value];
            GameManager.Instance.SaveService.DeleteProgress(locationId);
            GameManager.Instance.LoadLocation(locationId);
            Close();
        }

        private void OnContinueClick()
        {
            var locationId = GameManager.Instance.LocationManager.LocationIds[_locationDropdown.value];
            GameManager.Instance.LoadLocation(locationId);
            Close();
        }

        private void OnLocationChanged(int index)
        {
            var id = GameManager.Instance.LocationManager.LocationIds[index];
            _continueButton.interactable = GameManager.Instance.SaveService.HasProgress(id);
        }

        public override void Open()
        {
            base.Open();

            var locations = GameManager.Instance.LocationManager.LocationIds;
            
            _locationDropdown.options = locations.Select(id => new TMP_Dropdown.OptionData(id)).ToList();
            _locationDropdown.value = 0;
            OnLocationChanged(0);
        }
    }
}