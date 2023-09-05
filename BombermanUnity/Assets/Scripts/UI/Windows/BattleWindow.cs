using System;
using System.Collections;
using System.Linq;
using Managers;
using Map.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattleWindow : AbstractWindow
    {
        [SerializeField] private Button _throwButton;
        [SerializeField] private Slider _powerSlider;
        [SerializeField] private Image _selectedBombImage;
        [SerializeField] private Sprite[] _bombSprites;
        [SerializeField] private LocationManager _locationManager;

        private float _strength;
        private string _selectedBomb;
        private State _state;
        private Personage _personage;
        
        public static event Action<string, float> OnThrow;

        public override void Open()
        {
            base.Open();
            _personage = _locationManager.CurrentLocation.GetPersonage();
        }

        public void OnSelectBomb(string bombId)
        {
            _selectedBomb = bombId;
            _selectedBombImage.sprite = _bombSprites.FirstOrDefault(s =>
                string.Equals(s.name, bombId, StringComparison.CurrentCultureIgnoreCase));
        }
        
        public void OnThrowStartHold()
        {
            if (_state != State.Wait)
                return;
            _state = State.Charge;
            _powerSlider.gameObject.SetActive(true);
            StartCoroutine(Fill());
        }

        public async void OnThrowFinishHold()
        {
            if(_state!=State.Charge)
                return;
            _state = State.Cooldown;
            StopAllCoroutines();
            _powerSlider.gameObject.SetActive(false);

            //set visual button interactable
            _throwButton.interactable = false;

            await _personage.ThrowBomb(_selectedBomb, _strength);
            
            _throwButton.interactable = true;
            _state = State.Wait;
        }

        private IEnumerator Fill()
        {
            _strength = 0;
            while (_strength < 1)
            {
                _strength += Time.deltaTime;
                _powerSlider.value = _strength;
                yield return null;
            }

            _powerSlider.value = _strength = 1;
        }
        
        private enum State
        {
            Wait,
            Charge,
            Cooldown
        }
    }
}