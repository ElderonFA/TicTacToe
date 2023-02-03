using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header ("Objects")]
    [SerializeField]
    private GameObject playMenuObject;
    
    [Header ("Canvas group")]
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;
    [SerializeField]
    private CanvasGroup playMenuCanvasGroup;

    [Header ("Buttons")] 
    [SerializeField] 
    private Button playButton;
    [SerializeField] 
    private Button exitButton;
    [Space]
    [SerializeField] 
    private Button startButton;
    [SerializeField] 
    private Button backToMenuButton;

    [Header ("Texts")]
    [SerializeField] 
    private Text currentMapSizeText;
    [SerializeField] 
    private Text currentGameModeText;
    [SerializeField] 
    private Text warningText;

    private MapSizeType _mapSizeType = MapSizeType.None;
    private GameModeType _gameModeType = GameModeType.None;

    public static Action<MapSizeType> updateMapSizeTypeText;
    public static Action<GameModeType> updateGamemodeTypeText;

    private bool gameIsPaused;

    public void Initialize()
    {
        playButton.onClick.AddListener(OpenPlaySettings);
        exitButton.onClick.AddListener(GameManager.Instance.CloseApp);
        
        startButton.onClick.AddListener(StartGame);
        backToMenuButton.onClick.AddListener(BackToMenu);

        updateMapSizeTypeText += ChangeMapSizeType;
        updateGamemodeTypeText += ChangeGameModeType;

        GameManager.Instance.exitGameEvent += OpenMainMenu;
    }

    private void OpenMainMenu()
    {
        BackToMenu();
        UIHelper.showCanvasEvent?.Invoke(mainMenuCanvasGroup, gameObject.transform.gameObject);
    }

    private void OpenPlaySettings()
    {
        UIHelper.hideCanvasEvent?.Invoke(mainMenuCanvasGroup, gameObject);
        UIHelper.showCanvasEvent?.Invoke(playMenuCanvasGroup, playMenuObject);
    }

    private void BackToMenu()
    {
        UIHelper.showCanvasEvent?.Invoke(mainMenuCanvasGroup, gameObject);
        UIHelper.hideCanvasEvent?.Invoke(playMenuCanvasGroup, playMenuObject);

        currentGameModeText.text = "";
        currentMapSizeText.text = "";

        _gameModeType = GameModeType.None;
        _mapSizeType = MapSizeType.None;
    }

    private void StartGame()
    {
        if (_mapSizeType == MapSizeType.None)
        {
            UIHelper.showWarning?.Invoke(warningText, "Chose map size!");
            return;
        }
        if (_gameModeType == GameModeType.None)
        {
            UIHelper.showWarning?.Invoke(warningText, "Chose gamemode!");
            return;
        }

        UIHelper.hideCanvasEvent?.Invoke(playMenuCanvasGroup, playMenuObject);
        
        GameManager.Instance.startGameEvent(_mapSizeType, _gameModeType);
    }
    
    private void ChangeMapSizeType(MapSizeType mapSizeType)
    {
        _mapSizeType = mapSizeType;
        currentMapSizeText.text = mapSizeType switch
        {
            MapSizeType.ThreeXThree => " 3x3",
            MapSizeType.FiveXFive => " 5x5",
            _ => ""
        };
    }
    
    private void ChangeGameModeType(GameModeType gameModeType)
    {
        _gameModeType = gameModeType;
        currentGameModeText.text = gameModeType switch
        {
            GameModeType.PvsC => " Player VS Computer",
            GameModeType.PvsP => " Player VS Player",
            GameModeType.CvsC => " Computer VS Computer",
            _ => ""
        };
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        backToMenuButton.onClick.RemoveAllListeners();
        
        startButton.onClick.RemoveAllListeners();
        
        GameManager.Instance.exitGameEvent -= OpenMainMenu;
    }
}

[Serializable]
public enum MapSizeType
{
    None = 0,
    ThreeXThree = 1,
    FiveXFive = 2,
}

[Serializable]
public enum GameModeType
{
    None = 0,
    PvsC = 1,
    PvsP = 2,
    CvsC = 3,
}
