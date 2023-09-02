using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private List<BaseWindow> windows;

        private void Awake()
        {
            foreach (var window in windows)
            {
                window.Close();
            }
        }

        public void OpenWindow(string windowId)
        {
            OpenWindow(windowId, default);
        }
        
        public void OpenWindow(string windowId, object windowParam)
        {
            var window = GetWindow(windowId);
            if (window != null)
            {
                window.Open();
                if (windowParam != default)
                    window.SetParam(windowParam);
            }
        }

        public void CloseWindow(string windowId)
        {
            var window = GetWindow(windowId);
            if (window != null)
                window.Close();
        }

        private BaseWindow GetWindow(string windowId)
        {
            var window = windows.FirstOrDefault(w => string.Equals(w.gameObject.name, windowId, StringComparison.CurrentCultureIgnoreCase));
            if (window != default)
                return window;
            Debug.LogError($"Window with id {windowId} not found.");
            return null;

        }
    }
}
