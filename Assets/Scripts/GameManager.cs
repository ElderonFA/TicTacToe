using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header ("Transforms")]
    [SerializeField] 
    private Transform mainCanvas;
    [SerializeField] 
    private Transform currentPlayerIndicator;
    
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
    
    [Header("PopUp")]
    [SerializeField] 
    private EnterNamePopUp enterNamePopUp;
    
    [Header("Texts")]
    [SerializeField] 
    private Text playerOneNameText;
    [SerializeField] 
    private Text playerTwoNameText;
    
    public Action<MapSizeType, GameModeType> startGameEvent;
    public Action exitGameEvent;

    private Action onFirstPlayerNameEnter;

    public static GameManager Instance;

    private GameObject currentMap;

    public bool gameIsPaused = true;

    private PlayersHandler playersHandler;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        startGameEvent += PrepareGame;
        exitGameEvent += ExitGame;

        if (Instance == null)
        {
            Instance = this;
        }
        
        menuController.Initialize();
        pauseController.Initialize();
        uiHelper.Initialize();

        playersHandler = new PlayersHandler();
        playersHandler.Initialize(Random.Range(0, 2));
        playersHandler.SetIndicatorData(currentPlayerIndicator, playerOneNameText.transform, playerTwoNameText.transform);
    }

    private void PrepareGame(MapSizeType mapSizeType, GameModeType gameModeType)
    {
        playerOneNameText.gameObject.SetActive(true);
        playerTwoNameText.gameObject.SetActive(true);
        
        switch (gameModeType)
        {
            case GameModeType.PvsC:
                var popUpEnterName = Instantiate(enterNamePopUp, mainCanvas);
                popUpEnterName.Show();
                
                popUpEnterName.onConfirm += SetFirstPlayer;
                popUpEnterName.onDestroy += CreateMapWithSizeType;
                
                playersHandler.SecondPlayer.ChangeToComputer();
                SetSecondPlayer("Computer");
                break;
            
            case GameModeType.PvsP:
                var popUpEnterNameFirst = Instantiate(enterNamePopUp, mainCanvas);
                popUpEnterNameFirst.SetHeaderText("Enter first player name:");
                
                var popUpEnterNameSecond = Instantiate(enterNamePopUp, mainCanvas);
                popUpEnterNameSecond.SetHeaderText("Enter second player name:");
                
                popUpEnterNameFirst.Show();
                popUpEnterNameFirst.onConfirm += SetFirstPlayer;
                popUpEnterNameFirst.onDestroy += popUpEnterNameSecond.Show;

                popUpEnterNameSecond.onConfirm += SetSecondPlayer;
                popUpEnterNameSecond.onDestroy += CreateMapWithSizeType;
                break;
            
            case GameModeType.CvsC:
                playersHandler.FirstPlayer.ChangeToComputer();
                SetFirstPlayer("Computer 1");
                playersHandler.SecondPlayer.ChangeToComputer();
                SetSecondPlayer("Computer 2");
                
                CreateMapWithSizeType();
                break;
        }

        void CreateMapWithSizeType()
        {
            currentMap = mapSizeType switch
            {
                MapSizeType.ThreeXThree => Instantiate(map3),
                MapSizeType.FiveXFive => Instantiate(map5),
                _ => currentMap
            };

            gameIsPaused = false;

            StartGameEvent();
        }

        void SetFirstPlayer(string nameFirst)
        {
            playersHandler.FirstPlayer.SetName(nameFirst);
            playersHandler.FirstPlayer.moveIcon = cross;
            
            playerOneNameText.text = nameFirst;
        }

        void SetSecondPlayer(string nameSecond)
        {
            playersHandler.SecondPlayer.SetName(nameSecond);
            playersHandler.SecondPlayer.moveIcon = circle;
            
            playerTwoNameText.text = nameSecond;
        }
    }

    private void StartGameEvent()
    {
        playersHandler.ShowPlayerIndicator();
    }

    public void MakeMove(Tile tile)
    {
        if (gameIsPaused)
        {
            return;
        }
        
        var currentPlayer = playersHandler.GetCurrentPlayer;
        tile.SetImage(currentPlayer.moveIcon);
        playersHandler.ChangeCurrentPlayer();
        playersHandler.UpdateIndicatorPosition();
    }

    private void HideGameInterface()
    {
        playerOneNameText.gameObject.SetActive(false);
        playerTwoNameText.gameObject.SetActive(false);

        playerOneNameText.text = "";
        playerTwoNameText.text = "";
        
        playersHandler.HideIndicator();
    }
    
    private void ExitGame()
    {
        HideGameInterface();
        Destroy(currentMap);
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
