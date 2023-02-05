using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public TilePosition TilePosition;
    
    private SpriteRenderer image;
    [HideInInspector]
    public bool isUsed;

    public static Action reloadTile;

    private void Start()
    {
        image = GetComponent<SpriteRenderer>();

        reloadTile += ReloadTile;
    }

    private void ReloadTile()
    {
        isUsed = false;
        SetImage(null);
        
        GameManager.Instance.tilesForComputerPlayer.Add(this);
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    private void OnMouseDown()
    {
        if (isUsed)
        {
            return;
        }

        isUsed = true;
        GameManager.Instance.MakeMove(this);
    }

    private void OnDestroy()
    {
        reloadTile -= ReloadTile;
    }
}

[Serializable]
public class TilePosition
{
    public int linePos;
    public int columnPos;
}
