using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayersHandler
{
    public Player FirstPlayer;
    public Player SecondPlayer;
    
    private Player currentPlayer;
    public Player GetCurrentPlayer => currentPlayer;
    
    private int currentPlayerIndex;

    private Transform playerIndicatorTransform;
    private Vector2 startIndicatorPosition;
    private List<Vector2> indicatorPositions = new List<Vector2>();
    
    public void Initialize(int currentPlayerIndex)
    {
        FirstPlayer = new Player();
        SecondPlayer = new Player();
        currentPlayer = currentPlayerIndex == 0 ? FirstPlayer : SecondPlayer;
    }
    
    public void ChangeCurrentPlayer()
    {
        currentPlayerIndex = currentPlayerIndex == 0 ? 1 : 0;
        currentPlayer = currentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
    }

    public void SetIndicatorData(
        Transform newPlayerIndicatorTransform, 
        Transform playerOneTextPosition,
        Transform playerTwoTextPosition)
    {
        playerIndicatorTransform = newPlayerIndicatorTransform;
        
        startIndicatorPosition = playerIndicatorTransform.transform.position;
        var indicatorPositionOne = new Vector2(startIndicatorPosition.x, playerOneTextPosition.position.y);
        indicatorPositions.Add(indicatorPositionOne);
        var indicatorPositionTwo = new Vector2(startIndicatorPosition.x, playerTwoTextPosition.position.y);
        indicatorPositions.Add(indicatorPositionTwo);    
    }
    
    public void ShowPlayerIndicator()
    {
        playerIndicatorTransform.gameObject.SetActive(true);
        UpdateIndicatorPosition();
    }

    public void UpdateIndicatorPosition()
    {
        playerIndicatorTransform.position = indicatorPositions[currentPlayerIndex];
    }

    public void HideIndicator()
    {
        playerIndicatorTransform.gameObject.SetActive(false);
    }
}
