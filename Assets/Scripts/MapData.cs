using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.tilesForComputerPlayer = allTiles;
        GameManager.Instance.maxStep = allTiles.Count;
    }

    [SerializeField]
    private List<Tile> allTiles;
}
