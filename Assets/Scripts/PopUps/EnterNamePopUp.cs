using System;
using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public class EnterNamePopUp : PopUp
    {
        [SerializeField]
        private Button confirmButton;
        [SerializeField] 
        private InputField inputField;

        public Action<string> onConfirm;
        public Action onDestroy;

        private void Start()
        {
            confirmButton.onClick.AddListener(Confirm);
        }

        private void Confirm()
        {
            onConfirm?.Invoke(inputField.text);
            Close();
        }

        private void OnDestroy()
        {
            onDestroy?.Invoke();
            confirmButton.onClick.RemoveAllListeners();
        }
    }
}
