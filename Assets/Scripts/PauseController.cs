using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header ("Buttons")]
    [SerializeField] 
    private Button pauseButton;
    [SerializeField] 
    private Button exitFromGameButton;
    [SerializeField] 
    private Button exitPauseButton;

    [Header("CanvasGroup")] 
    [SerializeField]
    private CanvasGroup pauseCanvasGroup;
    
    public void Initialize()
    {
        pauseButton.onClick.AddListener(ShowPause);
        exitFromGameButton.onClick.AddListener(ExitFromGame);
        exitPauseButton.onClick.AddListener(HidePause);
    }
    
    private void ShowPause()
    {
        UIHelper.showCanvasEvent?.Invoke(pauseCanvasGroup, null);
        GameManager.Instance.gameIsPaused = true;
    }

    private void HidePause()
    {
        UIHelper.hideCanvasEvent?.Invoke(pauseCanvasGroup, null);
        GameManager.Instance.gameIsPaused = false;
    }
    
    private void ExitFromGame()
    {
        UIHelper.hideCanvasEvent?.Invoke(pauseCanvasGroup, null);
        GameManager.Instance.ExitGameEvent?.Invoke();
        GameManager.Instance.gameIsPaused = false;
    }

    private void OnDestroy()
    {
        pauseButton.onClick.RemoveAllListeners();
        exitFromGameButton.onClick.RemoveAllListeners();
        exitPauseButton.onClick.RemoveAllListeners();
    }
}
