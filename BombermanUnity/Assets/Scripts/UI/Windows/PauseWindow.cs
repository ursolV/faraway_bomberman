using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseWindow : BaseWindow
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _homeButton;

        private void Awake()
        {
            _continueButton.onClick.AddListener(Close);
            _saveButton.onClick.AddListener(OnSaveClick);
            _quitButton.onClick.AddListener(OnQuitClick);
            _homeButton.onClick.AddListener(OnHomeClick);
        }

        public override void Open()
        {
            base.Open();
            _saveButton.interactable = true;
        }

        private void OnQuitClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnSaveClick()
        {
            GameManager.Instance.SaveProgress();
            _saveButton.interactable = false;
        }

        private void OnHomeClick()
        {
            GameManager.Instance.OpenStartScreen();
            Close();
        }
    }
}