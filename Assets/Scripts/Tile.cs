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

    private void Start()
    {
        image = GetComponent<SpriteRenderer>();
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
}

[Serializable]
public class TilePosition
{
    public int linePos;
    public int columnPos;
}
