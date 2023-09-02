using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseWindow : BaseWindow
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button homeButton;

        private void Awake()
        {
            continueButton.onClick.AddListener(Close);
            saveButton.onClick.AddListener(OnSaveClick);
            quitButton.onClick.AddListener(OnQuitClick);
            homeButton.onClick.AddListener(OnHomeClick);
        }

        public override void Open()
        {
            base.Open();
            saveButton.interactable = true;
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
            saveButton.interactable = false;
        }

        private void OnHomeClick()
        {
            GameManager.Instance.OpenStartScreen();
            Close();
        }
    }
}