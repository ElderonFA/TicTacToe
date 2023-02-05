using System;
using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public class WinPopUp : PopUp
    {
        [SerializeField] 
        private Button replayButton;
        [SerializeField] 
        private Button exitButton;

        private void Start()
        {
            replayButton.onClick.AddListener(Replay);
            exitButton.onClick.AddListener(Exit);
        }

        private void Exit()
        {
            GameManager.Instance.exitGameEvent?.Invoke();
            Close();
        }

        private void Replay()
        {
            GameManager.Instance.replayGameEvent?.Invoke();
            Close();
        }

        private void OnDestroy()
        {
            replayButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}
