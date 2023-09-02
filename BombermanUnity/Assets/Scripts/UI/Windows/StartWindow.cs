using System.Linq;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartWindow : BaseWindow
    {
        [SerializeField] private TMP_Dropdown locationDropdown;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;

        private void Awake()
        {
            locationDropdown.onValueChanged.AddListener(OnLocationChanged);
            continueButton.onClick.AddListener(OnContinueClick);
            newGameButton.onClick.AddListener(OnNewGameClick);
        }

        private void OnNewGameClick()
        {
            var locationId = GameManager.Instance.LocationManager.Locations[locationDropdown.value];
            GameManager.Instance.SaveManager.DeleteProgress(locationId);
            GameManager.Instance.LoadLocation(locationId);
            Close();
        }

        private void OnContinueClick()
        {
            var locationId = GameManager.Instance.LocationManager.Locations[locationDropdown.value];
            GameManager.Instance.LoadLocation(locationId);
            Close();
        }

        private void OnLocationChanged(int index)
        {
            var id = GameManager.Instance.LocationManager.Locations[index];
            continueButton.interactable = GameManager.Instance.SaveManager.HasProgress(id);
        }

        public override void Open()
        {
            base.Open();

            var locations = GameManager.Instance.LocationManager.Locations;
            
            locationDropdown.options = locations.Select(id => new TMP_Dropdown.OptionData(id)).ToList();
            locationDropdown.value = 0;
            OnLocationChanged(0);
        }
    }
}