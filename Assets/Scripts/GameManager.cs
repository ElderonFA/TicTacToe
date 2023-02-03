using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Controllers")]
    [SerializeField] 
    private MenuController menuController;
    [SerializeField] 
    private PauseController pauseController;
    [SerializeField] 
    private UIHelper uiHelper;
    
    [Header("Maps")]
    [SerializeField] 
    private GameObject map3;
    [SerializeField] 
    private GameObject map5;
    
    [Header("Images")]
    [SerializeField] 
    private Sprite cross;
    [SerializeField] 
    private Sprite circle;
    
    public Action<MapSizeType, GameModeType> startGameEvent;
    public Action exitGameEvent;

    public static GameManager Instance;

    private GameObject currentMap;

    public bool gameIsPaused;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        startGameEvent += StartGame;
        exitGameEvent += ExitGame;

        if (Instance == null)
        {
            Instance = this;
        }
        
        menuController.Initialize();
        pauseController.Initialize();
        uiHelper.Initialize();
    }

    private void StartGame(MapSizeType mapSizeType, GameModeType gameModeType)
    {
        currentMap = mapSizeType switch
        {
            MapSizeType.ThreeXThree => Instantiate(map3),
            MapSizeType.FiveXFive => Instantiate(map5),
            _ => currentMap
        };
    }

    public void MakeMove(Tile tile)
    {
        if (gameIsPaused)
        {
            return;
        }
        
        tile.SetImage(cross);
    }
    
    private void ExitGame()
    {
        Destroy(currentMap);
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
