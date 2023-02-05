using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController
{
    private TileDataPlayer[,] currentMapData;
    private int mapSize;
    
    private int lines;
    private int columns;
    private int countToWin;

    public WinController(int size)
    {
        currentMapData = new TileDataPlayer[size,size];
        mapSize = size;
        countToWin = size;
        
        lines = currentMapData.GetUpperBound(0) + 1;
        columns = currentMapData.Length / lines;
    }
    
    public void UpdateMapData(Tile tile, Player usedPlayer)
    {
        currentMapData[tile.TilePosition.linePos, tile.TilePosition.columnPos]= new TileDataPlayer(usedPlayer);

        var s = "";
        
        for (var i = 0; i < lines; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var item = currentMapData[i, j];
                
                s += item == null ? "|    Null    |": "| " + item.usedByPlayer.GetName + " |";
            }

            s += "\n";
        }
        
        Debug.Log(s);
    }
    
    public void CheckWin(int currentStep)
    {
        CheckHorizontalWin();
        CheckVerticalWin();
        CheckDiagonalUpperLeftWin();
        CheckDiagonalUpperRightWin();
        
        if (currentStep == mapSize * mapSize)
        {
            GameManager.Instance.DrawGameEvent?.Invoke();
        }
    }

    private void CheckHorizontalWin()
    {
        for (var i = 0; i < lines; i++)
        {
            Player lastPlayer = null;
            var winCounter = 0;
            
            for (var j = 0; j < columns; j++)
            {
                var item = currentMapData[i, j];

                if (item == null)
                {
                    continue;
                }
                
                if (lastPlayer == null)
                {
                    winCounter++;
                    lastPlayer = item.usedByPlayer;
                    continue;
                }

                if (lastPlayer != item.usedByPlayer)
                {
                    continue;
                }

                winCounter++;

                if (winCounter == countToWin)
                {
                    GameManager.Instance.WinGameEvent?.Invoke(lastPlayer);
                    
                    Debug.Log(
                        "Horizontal WIN! " + 
                        lastPlayer.GetName + " is winner! " +
                        "On horizontal line - " + i);
                    return;
                }
            }
        }
    }
    
    private void CheckVerticalWin()
    {
        for (var i = 0; i < lines; i++)
        {
            Player lastPlayer = null;
            var winCounter = 0;
            
            for (var j = 0; j < columns; j++)
            {
                var item = currentMapData[j, i];

                if (item == null)
                {
                    continue;
                }

                if (lastPlayer == null)
                {
                    winCounter++;
                    lastPlayer = item.usedByPlayer;
                    continue;
                }

                if (lastPlayer != item.usedByPlayer)
                {
                    continue;
                }

                winCounter++;

                if (winCounter == countToWin)
                {
                    GameManager.Instance.WinGameEvent?.Invoke(lastPlayer);
                    
                    Debug.Log(
                        "Vertical WIN! " + 
                        lastPlayer.GetName + " is winner! " +
                        "On vertical line - " + i);
                    
                    return;
                }
            }
        }
    }

    private void CheckDiagonalUpperLeftWin()
    {
        Player lastPlayer = null;
        var currentLineElement = 0;
        
        for (var i = 0; i < lines; i++)
        {
            var item = currentMapData[i, currentLineElement];
            if (item == null)
            {
                return;
            }

            if (lastPlayer != null && lastPlayer != item.usedByPlayer)
            {
                return;
            }

            lastPlayer = item.usedByPlayer;
            currentLineElement++;
        }
        
        GameManager.Instance.WinGameEvent?.Invoke(lastPlayer);
        
        Debug.Log("Diagonal UpperLeft WIN! " + lastPlayer.GetName + " is winner!");
    }

    private void CheckDiagonalUpperRightWin()
    {
        Player lastPlayer = null;
        var currentLineElement = columns - 1;
        
        for (var i = 0; i < lines; i++)
        {
            var item = currentMapData[i, currentLineElement];
            if (item == null)
            {
                return;
            }

            if (lastPlayer != null && item.usedByPlayer != lastPlayer)
            {
                return;
            }

            lastPlayer = item.usedByPlayer;
            currentLineElement--;
        }
        
        GameManager.Instance.WinGameEvent?.Invoke(lastPlayer);
        
        Debug.Log("Diagonal UpperRight WIN! " + lastPlayer.GetName + " is winner!");
    }

    public void ReloadMapData()
    {
        currentMapData = new TileDataPlayer[mapSize, mapSize];
    }
}

public class TileDataPlayer
{
    public Player usedByPlayer;

    public TileDataPlayer(Player valuePlayer)
    {
        usedByPlayer = valuePlayer;
    }
}
