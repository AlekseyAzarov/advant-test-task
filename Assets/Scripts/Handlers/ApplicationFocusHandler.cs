using System;
using UnityEngine;

namespace ClickerLogic
{
    public class ApplicationFocusHandler : MonoBehaviour
    {
        public event Action<bool> FocusChanged;

        private void OnApplicationFocus(bool focus)
        {
            FocusChanged?.Invoke(focus);
        }
    }
}
