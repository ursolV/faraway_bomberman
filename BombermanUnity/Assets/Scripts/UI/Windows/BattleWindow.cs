using System;
using System.Collections;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattleWindow : BaseWindow
    {
        [SerializeField] private Button throwButton;
        [SerializeField] private Slider powerSlider;
        [SerializeField] private Image selectedBomb;
        [SerializeField] private Sprite[] bombSprites;

        private float _power;
        private string _selectedBomb;
        private State _state;

        public void OnSelectBomb(string bombId)
        {
            _selectedBomb = bombId;
            selectedBomb.sprite = bombSprites.FirstOrDefault(s =>
                string.Equals(s.name, bombId, StringComparison.CurrentCultureIgnoreCase));
        }
        
        public void OnThrowStartHold()
        {
            if (_state != State.Wait)
                return;
            _state = State.Charge;
            powerSlider.gameObject.SetActive(true);
            StartCoroutine(Fill());
        }

        public async void OnThrowFinishHold()
        {
            if(_state!=State.Charge)
                return;
            _state = State.Cooldown;
            StopAllCoroutines();
            powerSlider.gameObject.SetActive(false);

            //set visual button interactable
            throwButton.interactable = false;
            await GameManager.Instance.LocationManager.Throw(_selectedBomb, _power);
            throwButton.interactable = true;
            _state = State.Wait;
        }

        private IEnumerator Fill()
        {
            _power = 0;
            while (_power < 1)
            {
                _power += Time.deltaTime;
                powerSlider.value = _power;
                yield return null;
            }

            powerSlider.value = _power = 1;
        }
        
        private enum State
        {
            Wait,
            Charge,
            Cooldown
        }
    }
}