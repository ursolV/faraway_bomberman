using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartClick);
            homeButton.onClick.AddListener(OnHomeClick);
        }

        public override void SetParam(object param)
        {
            base.SetParam(param);
            var result = (bool)param;
            resultText.text = result ? "You won!" : "You lost!";
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