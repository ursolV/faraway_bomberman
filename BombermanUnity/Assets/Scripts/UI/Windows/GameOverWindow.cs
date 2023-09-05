using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _homeButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartClick);
            _homeButton.onClick.AddListener(OnHomeClick);
        }

        public override void SetParam(object param)
        {
            base.SetParam(param);
            var result = (bool)param;
            _resultText.text = result ? "You won!" : "You lost!";
        }

        private void OnHomeClick()
        {
            GameManager.Instance.OpenStartScreen();
            Close();
        }

        private void OnRestartClick()
        {
            GameManager.Instance.RestartLocation();
            Close();
        }
    }
}