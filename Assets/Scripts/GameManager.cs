using System;
using System.Collections;
using System.Collections.Generic;
using PopUps;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header ("Transforms")]
    [SerializeField] 
    private Transform mainCanvasTransform;
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
    [SerializeField] 
    private WinPopUp winPopUp;

    [Header("Canvas")] 
    [SerializeField] 
    private CanvasGroup gameInterfaceCanvas;

    [Header("Texts")]
    [SerializeField] 
    private Text playerOneNameText;
    [SerializeField] 
    private Text playerTwoNameText;
    [SerializeField] 
    private List<Text> playersWinTexts;
    
    public Action<MapSizeType, GameModeType> startGameEvent;
    public Action exitGameEvent;
    public Action replayGameEvent;
    public Action<Player> winGameEvent;

    private Action onFirstPlayerNameEnter;
    private Action<Tile, Player> onMakeMove;

    public static GameManager Instance;
    [HideInInspector]
    public bool gameIsPaused = true;

    private GameObject currentMap;

    private PlayersHandler playersHandler;
    private WinController winController;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        startGameEvent += PrepareGame;
        exitGameEvent += ExitGame;

        winGameEvent += WinGame;
        replayGameEvent += ReplayGame;

        if (Instance == null)
        {
            Instance = this;
        }
        
        menuController.Initialize();
        pauseController.Initialize();
        uiHelper.Initialize();
    }

    private void PrepareGame(MapSizeType mapSizeType, GameModeType gameModeType)
    {
        playerOneNameText.gameObject.SetActive(true);
        playerTwoNameText.gameObject.SetActive(true);
        
        playersHandler = new PlayersHandler(Random.Range(0, 2));
        playersHandler.SetIndicatorData(currentPlayerIndicator, playerOneNameText.transform, playerTwoNameText.transform);
        
        switch (gameModeType)
        {
            case GameModeType.PvsC:
                var popUpEnterName = Instantiate(enterNamePopUp, mainCanvasTransform);
                popUpEnterName.Show();
                
                popUpEnterName.onConfirm += SetFirstPlayer;
                popUpEnterName.onDestroy += CreateMapWithSizeType;
                
                playersHandler.SecondPlayer.ChangeToComputer();
                SetSecondPlayer("Computer");
                break;
            
            case GameModeType.PvsP:
                var popUpEnterNameFirst = Instantiate(enterNamePopUp, mainCanvasTransform);
                popUpEnterNameFirst.SetHeaderText("Enter first player name:");
                
                var popUpEnterNameSecond = Instantiate(enterNamePopUp, mainCanvasTransform);
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
            switch (mapSizeType)
            {
                case MapSizeType.ThreeXThree:
                    currentMap = Instantiate(map3);
                    StartGame(3);
                    break;
                case MapSizeType.FiveXFive:
                    currentMap = Instantiate(map5);
                    StartGame(5);
                    break;
            }
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

    private void StartGame(int size)
    {
        gameIsPaused = false;
        
        winController = new WinController(size);

        onMakeMove += winController.UpdateMapData;

        foreach (var playerText in playersWinTexts)
        {
            playerText.text = "0";
        }

        playersHandler.UpdateIndicatorPosition();
        UIHelper.showCanvasEvent?.Invoke(gameInterfaceCanvas, gameInterfaceCanvas.gameObject);
    }

    public void MakeMove(Tile tile)
    {
        if (gameIsPaused)
        {
            return;
        }

        var currentPlayer = playersHandler.GetCurrentPlayer;

        onMakeMove?.Invoke(tile, currentPlayer);
        
        playersHandler.ChangeCurrentPlayer();

        tile.SetImage(currentPlayer.moveIcon);
        
        winController.CheckWin();
    }
    
    private void WinGame(Player winPlayer)
    {
        gameIsPaused = true;

        winPlayer.CountWin++;
        UpdateCounterPlayer(winPlayer);

        var popupObject = Instantiate(winPopUp, mainCanvasTransform);
        
        popupObject.SetHeaderText("Winner is " + winPlayer.GetName + "!");
        popupObject.Show();
    }

    private void UpdateCounterPlayer(Player player)
    {
        playersWinTexts[player.index].text = player.CountWin.ToString();
    }

    private void ReplayGame()
    {
        gameIsPaused = false;
        
        Tile.reloadTile?.Invoke();

        winController.ReloadMapData();
    }

    private void HideGameInterface()
    {
        UIHelper.hideCanvasEvent?.Invoke(gameInterfaceCanvas, gameInterfaceCanvas.gameObject);
    }

    private void ExitGame()
    {
        HideGameInterface();
        Destroy(currentMap);

        playersHandler = null;
        
        onMakeMove -= winController.UpdateMapData;
        winController = null;
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
