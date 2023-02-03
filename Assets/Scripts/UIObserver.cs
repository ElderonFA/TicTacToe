using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObserver : MonoBehaviour
{
    [Header ("Objects")]
    [SerializeField]
    private GameObject mainMenuObject;
    [SerializeField]
    private GameObject playMenuObject;
    [SerializeField]
    private GameObject gameMenuObject;

    [Header ("Buttons")] 
    [SerializeField] 
    private Button playButton;
    [SerializeField] 
    private Button exitButton;
    [SerializeField] 
    private Button backToMenuButton;
    [Space]
    [SerializeField] 
    private Button startButton;
    /*[SerializeField] 
    private Button pauseButton;*/
    
    [Header ("Texts")]
    [SerializeField] 
    private Text currentMapSizeText;
    [SerializeField] 
    private Text currentGameModeText;
    [SerializeField] 
    private Text errorText;
    
    /*[Header ("Changing Images")]
    [SerializeField] 
    private Image pauseImage;*/

    private MapSizeType _mapSizeType = MapSizeType.None;
    private GameModeType _gameModeType = GameModeType.None;

    public static Action<MapSizeType> updateMapSizeTypeText;
    public static Action<GameModeType> updateGamemodeTypeText;

    private void Awake()
    {
        playButton.onClick.AddListener(PlaySettings);
        exitButton.onClick.AddListener(ExitApplication);
        backToMenuButton.onClick.AddListener(BackToMenu);
        
        startButton.onClick.AddListener(StartGame);

        updateMapSizeTypeText += ChangeMapSizeType;
        updateGamemodeTypeText += ChangeGameModeType;
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

    private void BackToMenu()
    {
        mainMenuObject.SetActive(true);
        playMenuObject.SetActive(false);

        currentGameModeText.text = "";
        currentMapSizeText.text = "";

        _gameModeType = GameModeType.None;
        _mapSizeType = MapSizeType.None;
    }

    private void PlaySettings()
    {
        mainMenuObject.SetActive(false);
        playMenuObject.SetActive(true);
    }
    
    private void StartGame()
    {
        if (_mapSizeType == MapSizeType.None)
        {
            ShowError("Chose map size!");
            return;
        }

        if (_gameModeType == GameModeType.None)
        {
            ShowError("Chose gamemode!");
        }
    }

    private void Pause()
    {
        gameMenuObject.SetActive(true);
    }

    private void ExitApplication()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        backToMenuButton.onClick.RemoveAllListeners();
        
        startButton.onClick.RemoveAllListeners();
    }

    private void ShowError(string error)
    {
        errorText.text = error;
        StartCoroutine(ShowErrorCoroutine());
    }

    private IEnumerator ShowErrorCoroutine()
    {
        var alph = 1f;
        errorText.color = new Color(1f, 0f, 0f, alph);

        while (alph > 0)
        {
            alph -= Time.deltaTime;
            errorText.color = new Color(1f, 0f, 0f, alph);
            yield return null;
        }
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
